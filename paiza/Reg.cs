using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace paiza
{
    /*
     * 正则表达式类
     */
    class Reg
    {
        private string pattern;

        public static string betweenStr(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
        public Reg(string str) {
            this.pattern = str;
        }
        public static ArrayList betweenStrGetArray(string str, string s, string e)
        {
            MatchCollection mc;
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            mc = rg.Matches(str);
            ArrayList result = new ArrayList();
            for (int i = 0; i < mc.Count; i++)
            {

                result.Add( mc[i].ToString() );

            }
            return result;
        }
        public static string replace(string str) {
            return Regex.Replace(str, @"(\042|'|\t|\n|<td class='cell1'>|<dd>|<p>|<br />|<p>|</p>|</th>|</dl>|<td colspan='3'>|<dt class='icon1'>|<dl class='txt2 mb0'>|<dt class='icon2'>|</dt>|<dt>|<td>|</td>|<tr>|</tr>|<li>|</li>|<li class=feature_tag font12 strong>)", "", RegexOptions.IgnoreCase);
        }
    }
}
