//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LiteDB;

//namespace SnapFish66_Console
//{
//    public class Database
//    {
//        public class Record
//        {
//            // char 0 : trump [M,P,T,Z]
//            // char 1 : next player [A,B]
//            // char 2-21 : place for each card from M2, M3, ... to ... Z10, Z11 ; 1 byte each
//            // [
//            //   0-8 : in deck (0 is the top card)
//            //   D : bottom of deck
//            //   A : in A's hand
//            //   B : in B's hand
//            //   E : A put down
//            //   F : B put down
//            //   G : taken by A
//            //   H : taken by B
//            // ]
//            // char 22-25 : 20/40 of M,P,T,Z [A,B,X] ; X means nobody said it yet
//            [BsonId]
//            public string Key { get; set; }
//            // [-3 - +3]
//            public sbyte Value { get; set; }
//            // [0 - 20]
//            public byte Depth { get; set; }
//        }

//        public static List<string> IDs = new List<string> { "M2", "M3", "M4", "M10", "M11", "P2", "P3", "P4", "P10", "P11", "T2", "T3", "T4", "T10", "T11", "Z2", "Z3", "Z4", "Z10", "Z11" };


//        LiteDatabase database;

//        //Opening the database
//        public Database()
//        {
//            database = new LiteDatabase("Database.db");
//        }

//        public void CloseDB()
//        {
//            database.Dispose();
//        }

//        public void AddToDB(Node node)
//        {
//            Record rec = new Record
//            { 
//                Key = StringifyNode(node),
//                Value = Convert.ToSByte(node.value),
//                Depth = Convert.ToByte(node.depth)
//            };

//            // Get collection instance
//            var col = database.GetCollection<Record>("Records");

//            // Insert document to collection - if collection do not exists, create now
//            col.Upsert(rec);

//            // Create, if not exists, new index on Key field
//            col.EnsureIndex(x => x.Key);

//        }

//        public int ReadFromDB(Node node)
//        {
//            var col = database.GetCollection("Records");

//            BsonDocument result = col.FindById(StringifyNode(node));

//            if(result==null)
//            {
//                return -100; //Error code
//            }
//            else
//            {
//                return result["Value"];
//            }
//        }

//        //Helper for stringify
//        private void SetChar(ref char[] key, Card place, char val, State s, int i)
//        {
//            if (place!=null && place.ID == IDs[i])
//            {
//                key[i + 2] = val;
//            }
//        }

//        private string StringifyNode(Node node)
//        {
//            char[] key = new char[26];

//            State s = node.state;

//            key[0] = s.trump;
//            key[1] = s.isAnext ? 'A' : 'B';

//            //for each card
//            for (int i = 0; i < IDs.Count; i++)
//            {
//                //deck
//                long deck = s.deck;
//                int j = 0;
//                while (deck>0)
//                {
//                    if(Card.PopCardFromDeck(ref deck).ID == IDs[i])
//                    {
//                        key[i + 2] = Convert.ToChar(j+'0');
//                    }

//                    j++;
//                }

//                //dbottom
//                SetChar(ref key, s.dbottom, 'D', s, i);

//                //hands
//                SetChar(ref key, s.a1, 'A', s, i);
//                SetChar(ref key, s.a2, 'A', s, i);
//                SetChar(ref key, s.a3, 'A', s, i);
//                SetChar(ref key, s.a4, 'A', s, i);
//                SetChar(ref key, s.a5, 'A', s, i);
//                SetChar(ref key, s.b1, 'B', s, i);
//                SetChar(ref key, s.b2, 'B', s, i);
//                SetChar(ref key, s.b3, 'B', s, i);
//                SetChar(ref key, s.b4, 'B', s, i);
//                SetChar(ref key, s.b5, 'B', s, i);

//                //put down
//                SetChar(ref key, s.adown, 'E', s, i);
//                SetChar(ref key, s.bdown, 'F', s, i);

//                //taken
//                if(s.atook.HasFlag((State.CardSet)Card.GetCard(IDs[i]).value))
//                {
//                    key[i + 2] = 'G';
//                }
//                if (s.btook.HasFlag((State.CardSet)Card.GetCard(IDs[i]).value))
//                {
//                    key[i + 2] = 'H';
//                }
//            }

//            //20/40
//            if(s.AM20)
//            {
//                key[22] = 'A';
//            }
//            else if(s.BM20)
//            {
//                key[22] = 'B';
//            }
//            else
//            {
//                key[22] = 'X';
//            }

//            if (s.AP20)
//            {
//                key[23] = 'A';
//            }
//            else if (s.BP20)
//            {
//                key[23] = 'B';
//            }
//            else
//            {
//                key[23] = 'X';
//            }

//            if (s.AT20)
//            {
//                key[24] = 'A';
//            }
//            else if (s.BT20)
//            {
//                key[24] = 'B';
//            }
//            else
//            {
//                key[24] = 'X';
//            }

//            if (s.AZ20)
//            {
//                key[25] = 'A';
//            }
//            else if (s.BZ20)
//            {
//                key[25] = 'B';
//            }
//            else
//            {
//                key[25] = 'X';
//            }

//            return new string(key);
//        }
//    }
//}
