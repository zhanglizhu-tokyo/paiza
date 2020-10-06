using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using paiza.DB;
using paiza.DB.Entry;



namespace paiza
{
    public partial class MainFrame : Form
    {
        //まだ検索してないURL
        
        //検索済みURL
        private static List<SearchItem> searchedlist=new List<SearchItem>();
        
        public MainFrame()
        {
            InitializeComponent();           
        }
       
        //データ取得開始       
        private void startbutton_Click(object sender, EventArgs e)
        {
            status_txt.Text = "第二段階";
            Refresh();
            initCompanyCount();
            Log.WriteMsg("\n★★★二段階完了★★★\n");

            //第３段階に行く
            singlecompany_Click(null,null);
        }       
        //得到单页
        public string getPageByUrl(string str) {
            WebRequest wReq = WebRequest.Create(str);
            WebResponse wResp = wReq.GetResponse();
            StreamReader reader = new StreamReader(wResp.GetResponseStream());
            return reader.ReadToEnd();
        }      

        private void singlecompany_Click(object sender, EventArgs e)
        {
            status_txt.Text = "第三段階";
            Refresh();
            Log.WriteMsg("\n★★★DB更新开始" + System.DateTime.Now.ToString("F") + "★★★\n");
            CompanyprogressBar.Value = 0;
            CompanyprogressBar.Maximum = searchedlist.Count;
            foreach (SearchItem item in searchedlist)
            {
                if(!item.isvisited){
                    string URL = item.url;
                    int count =item.count;
                    List<string> urllist=GetUrlListByCount(URL,count);
                    if(urllist.Count==0) return;
                    foreach(string pageurl in urllist){
                        InsertCompanyinfoIntoDb(pageurl);
                    }
                    
                }
                item.isvisited=true;
                //进度条加一
                CompanyprogressBar.Value += 1;
            }           
            
            Log.WriteMsg("\n★★★DB更新完了" + System.DateTime.Now.ToString("F") + "★★★\n");
            //·第４段階に行く
            offerbutton_Click(null,null);
        }

        private void dbbutton_Click(object sender, EventArgs e)
        {
            status_txt.Text = "第一段階";
            Refresh();
            getSearchUrlAndCount();
            Log.WriteMsg("\n★★★一段階完了★★★\n");
            startbutton_Click(null,null);    
        }
    
        public void getSearchUrlAndCount() {
            try
            {
                string sql = "select ID,URL,COUNT from m_search_master";
                DBHelper helper = new DBHelper();
                List<List<string>> result = helper.Select(sql);
                //进度条
                CompanyprogressBar.Value = 0;
                CompanyprogressBar.Maximum = result.Count;
                foreach (List<string> item in result)
                {
                    SearchItem sitem = new SearchItem();
                    //URL
                    sitem.url = item[1];
                    //COUNT
                    sitem.count = int.Parse(item[2]);
                    //isvisited
                    sitem.isvisited = false;
                    searchedlist.Add(sitem);

                    CompanyprogressBar.Value += 1;
                }
               
                
            }
            catch (Exception ex)
            {
                Log.WriteMsg("検索条件出错" + ex.Message);
            }
        }
        
