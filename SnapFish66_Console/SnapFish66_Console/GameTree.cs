﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnapFish66_Console
{
    public class GameTree
    {
        //public static int GenerateTimeSum;

        private readonly object lockobject = new object();
        private readonly object lockobject2 = new object();

        public Node root;

        public static Database database;

        public static DateTime started;

        //Key is the depth
        public static int[] VisitedNodes = new int[21];
        public static int[] ReadNodes = new int[21];
        public static int[] SavedNodes = new int[21];

        //Estimated values of actions
        public List<double> a1;
        public List<double> a2;
        public List<double> a3;
        public List<double> a4;
        public List<double> a5;
        public List<double> b1;
        public List<double> b2;
        public List<double> b3;
        public List<double> b4;
        public List<double> b5;
        public List<double> cover;

        //Easy access to the lists above
        public List<List<double>> estimatedLists;

        //The averages of the lists above repectively, CalcAverages() sets it
        public List<double> averages;

        List<string> actionList;

        public static List<Node> allNodes;

        public int possibleSubroots;
        public List<Node> subroots;
        

        public GameTree(State s)
        {
            started = DateTime.Now;

            a1 = new List<double>();
            a2 = new List<double>();
            a3 = new List<double>();
            a4 = new List<double>();
            a5 = new List<double>();
            b1 = new List<double>();
            b2 = new List<double>();
            b3 = new List<double>();
            b4 = new List<double>();
            b5 = new List<double>();
            cover = new List<double>();
            estimatedLists = new List<List<double>> { a1, a2, a3, a4, a5, b1, b2, b3, b4, b5, cover };
            actionList = new List<string> { "A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "B4", "B5", "cover" };

            averages = new List<double>();
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                averages.Add(double.NaN);
            }
            subroots = new List<Node>();

            int round = CardToInt(s.adown) + CardToInt(s.bdown) + s.atook.Count + s.btook.Count;

            root = new Node(null, s, "", round);
            root.SetMaximizer();

            allNodes = new List<Node> { root };
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
            int cardsInDeck = 20 - cardsInBHand - CardToInt(root.state.dbottom) - CardToInt(root.state.adown) - CardToInt(root.state.bdown) - root.state.atook.Count - root.state.btook.Count - CardToInt(root.state.a1) - CardToInt(root.state.a2) - CardToInt(root.state.a3) - CardToInt(root.state.a4) - CardToInt(root.state.a5);

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
            allNodes.Clear();
            subroots.Clear();
            averages.Clear();
            for (int i = 0; i < estimatedLists.Count; i++)
            {
                averages.Add(double.NaN);
            }
            root = new Node(null, state, "", 0);
            root.SetMaximizer();
            allNodes.Add(root);
            VisitedNodes = new int[21];
            ReadNodes = new int[21];
            SavedNodes = new int[21];
        }

        public void Calculate()
        {
            possibleSubroots = CalcPossibleSubroots();

            int calculatedSubroots = 0;

            //Does not create new database, only opens it
            database = new Database();

            while (calculatedSubroots < possibleSubroots)
            {
                for (int i = 0; i < 1000 && subroots.Count<possibleSubroots; i++)
                {
                    Node subroot = CreateNewSubroot(); //Adds it automatically to subroots
                    //Console.Clear();
                    //Console.WriteLine("Created subroots: " + subroots.Count + "/" + possibleSubroots);
                }

                started = DateTime.Now;

                Dictionary<Node, List<Node>> children = new Dictionary<Node, List<Node>>();

                Parallel.ForEach(subroots, (subroot) =>
                {
                    calculatedSubroots++;

                //Add children of different actions (will be root of alphabeta)
                for (int i = 0; i < actionList.Count - 1; i++) //without cover
                {
                        Node child = new Node(subroot, subroot.state.Copy(), actionList[i], subroot.depth + 1);
                        bool success = child.state.Step(child.state, actionList[i]);
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
                        children[subroot][i].AlphaBeta(-3, 3);

                        lock (lockobject)
                        {
                            AddEstimatedValue(children[subroot][i]);
                            CalcAverages(); 
                        }

                    //We wont need the children of the subroot
                    children[subroot].RemoveAt(i);
                        i--;
                    }

                    //Writing data to console
                    lock (lockobject2)
                    {
                        //Console.Clear();
                        //Console.Write("Unknown card permutations: ");
                        //Console.WriteLine(calculatedSubroots + "/" + possibleSubroots);

                        //double estimatedSpeed = calculatedSubroots / (DateTime.Now - started).TotalSeconds; //permutations per second
                        //double finishduration = (possibleSubroots - calculatedSubroots) / estimatedSpeed;//finish will be after x seconds
                        //DateTime finishtime = DateTime.Now.AddSeconds(finishduration);

                        //Console.WriteLine();
                        //Console.WriteLine("Finish at: " + finishtime);

                        //Console.WriteLine();
                        //Console.WriteLine("Estimated values for cards:");
                        //State s = subroots[0].state; //Can be any subroot

                        //if (s.a1 != null)
                        //{
                        //    Console.WriteLine(s.a1.ID + " : " + Math.Round(averages[0], 2));
                        //}
                        //if (s.a2 != null)
                        //{
                        //    Console.WriteLine(s.a2.ID + " : " + Math.Round(averages[1], 2));
                        //}
                        //if (s.a3 != null)
                        //{
                        //    Console.WriteLine(s.a3.ID + " : " + Math.Round(averages[2], 2));
                        //}
                        //if (s.a4 != null)
                        //{
                        //    Console.WriteLine(s.a4.ID + " : " + Math.Round(averages[3], 2));
                        //}
                        //if (s.a5 != null)
                        //{
                        //    Console.WriteLine(s.a5.ID + " : " + Math.Round(averages[4], 2));
                        //} 
                    }
                });

                subroots.Clear();
            }

            database.CloseDB();
        }

        private void AddEstimatedValue(Node child)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                if(actionList[i]==child.actionBefore)
                {
                    estimatedLists[i].Add(child.value);
                    break;
                }
            }
            
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
            int round = CardToInt(root.state.adown) + CardToInt(root.state.bdown) + root.state.atook.Count + root.state.btook.Count;
            Node newNode = new Node(null,root.state.GenerateRandom(),"",round);

            //DateTime begin = DateTime.Now;

            while(!found)
            {
                newNode = new Node(null, root.state.GenerateRandom(), "", round);
                found = true;
                foreach (Node n in subroots)
                {
                    if(newNode.state.IsSame(n.state))
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

            subroots.Add(newNode);
            return newNode;
        }
    }
}