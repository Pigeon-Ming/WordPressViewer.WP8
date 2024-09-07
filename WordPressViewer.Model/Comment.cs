using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WordPressViewer.Model
{
    public class Comment
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string authorName { get; set; }
        public int authorId { get; set; }
        public int parentId { get; set; }
        public int postId { get; set; }
        public string content { get; set; }
    }

    public class CommentManager
    {
        Website website;
        public CommentManager(Website website)
        {
            this.website = website;
        }

        public  async Task<List<Comment>> GetCommentsByPost(int PostId, int Page = 1)
        {
            //Debug.WriteLine(website.url + "/wp-json/wp/v2/comments?post=" + PostId);
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/comments?post=" + PostId+"&page="+Page);
            return GetCommentsFromJsonArray(JsonArray.Parse(str));
        }

        public async Task<List<Comment>> GetCommentsAsync(int Page = 1)
        {
            //Debug.WriteLine(website.url + "/wp-json/wp/v2/comments?post=" + PostId);
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/comments?page=" + Page);
            Debug.WriteLine(str);
            JsonArray jsonArray = null;
            if (JsonArray.TryParse(str, out jsonArray))
                return GetCommentsFromJsonArray(jsonArray);
            else
                return new List<Comment>();
        }

        private  List<Comment> GetCommentsFromJsonArray(JsonArray jsonArray)
        {
            List<Comment> comments = new List<Comment>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                Debug.WriteLine(jsonArray.ToString());
                comments.Add(GetCommentFromJsonObject(jsonArray[i].GetObject()));
            }
            return comments;
        }

        private  Comment GetCommentFromJsonObject(JsonObject jsonObject)
        {
            Comment comment = new Comment();
            comment.date = DateTime.Parse(jsonObject.GetNamedString("date"));
            comment.id = Convert.ToInt32(jsonObject.GetNamedNumber("id"));
            comment.authorName = jsonObject.GetNamedString("author_name");
            comment.authorId = Convert.ToInt32(jsonObject.GetNamedNumber("author"));
            comment.parentId = Convert.ToInt32(jsonObject.GetNamedNumber("parent"));
            if (jsonObject.GetNamedValue("content").ValueType == JsonValueType.Object)
                comment.content = StringManager.RemoveHtmlTags(jsonObject.GetNamedObject("content").GetNamedString("rendered"));
            else if(jsonObject.GetNamedValue("content").ValueType == JsonValueType.String)
                comment.content = StringManager.RemoveHtmlTags(jsonObject.GetNamedString("content"));
            if (jsonObject.GetNamedValue("post").ValueType == JsonValueType.Number)
            {
                comment.postId = Convert.ToInt32(jsonObject.GetNamedNumber("post"));
            }
            return comment;
        }
    }
}
