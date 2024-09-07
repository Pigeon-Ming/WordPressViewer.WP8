using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WordPressViewer.Model
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int count { get; set; }
        public int parentId { get; set; }
        public string parentName { get; set; }
    }

    public class CategoryManager
    {
        Website website { get; set; }

        public CategoryManager(Website website)
        {
            this.website = website;
        }

        public async Task<List<Category>> GetCategoriesAsync(int page)
        {
            string str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/categories?page=" + page.ToString());
            if (String.IsNullOrEmpty(str))
                return null;
            return GetCategoriesFromJsonArray(JsonArray.Parse(str));
        }

        private async Task<Category> GetCategoryById(int Id)
        {
            String str = await HttpClientManager.GetRequest(website.url + "/wp-json/wp/v2/categories/" + Id);
            if (String.IsNullOrEmpty(str))
                return null;
            return GetCategoryFromJsonObject(JsonObject.Parse(str));
        }

        public  List<string> GetCategoriesNameById(List<int> IdList)
        {
            List<string> categories = new List<string>();
            for (int i = 0; i < IdList.Count; i++)
            {

                Category category = website.CacheData.Categories.Find(x => x.id == IdList[i]);
                if (category == null)
                    categories.Add("[未知分类]");
                else
                    categories.Add(category.name);

            }
            return categories;
        }

        public  string GetCategorieNameById(int Id, List<Category> categoriesList)
        {
            Category category = categoriesList.Find(x => x.id == Id);
            if (category == null)
                return "[未知分类]";
            else
                return category.name;
        }

        private  List<Category> GetCategoriesFromJsonArray(JsonArray jsonArray)
        {
            List<Category> Categories = new List<Category>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                Categories.Add(GetCategoryFromJsonObject(jsonArray[i].GetObject()));
            }
            return Categories;
        }

        private  Category GetCategoryFromJsonObject(JsonObject jsonObject)
        {
            Category category = new Category();
            category.id = Convert.ToInt32(jsonObject.GetNamedNumber("id"));
            category.name = jsonObject.GetNamedString("name");
            category.description = jsonObject.GetNamedString("description");
            //category.slug = jsonObject.GetNamedString("slug");
            category.parentId = Convert.ToInt32(jsonObject.GetNamedNumber("parent"));
            category.count = Convert.ToInt32(jsonObject.GetNamedNumber("count"));
            return category;
        }
    }

}
