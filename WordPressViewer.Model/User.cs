using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WordPressViewer.Model
{
    public class User
    {
        public int id { get; set; } = -1;
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }

    public class UserManager
    {
        Website website;
        public UserManager(Website website)
        {
            this.website = website;
        }

        public async Task<User> GetUserByIdAsync(int Id)
        {
            User user = website.CacheData.Users.Find(x => x.id == Id);
            Debug.WriteLine(user == null);
            if (user != null)
                return user;
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/users/"+Id);
            JsonObject jsonObject = null;
            JsonObject.TryParse(str,out jsonObject);
            if (jsonObject == null)
                return null;
            return GetUserFromJsonObject(jsonObject);
        }

        public  async Task<List<User>> GetAllUsersAsync()
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/users");
            return GetUsersFromJsonArray(JsonArray.Parse(str));
        }

        public  async Task<List<User>> GetUsersAsync(int page = 1)
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/users?page=" + page);
            return GetUsersFromJsonArray(JsonArray.Parse(str));
        }

        private  List<User> GetUsersFromJsonArray(JsonArray jsonArray)
        {
            List<User> users = new List<User>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                users.Add(GetUserFromJsonObject(jsonArray[i].GetObject()));
            }
            return users;
        }

        private  User GetUserFromJsonObject(JsonObject jsonObject)
        {
            User user = new User();
            user.id = Convert.ToInt32((jsonObject.GetNamedNumber("id")));
            user.name = jsonObject.GetNamedString("name");
            user.description = jsonObject.GetNamedString("description");
            user.url = jsonObject.GetNamedString("url");


            //将得到的内容写入缓存
            website.CacheData.Users = website.CacheData.Users.Union(new List<User>(){ user }).ToList<User>();


            return user;
        }
    }
}
