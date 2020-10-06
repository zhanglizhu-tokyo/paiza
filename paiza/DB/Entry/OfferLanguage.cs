using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace paiza.DB.Entry
{
    class OfferLanguage
    {
        private int offerid;
        private ArrayList language;

        public void setOfferid(int id)
        {
            this.offerid = id;
        }
        public int getOfferid()
        {
            return this.offerid;
        }

        public void setLanguage(ArrayList str)
        {
            this.language = str;
        }

        public ArrayList getLanguage()
        {
            return this.language;
        }

        public void InsertDb()
        {
            try
            {
                if (offerid != 0 && this.language.Count !=0)
                {
                    foreach (string lang in this.language)
                    {
                        DBHelper help = new DBHelper();
                        string sql = "insert into offer_language(offerid,language) values("
                        + this.offerid + ","
                        + "'" + lang + "')";
                        help.Insert(sql);
                    }
                    
                }
            }
            catch (Exception e)
            {
                Log.WriteMsg("language ERROR:offerid" + this.offerid + "\t" + e.Message);
            }
        }
    }
}
