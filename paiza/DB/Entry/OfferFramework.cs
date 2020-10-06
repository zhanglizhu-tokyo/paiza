using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace paiza.DB.Entry
{
    class OfferFramework
    {
        private int offerid;
        private ArrayList framework;

        public void setOfferid(int id)
        {
            this.offerid = id;
        }
        public int getOfferid()
        {
            return this.offerid;
        }

        public void setFramework(ArrayList str)
        {
            this.framework = str;
        }

        public ArrayList getFramework()
        {
            return this.framework;
        }

        public void InsertDb()
        {
            try
            {
                if (offerid != 0 && this.framework.Count != 0)
                {
                    foreach (string frameworkitem in this.framework) {
                        DBHelper help = new DBHelper();
                        string sql = "insert into offer_framework(offerid,framework) values("
                            + this.offerid + ","
                            + "'" + frameworkitem + "')";
                        help.Insert(sql);
                    }
                    
                }
            }
            catch (Exception e)
            {
                Log.WriteMsg("framework ERROR:offerid" + this.offerid + "\t" + e.Message);
            }
        }
    }
}
