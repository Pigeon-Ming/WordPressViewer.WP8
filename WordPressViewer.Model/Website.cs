using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace WordPressViewer.Model
{
    public class Website
    {
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }

        public CacheData CacheData { get; set; } = new CacheData();
        public ViewCacheData ViewCacheData { get; set; } = new ViewCacheData();
    }

    public class CacheData//加速查找用的缓存
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<User> Users { get; set; } = new List<User>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }

    public class ViewCacheData//显示用的缓存
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<User> Users { get; set; } = new List<User>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class WebsiteManager
    {
        public static List<Website> WebsiteList { get; set; } = new List<Website>();

        public static async Task ReadWebsiteList()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string str = await FileManager.ReadFile(storageFolder, "WebsiteList.json");
            Debug.WriteLine("Json文件：" + str);
            if (String.IsNullOrEmpty(str)) return;
            JsonArray jsonArray = JsonArray.Parse(str);
            WebsiteList.Clear();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                JsonObject jsonObject = jsonArray[i].GetObject();
                Website website = new Website();
                website.name = jsonObject.GetNamedString("name");
                website.url = jsonObject.GetNamedString("url");
                website.description = jsonObject.GetNamedString("description");
                WebsiteList.Add(website);
            }
        }

        public static async Task SaveWebsiteList()
        {
            JsonArray jsonArray = new JsonArray();
            for (int i = 0; i < WebsiteList.Count; i++)
            {
                JsonObject jsonObject = new JsonObject();
                jsonObject.Add("name", JsonValue.CreateStringValue(WebsiteList[i].name));
                jsonObject.Add("url", JsonValue.CreateStringValue(WebsiteList[i].url));
                jsonObject.Add("description", JsonValue.CreateStringValue(WebsiteList[i].description));
                jsonArray.Add(jsonObject);
            }
            Debug.WriteLine(jsonArray.Stringify());
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            await FileManager.WriteFile(storageFolder, "WebsiteList.json", jsonArray.Stringify());
        }
    }
}
