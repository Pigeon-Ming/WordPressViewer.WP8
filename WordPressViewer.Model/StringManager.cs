using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WordPressViewer.Model
{
    public class StringManager
    {
        public static string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, "<.*?>", "");
        }

        public static string StringListToString(List<String>StringList,String Separator)
        {
            if (StringList == null)
                return "";
            String str = "";
            for(int i = 0; i < StringList.Count; i++)
            {
                str += StringList[i];
                if(i!=StringList.Count-1)
                    str += Separator;
            }
                return str;
        }

        public static bool CheckIsUrlFormat(string strValue)
        {
            if (String.IsNullOrEmpty(strValue) == false && strValue.Trim() != "")
            {
                Regex re = new Regex(@"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
                if (re.IsMatch(strValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
