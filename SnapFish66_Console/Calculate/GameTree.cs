﻿using SnapFish66_Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculate
{
    public class GameTree
    {
        //public static int GenerateTimeSum;

        public static ConcurrentDictionary<Transposition, sbyte> TranspositionTable = new ConcurrentDictionary<Transposition, sbyte>();

        [Flags]
        public enum PossibleSteps
        {
            NONE = 0,
            A1 = 1,
            A2 = 2,
            A3 = 4,
            A4 = 8,
            B1 = 16,
            B2 = 32,
            B3 = 64,
            B4 = 128,
            B5 = 256
        }

        private readonly object lockobject = new object();

        public Node root;

        public static Database database;

        public static DateTime started;

        //Key is the depth
        public static int[] VisitedNodes = new int[21];
        public static int[] ReadNodes = new int[21];
        public static int[] SavedNodes = new int[21];

        //Estimated values of actions
        public List<float> a1;
        public List<float> a2;
        public List<float> a3;
        public List<float> a4;
        public List<float> a5;
        public List<float> b1;
        public List<float> b2;
        public List<float> b3;
        public List<float> b4;
        public List<float> b5;
        public List<float> cover;

        //Easy access to the lists above
        public List<List<float>> estimatedLists;

        //The averages of the lists above repectively, CalcAverages() sets it
        public List<float> averages;

        //public static List<Node> allNodes;

        public int possibleSubroots;
        public ConcurrentStack<Node> subroots;

        public GameTree(State s)
        {
            started = DateTime.Now;

            a1 = new List<float>();
            a2 = new List<float>();
            a3 = new List<float>();
            a4 = new List<float>();
            a5 = new List<float>();
            b1 = new List<float>();
            b2 = new List<float>();
            b3 = new List<float>();
            b4 = new List<float>();
            b5 = new List<float>();
            cover = new List<float>();
            estimatedLists = new List<List<float>> { a1, a2, a3, a4, a5, b1, b2, b3, b4, b5, cover };

            averages = new List<float>();
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                averages.Add(float.NaN);
            }
            subroots = new ConcurrentStack<Node>();

            byte round = (byte)(CardToInt(s.adown) + CardToInt(s.bdown) + s.atookCount + s.btookCount);

            root = new Node(s, 0, round);
            root.SetMaximizer();
            root.state.InitPoints();

            //allNodes = new List<Node> { root };
        }

        private int CardToInt(Card c)
        {
            if(c==null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private int CalcPossibleSubroots()
        {
            int cardsInBHand = CardToInt(root.state.b1) + CardToInt(root.state.b2) + CardToInt(root.state.b3) + CardToInt(root.state.b4) + CardToInt(root.state.b5);
            int cardsInDeck = 20 - cardsInBHand - CardToInt(root.state.dbottom) - CardToInt(root.state.adown) - CardToInt(root.state.bdown) - root.state.atookCount - root.state.btookCount - CardToInt(root.state.a1) - CardToInt(root.state.a2) - CardToInt(root.state.a3) - CardToInt(root.state.a4) - CardToInt(root.state.a5);

            // count is sum unknown factorial per in hand factorial
            int count = 1;
            for (int i = cardsInBHand+1; i <= cardsInBHand+cardsInDeck; i++)
            {
                count *= i;
            }

            return count;
        }
        
        public void Reset(State state)
        {
            started = DateTime.Now;
            foreach (var item in estimatedLists)
            {
                item.Clear();
            }
            //allNodes.Clear();
            subroots.Clear();
            averages.Clear();
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                averages.Add(float.NaN);
            }
            root = new Node(state, 0, 0);
            root.SetMaximizer();
            //allNodes.Add(root);
            VisitedNodes = new int[21];
            ReadNodes = new int[21];
            SavedNodes = new int[21];
        }

        private ReturnObject GenerateReturnObject(int calculatedSubroots, bool finished)
        {
            State s = subroots.Last().state; //Can be any subroot

            ReturnObject returnObject = new ReturnObject();

            if(s.a1!= null)
            {
                returnObject.a1Name = s.a1.ID;
                returnObject.a1Value = (float)Math.Round(averages[0], 2);
            }
            else
            {
                returnObject.a1Name = "";
            }
            if (s.a2 != null)
            {
                returnObject.a2Name = s.a2.ID;
                returnObject.a2Value = (float)Math.Round(averages[1], 2);
            }
            else
            {
                returnObject.a2Name = "";
            }
            if (s.a3 != null)
            {
                returnObject.a3Name = s.a3.ID;
                returnObject.a3Value = (float)Math.Round(averages[2], 2);
            }
            else
            {
                returnObject.a3Name = "";
            }
            if (s.a4 != null)
            {
                returnObject.a4Name = s.a4.ID;
                returnObject.a4Value = (float)Math.Round(averages[3], 2);
            }
            else
            {
                returnObject.a4Name = "";
            }
            if (s.a5 != null)
            {
                returnObject.a5Name = s.a5.ID;
                returnObject.a5Value = (float)Math.Round(averages[4], 2);
            }
            else
            {
                returnObject.a5Name = "";
            }

            returnObject.calculatedGames = calculatedSubroots;
            returnObject.allGames = possibleSubroots;

            returnObject.isFinished = finished;

            return returnObject;
        }

        public void Calculate(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            possibleSubroots = CalcPossibleSubroots();

            int calculatedSubroots = 0;

            //Does not create new database, only opens it
            database = new Database();

            ConcurrentStack<Node> partialSubroots = new ConcurrentStack<Node>();

            bool stopping = false;

            while (subroots.Count < possibleSubroots && !stopping)
            {
                for (int i = 0; i < 1000 && subroots.Count < possibleSubroots; i++)
                {
                    Node n = CreateNewSubroot();
                    subroots.Push(n);
                    partialSubroots.Push(n);
                    //Console.Clear();
                    //Console.WriteLine("Created subroots: " + subroots.Count + "/" + possibleSubroots);
                }

                started = DateTime.Now;

                Dictionary<Node, List<Node>> children = new Dictionary<Node, List<Node>>();

                DateTime last = DateTime.Now;

                Parallel.ForEach(partialSubroots, (subroot, forEachState) =>
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        stopping = true;
                        //return;
                    }
                    else
                    {
                        Interlocked.Increment(ref calculatedSubroots);

                        //Add children of different actions (will be root of alphabeta)
                        for (byte i = 0; i < 5; i++) //without cover
                        {
                            Node child = new Node(subroot.state.Copy(), i, (byte)(subroot.depth + 1));
                            bool success = child.state.StepOne(child.state, i);
                            child.SetMaximizer();

                            if (success)
                            {
                                lock (lockobject)
                                {
                                    if (!children.ContainsKey(subroot))
                                    {
                                        children[subroot] = new List<Node>();
                                    }
                                }
                                children[subroot].Add(child);
                            }
                        }

                        for (int i = 0; i < children[subroot].Count; i++)
                        {
                            Node child = children[subroot][i];

                            float value = child.AlphaBeta(-3, 3);

                            lock (lockobject)
                            {
                                estimatedLists[child.actionBefore].Add(value);
                                CalcAverages();
                            }

                            //We wont need the children of the subroot
                            children[subroot].RemoveAt(i);
                            i--;
                        }

                        //Writing data to console
                        if ((DateTime.Now - last).TotalMilliseconds > 1000)
                        {
                            //WriteDataToConsole(calculatedSubroots);
                            worker.ReportProgress(0, GenerateReturnObject(calculatedSubroots, false));
                            last = DateTime.Now;
                        }
                    }
                 }) ;

                //WriteDataToConsole(subroots.Count);
                partialSubroots.Clear();
            }

            worker.ReportProgress(100, GenerateReturnObject(calculatedSubroots, true));

            database.CloseDB();
        }

        private void CalcAverages()
        {
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                if (estimatedLists[i].Count > 0)
                {
                    averages[i] = estimatedLists[i].Average();
                }
            }
        }
        
        //Creates random root node by giving value to unknown cards, and adds it to subroots
        private Node CreateNewSubroot()
        {
            bool found = false;
            byte round = (byte)(CardToInt(root.state.adown) + CardToInt(root.state.bdown) + root.state.atookCount + root.state.btookCount);
            Node newNode = null;

            //DateTime begin = DateTime.Now;

            while(!found)
            {
                newNode = new Node(root.state.GenerateRandom(), 0, round);
                found = true;
                foreach (Node n in subroots)
                {
                    if(newNode.state.Equals(n.state))
                    {
                        found = false;
                        break;
                    }
                }
            }

            //DateTime end = DateTime.Now;

            //TimeSpan span = end - begin;

            //GenerateTimeSum += span.Milliseconds;

            //Console.WriteLine(GenerateTimeSum);
            return newNode;
        }
    }
}