        //会社情報を Insertします
        private void InsertCompanyinfoIntoDb(string url) {
            try{
            string firstpage = getPageByUrl(url);
            ArrayList conpanyliststr = Reg.betweenStrGetArray(firstpage, "<div class='ctx cfix mb0'>\n", "求人詳細／開発環境を見る");
            ArrayList companylist = new ArrayList();
            foreach (string con in conpanyliststr)
            {
               
                Company conpany = new Company();
                //会社画像
                conpany.img = Reg.betweenStr(con, "<img src=\"", "\" alt=");
                //会社名
                conpany.name = Reg.betweenStr(con, "corpname\">", "</a>");
                //設立日
                conpany.built_date = Reg.betweenStr(con, "<th>設立</th>\\n<td>", "</td>");
                //社員数
                conpany.staffnum = Reg.betweenStr(con, "<th>社員数</th>\\n<td>", "</td>");
                //平均年齢
                conpany.staffage = Reg.betweenStr(con, "<th>平均年齢</th>\\n<td>", "</td>");
                //offerID
                conpany.offerid = int.Parse(Reg.betweenStr(con, "<li class='lst'>\\n<a href=\"", "\">").Replace("/career/job_offers/",""));
                companylist.Add(conpany);
                foreach (Company conp in companylist)
                {

                    //inser チェック 会社
                    if (insertCheckCompany(conp) == true)
                    {
                        try
                        {
                            string sql = "insert into company(IMG,NAME,BUILT_DATE,STAFF_NUM,STAFF_AGE) values(\""
                            + conp.img + "\",\""
                            + conp.name + "\",\""
                            + conp.built_date + "\",\""
                            + conp.staffnum + "\",\""
                            + conp.staffage + "\")";
                            DBHelper helper = new DBHelper();
                            helper.Insert(sql);

                            insertOffer(conpany.offerid, conpany.name);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMsg("ERROR!会社名" + conpany.name + "\t" + ex.Message);
                        }
                    }
                    else
                    {
                        //数据库取出重名公司，进行比较
                        try
                        {
                            insertOffer(conpany.offerid, conpany.name);

                        }
                        catch (Exception ex)
                        {
                            Log.WriteMsg("ERROR!可能是更新时数组越界" + ex.Message);
                        }
                    }                
                }
            }
            }
            catch (Exception ex)
            {
                Log.WriteMsg(url+"分析不能" + ex.Message);
            }
        }
        //fisrt insert offer
        public void insertOffer(int offerid,string companyname) {

            //check offer
            if (insertCheckOffer(offerid))
            {

                int companyid = getCompanyIDByName(companyname);
                if (companyid == -1)
                {
                    Log.WriteMsg("DB data 問題あり！OFFER ID" + offerid + " 会社名" + companyname);
                    return;
                }
                string sql_insertoffer = "insert into offer(ID,COMPANY_ID) values("
                       + offerid + ","
                       + companyid
                       + ")";
                try
                {
                    DBHelper helper = new DBHelper();
                    helper.Insert(sql_insertoffer);
                }
                catch (Exception ex)
                {
                    Log.WriteMsg("SQL ERROR！" + sql_insertoffer + "★★★" + ex.Message + "\n");
                }

            }
        }
        //get company id by companyname
        private int getCompanyIDByName(string name) {
            try
            {
                string sql = "select id from company where name='" + name + "'";
                DBHelper helper = new DBHelper();
                List<List<string>> result = helper.Select(sql);
                if (result.Count != 0 && result[0].Count != 0)
                {
                    return int.Parse(result[0][0]);
                }
                else
                {
                    return -1;
                }
            }catch(Exception e){
                Log.WriteMsg("ERROR!getCompanyIDByName"+e.Message);
                return -1;
            }
        }
        //insert check
        private bool insertCheckCompany(Company con) {
            bool flag = true;
            if(con.name.Length==0)return false;
            string sql="select id from company where name ='"+con.name+"'";
            DBHelper helper=new DBHelper();
            if(helper.Select(sql).Count!=0){
                flag = false;
            }
            return flag;
        }
        //insert check offer
        private bool insertCheckOffer(int id)
        {
            bool flag = true;

            string sql = "select id from offer where id ='" + id+ "'";
            DBHelper helper = new DBHelper();
            if (helper.Select(sql).Count != 0)
            {
                flag = false;
            }
            return flag;
        }

        //URLとcountによるURLLIstを作成
        private List<string> GetUrlListByCount(string URL,int count) {
            List<string> list = new List<string>();
            if(count<11&&count>0){
                list.Add(URL);
            }
            if(count>10){
                int pagenum = (int)Math.Ceiling((double)(count/10));
                list.Add(URL);
                for (int i = 1; i < pagenum+1;i++ )
                {
                    string nextpageurl = URL + "?page=" + (i + 1);
                    list.Add(nextpageurl);
                }
            }
            return list;
        }
        //初期化COUNT
        private void initCompanyCount() {
            try
            {
                CompanyprogressBar.Value=0;
                CompanyprogressBar.Maximum = searchedlist.Count;
                foreach(SearchItem item in searchedlist){

                    string firstpage = getPageByUrl(item.url);
                    //総件数
                    string count_str = Reg.betweenStr(firstpage, "<span class='total_count'>\n", "件\\n");
                    int all_count = int.Parse(count_str);
                    //将URL/件数存入数据库
                    if(all_count!=item.count){
                        item.count = all_count;
                        Log.WriteMsg("\n importment! " + item.url +"件数変わった、"+item.count+" => "+all_count+ "\n");
                        //更新数据库
                        DBHelper helper = new DBHelper();
                        string sql = "update M_SEARCH_MASTER set COUNT=" + all_count + " where URL='" + item.url + "'";
                        helper.Update(sql);
                    }
                    CompanyprogressBar.Value += 1;
                }
                

            }
            catch (Exception ex)
            {
                Log.WriteMsg("总件数出错" + ex.Message);
            }
        }
        //更新变化字段
        private void recordChangedColumns(string clouns,string bef_value,string aft_value,int ID) {
            try
            {
                string sql = "insert into T_change(COMPANY_ID,CHANGED_COLUMN,BEFORE_VALUE,AFTER_VALUE,DATE_NOW)values(" + ID + ",'" + clouns + "','" + bef_value + "','" + aft_value + "',NOW())";
                DBHelper helper = new DBHelper();
                helper.Insert(sql);
            }catch(Exception e){
                Log.WriteMsg("★★★记录变化有误，公司ID:" + ID +e.Message+ "★★★\n");
            }
        }

