using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace paiza.DB.Entry
{
    class OfferLable
    {
        private int offerid;
        private ArrayList label;

        public void setOfferid(int id)
        {
            this.offerid = id;
        }
        public int getOfferid()
        {
            return this.offerid;
        }

        public void setLabel(ArrayList str)
        {
            this.label = str;
        }

        public ArrayList getLabel()
        {
            return this.label;
        }

        public void InsertDb()
        {
            try
            {
                if (offerid != 0 && this.label.Count!=0)
                {
                    foreach (string labelitem in this.label) {
                        DBHelper help = new DBHelper();
                        string sql = "insert into offer_label(offerid,label) values("
                            + this.offerid + ","
                            + "'" + labelitem + "')";
                        help.Insert(sql);
                    }
                   
                }
            }
            catch (Exception e)
            {
                Log.WriteMsg("Label ERROR:offerid" + this.offerid + "\t" + e.Message);
            }
        }
    }
}
