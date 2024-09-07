using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WordPressViewer.Model
{
    public class JsonHelper
    {
        public static List<int> JsonArrayToIntList(JsonArray jsonArray)
        {
            List<int> list = new List<int>();
            for(int i = 0; i < jsonArray.Count; i++)
            {
                list.Add(Convert.ToInt32(jsonArray[i].GetNumber()));
            }
            return list;
        }
    }
}