        //OFFERテーブルを更新する
        private void offerbutton_Click(object sender, EventArgs e)
        {            
            string sql = "select id from offer";
            DBHelper helper = new DBHelper();
            List<List<string>> result = helper.Select(sql);

            if (result.Count!=0)
            {
                //进度条
                CompanyprogressBar.Value = 0;
                CompanyprogressBar.Maximum = result.Count;
                foreach (List<string> id in result)
                {
                    try
                    {
                        string url = "https://paiza.jp/career/job_offers/" + id[0] + "/";
                        string offerinfo = Reg.betweenStr(getPageByUrl(url), "<td class='cell1'>通過ランク</td>", "<div class='boxDetail6'>").ToString();

                        Offer offer = new Offer();
                        //勤務地
                        offer.ADDRESS = Reg.betweenStr(offerinfo, "<dt>勤務地</dt>\n<dd>", "</dd>");
                        offer.ADDRESS = Reg.replace(offer.ADDRESS);
                        //必須要件    
                        offer.NECESSARY = Reg.betweenStr(offerinfo, "<dt class='icon3'>必須要件</dt>", "</dd>");
                        offer.NECESSARY = Reg.replace(offer.NECESSARY);
                        //給与体系・詳細
                        offer.SALARY_INFO = Reg.betweenStr(offerinfo, "<span>給与体系・詳細</span>\n</th>\n<td class='cell1'><p>", "</p></td>");
                        offer.SALARY_INFO = Reg.replace(offer.SALARY_INFO);
                        //職務内容
                        offer.JOB_CONTENT = Reg.betweenStr(offerinfo, "<span>職務内容</span>", "</td>");
                        offer.JOB_CONTENT = Reg.replace(offer.JOB_CONTENT);
                        //ポジション
                        offer.POSITION = Reg.betweenStr(offerinfo, "<th>ポジション</th>", "</td>");
                        offer.POSITION = Reg.replace(offer.POSITION);
                        //募集人数
                        offer.RECRUIT_PERSONNUM = Reg.betweenStr(offerinfo, "<span>募集人数</span>", "<th>");
                        offer.RECRUIT_PERSONNUM = Reg.replace(offer.RECRUIT_PERSONNUM);
                        //配属部署人数
                        offer.DEPARTMENT_PERSONNUM = Reg.betweenStr(offerinfo, "<span>配属部署人数</span>", "</tr>");
                        offer.DEPARTMENT_PERSONNUM = Reg.replace(offer.DEPARTMENT_PERSONNUM);
                        //想定年収
                        offer.YEAR_SALARY = Reg.betweenStr(offerinfo, "<span>想定年収</span>", "</tr>");
                        offer.YEAR_SALARY = Reg.replace(offer.YEAR_SALARY);
                        //休日休暇
                        offer.RESTDAY = Reg.betweenStr(offerinfo, "<span>休日休暇</span>", "</tr>");
                        offer.RESTDAY = Reg.replace(offer.RESTDAY);
                        //勤務時間
                        offer.JOB_TIME = Reg.betweenStr(offerinfo, "<span>勤務時間</span>", "<th>");
                        offer.JOB_TIME = Reg.replace(offer.JOB_TIME);
                        //諸手当
                        offer.INSURANCE = Reg.betweenStr(offerinfo, "<span>諸手当</span>", "<th>");
                        offer.INSURANCE = Reg.replace(offer.INSURANCE);
                        //インセンティブ
                        offer.INSETIVE = Reg.betweenStr(offerinfo, "<span>インセンティブ</span>", "<th>");
                        offer.INSETIVE = Reg.replace(offer.INSETIVE);
                        //昇給・昇格
                        offer.PROMOTE = Reg.betweenStr(offerinfo, "<span>昇給・昇格</span>", "<th>");
                        offer.PROMOTE = Reg.replace(offer.PROMOTE);
                        //保険
                        offer.INSURANCE = Reg.betweenStr(offerinfo, "<span>保険</span>", "<th>");
                        offer.INSURANCE = Reg.replace(offer.INSURANCE);
                        //選考フロー
                        offer.APPLICATED_FLOW = Reg.betweenStr(offerinfo, "<span>選考フロー</span>", "</tr>");
                        offer.APPLICATED_FLOW = Reg.replace(offer.APPLICATED_FLOW);
                        //試用期間
                        offer.TRY_TIME = Reg.betweenStr(offerinfo, "<span>試用期間</span>", "<th>");
                        offer.TRY_TIME = Reg.replace(offer.TRY_TIME);

                        //更新
                        string sql_updateoffer = "update offer set "
                            + "ADDRESS ='" + offer.ADDRESS + "',"
                            + " NECESSARY='" + offer.NECESSARY + "',"
                            + " SALARY_INFO='" + offer.SALARY_INFO + "',"
                            + " JOB_CONTENT='" + offer.JOB_CONTENT + "',"
                            + " POSITION='" + offer.POSITION + "',"
                            + " recruit_personnum='" + offer.RECRUIT_PERSONNUM + "',"
                            + " Department_personnum='" + offer.DEPARTMENT_PERSONNUM + "',"
                            + " year_salary='" + offer.YEAR_SALARY + "',"
                            + " restday='" + offer.RESTDAY + "',"
                            + " job_time='" + offer.JOB_TIME + "',"
                            + " allowance='" + offer.ALLOWANCE + "',"
                            + " insetive='" + offer.INSETIVE + "',"
                            + " promote='" + offer.PROMOTE + "',"
                            + " insurance='" + offer.INSURANCE + "',"
                            + " applicated_flow='" + offer.APPLICATED_FLOW + "',"
                            + " try_time='" + offer.TRY_TIME + "'"
                            + " where id=" + id[0];
                        helper.Update(sql_updateoffer);
                        //開発環境
                        OfferEnvironment env = new OfferEnvironment();
                        env.setEnvironment((Reg.betweenStr(offerinfo, "<th>環境</th>\n<td>", "</td>")).Split('、'));
                        env.setOfferid(int.Parse(id[0]));
                        env.InsertDb();
                        //framework
                        OfferFramework framework = new OfferFramework();
                        string frameworkstr = Reg.betweenStr(offerinfo, "<th>フレームワーク</th>", "</tr>");

                        frameworkstr = Reg.betweenStr(frameworkstr, "<a href=", "</td>");
                        framework.setFramework(Reg.betweenStrGetArray(frameworkstr, "\">", "</a>"));
                        framework.setOfferid(int.Parse(id[0]));
                        framework.InsertDb();
                        //label
                        OfferLable label = new OfferLable();
                        string labstr = Reg.betweenStr(offerinfo, "<span>特徴</span>", "</tr>");

                        labstr = Reg.replace(Reg.betweenStr(labstr,"<a href=","</td>"));
                        label.setLabel(Reg.betweenStrGetArray(Reg.replace(labstr),">","</a>"));
                        label.setOfferid(int.Parse(id[0]));
                        label.InsertDb();
                        //言語
                        OfferLanguage language = new OfferLanguage();
                        string languagestr = Reg.betweenStr(offerinfo, "<th>言語</th>", "</tr>");

                        languagestr = Reg.betweenStr(languagestr, "<ul class=\"cfix mb0\">", "</td>");
                        language.setLanguage(Reg.betweenStrGetArray(languagestr, "\">", "</a>"));
                        language.setOfferid(int.Parse(id[0]));
                        language.InsertDb();
                    }catch(Exception ex){
                        Log.WriteMsg("ERROR:offer insert 失敗"+ex.Message);
                    }
                    //进度条
                    CompanyprogressBar.Value += 1;
                    status_txt.Text = "第四段階:" + CompanyprogressBar.Value + "/" + CompanyprogressBar.Maximum;
                    Refresh();
                }
            }
            MessageBox.Show("全部更新完了");
        }
        //
    }
}
