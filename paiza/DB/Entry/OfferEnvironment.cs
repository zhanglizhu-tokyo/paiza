
using System;
namespace paiza.DB.Entry
{
    class OfferEnvironment
    {
        private int offerid;
        private string[] environment;

        public void setOfferid(int id){
            this.offerid = id;
        }
        public int getOfferid() {
            return this.offerid;
        }

        public void setEnvironment(string[] str) {
            this.environment = str;
        }

        public string[] getEnvironment() {
            return this.environment;
        }

        public void InsertDb() {
            try
            {
                if (offerid != 0 && this.environment.Length!=0)
                {
                    DBHelper help = new DBHelper();
                    foreach (string env in this.environment)
                    {
                        string sql = "insert into offer_environment(offerid,environment) values("
                        + this.offerid + ","
                        + "'" + env + "')";
                        help.Insert(sql);
                    }                    
                }
            }catch(Exception e){
                Log.WriteMsg("environment ERROR:offerid" + this.offerid + "\t" + e.Message);
            }
        }
    }
}
