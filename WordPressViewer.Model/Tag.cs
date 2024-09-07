using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WordPressViewer.Model
{
    public class Tag
    {
        public int id { get; set; }
        public int count { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }

    public class TagManager
    {
        Website website;
        public TagManager(Website website)
        {
            this.website = website;
        }

        public async Task<Tag> GetTagByIdAsync(int Id)
        {
            Tag tag = website.CacheData.Tags.Find(x => x.id == Id);
            if (tag != null)
                return tag;
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/tags/" + Id);
            JsonObject jsonObject = null;
            JsonObject.TryParse(str, out jsonObject);
            if (jsonObject == null)
                return null;
            return GetTagFromJsonObject(jsonObject);
        }

        public async Task<List<Tag>> GetTagsAsync(int page = 1)
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/tags?page=" + page);
            return GetTagsFromJsonArray(JsonArray.Parse(str));
        }

        public  async Task<List<Tag>> GetTagAsync(string WebURL, int page = 1)
        {
            string str = await HttpClientManager.GetRequest(WebURL + "/wp-json/wp/v2/tags?page=" + page);
            return GetTagsFromJsonArray(JsonArray.Parse(str));
        }

        public  string GetTagNameById(int Id, List<Tag> tagsList)
        {
            Tag tag = tagsList.Find(x => x.id == Id);

            if (tag == null)
                return "[未知分类]";
            else
                return tag.name;
        }

        private  List<Tag> GetTagsFromJsonArray(JsonArray jsonArray)
        {
            List<Tag> Tag = new List<Tag>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                Tag.Add(GetTagFromJsonObject(jsonArray[i].GetObject()));
            }
            return Tag;
        }

        private  Tag GetTagFromJsonObject(JsonObject jsonObject)
        {
            Tag tag = new Tag();
            tag.id = Convert.ToInt32(jsonObject.GetNamedNumber("id"));
            tag.name = jsonObject.GetNamedString("name");
            tag.description = jsonObject.GetNamedString("description");
            tag.count = Convert.ToInt32(jsonObject.GetNamedNumber("count"));
            return tag;
        }
    }
}
