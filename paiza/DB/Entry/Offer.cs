using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paiza.DB.Entry
{
    class Offer
    {
        //ID
        public int ID;
        //会社ID
        public int CompanyID;
 
        //勤務地
        public string ADDRESS;
        //応募要件
        public string NECESSARY;
        //給与体系・詳細
        public string SALARY_INFO;
        //職務内容
        public string JOB_CONTENT;
        //ポジション
        public string POSITION;
        //募集人数
        public string RECRUIT_PERSONNUM;
        //配属部署人数
        public string DEPARTMENT_PERSONNUM;
        //想定年収
        public string YEAR_SALARY;
        //休日休暇
        public string RESTDAY;
        //勤務時間
        public string JOB_TIME;
        //諸手当
        public string ALLOWANCE;
        //インセンティブ
        public string INSETIVE;
        //昇給・昇格
        public string PROMOTE;
        //保険
        public string INSURANCE;
        //選考フロー
        public string APPLICATED_FLOW;
        //試用期間
        public string TRY_TIME;
    }
}
