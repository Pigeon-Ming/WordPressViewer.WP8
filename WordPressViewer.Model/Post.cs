using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WordPressViewer.Model
{
    public class Post
    {
        public int id { get; set; }
        public string link { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public string authorId { get; set; }
        public List<Category> categories { get; set; } = new List<Category>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public class PostManager
    {
        Website website;

        public PostManager(Website website)
        {
            this.website = website;
        }

        public  async Task<List<Post>> GetLatestPostsAsync()
        {

            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/posts?page=1");
            if (String.IsNullOrEmpty(str))
                return null;
            return await GetPostsFromJsonArray(JsonArray.Parse(str));

        }

        public async Task<Post> GetPostByIdAsync(int Id)
        {

            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/posts/"+Id);
            if (String.IsNullOrEmpty(str))
                return null;
            return await GetPostFromJsonObject(JsonObject.Parse(str));

        }

        public  async Task<List<Post>> GetPostsAsync(int page)
        {

            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/posts?page=" + page.ToString());
            Debug.WriteLine(page);
            if (String.IsNullOrEmpty(str))
                return null;
            return await GetPostsFromJsonArray(JsonArray.Parse(str));

        }

        public  async Task<List<Post>> GetPostsByCategory(int CategoryId,int page = 1)
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/posts?categories=" + CategoryId+"&page="+page);
            return await GetPostsFromJsonArray(JsonArray.Parse(str));
        }

        public async Task<List<Post>> GetPostsByTag(int TagId, int page = 1)
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/posts?tags=" + TagId + "&page=" + page);
            return await GetPostsFromJsonArray(JsonArray.Parse(str));
        }

        public  async Task<List<Post>> GetPostsByUser(int UserId, int page = 1)
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/posts?author=" + UserId + "&page=" + page);
            return await GetPostsFromJsonArray(JsonArray.Parse(str));
        }

        public async Task<List<Post>> GetPostsFromJsonArray(JsonArray jsonArray)
        {
            List<Post> posts = new List<Post>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                posts.Add(await GetPostFromJsonObject(jsonArray[i].GetObject()));
            }
            return posts;
        }

        private async Task<Post> GetPostFromJsonObject(JsonObject jsonObject)
        {
            Post post = new Post();
            post.id = Convert.ToInt32(jsonObject.GetNamedNumber("id"));
            post.date = DateTime.Parse(jsonObject.GetNamedString("date"));
            JsonObject obj = jsonObject.GetNamedObject("title");
            post.title = obj.GetNamedString("rendered");
            obj = jsonObject.GetNamedObject("excerpt");
            post.excerpt = obj.GetNamedString("rendered");
            post.content = jsonObject.GetNamedObject("content").GetNamedString("rendered");
            post.date = DateTime.Parse(jsonObject.GetNamedString("date"));
            int authorId = Convert.ToInt32(jsonObject.GetNamedNumber("author"));

            if (String.IsNullOrEmpty(post.author = (await new UserManager(website).GetUserByIdAsync(authorId)).name))
                post.author = authorId.ToString();
                

            JsonArray jsonArray = jsonObject.GetNamedArray("categories");
            new CategoryManager(website).GetCategoriesNameById(JsonHelper.JsonArrayToIntList(jsonArray));
            return post;
        }
    }
}
