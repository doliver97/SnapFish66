using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace SnapFish66_Console
{
    public class Database
    {
        public class Record
        {
            // char 0 : trump [M,P,T,Z]
            // char 1 : next player [A,B]
            // char 2-21 : place for each card from M2, M3, ... to ... Z10, Z11 ; 1 byte each
            // [
            //   0-8 : in deck (0 is the top card)
            //   D : bottom of deck
            //   A : in A's hand
            //   B : in B's hand
            //   E : A put down
            //   F : B put down
            //   G : taken by A
            //   H : taken by B
            // ]
            // char 22-25 : 20/40 of M,P,T,Z [A,B,X] ; X means nobody said it yet
            // char 26 : alpha + 3 as char
            // char 27 : beta + 3 as char
            [BsonId]
            public string Key { get; set; }
            // [-3 - +3]
            public sbyte Value { get; set; }
        }

        LiteDatabase database;

        //Opening the database
        public Database()
        {
            database = new LiteDatabase("Database.db");
        }

        public void CloseDB()
        {
            database.Dispose();
        }

        public void AddToDB(string node, sbyte value)
        {
            Record rec = new Record
            {
                Key = node,
                Value = Convert.ToSByte(value)
            };

            // Get collection instance
            var col = database.GetCollection<Record>("Records");

            // Insert document to collection - if collection do not exists, create now
            col.Upsert(rec);

            // Create, if not exists, new index on Key field
            col.EnsureIndex(x => x.Key);

        }

        public int ReadFromDB(string node)
        {
            var col = database.GetCollection("Records");

            BsonDocument result = col.FindById(node);

            if (result == null)
            {
                return -100; //Error code
            }
            else
            {
                return result["Value"];
            }
        }

        

        
    }
}
