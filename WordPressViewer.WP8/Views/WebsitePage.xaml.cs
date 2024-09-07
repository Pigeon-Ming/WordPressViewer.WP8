using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WordPressViewer.Model;
using WordPressViewer.WP8.Common;
using WordPressViewer.WP8.Views.ContentPage;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WordPressViewer.WP8.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WebsitePage : Page
    {
        private readonly NavigationHelper navigationHelper;
        public static Website website { get; set; }

        public WebsitePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            //Debug.WriteLine(website.GetHashCode()+"\n"+WebsiteManager.WebsiteList[0].GetHashCode());

            Loaded += WebsitePage_Loaded;
            MainPivot.Title = website.name;
        }

        private void WebsitePage_Loaded(object sender, RoutedEventArgs e)
        {
            PostListControl.BaseListView.ItemTemplate = Resources["BasicPostItem"] as DataTemplate;
            PostListControl.LoadMoreButton.Click += (a, b) => { LoadPosts(); };
            PostListControl.BaseListView.ItemClick += (a, b) => { PostPage.post = b.ClickedItem as Post; PostPage.website = website; Frame.Navigate(typeof(PostPage)); };
            CategoryListControl.BaseListView.ItemTemplate = Resources["BasicCategoryItem"] as DataTemplate;
            CategoryListControl.LoadMoreButton.Click += (a, b) => { LoadCategories(); };
            CategoryListControl.BaseListView.ItemClick += (a, b) => { CategoryPage.category = b.ClickedItem as Category; CategoryPage.website = website; Frame.Navigate(typeof(CategoryPage)); };
            TagListControl.BaseListView.ItemTemplate = Resources["BasicTagItem"] as DataTemplate;
            TagListControl.LoadMoreButton.Click += (a, b) => { LoadTags(); };
            TagListControl.BaseListView.ItemClick += (a, b) => { TagPage.tag = b.ClickedItem as Tag; TagPage.website = website; Frame.Navigate(typeof(TagPage)); };
            UserListControl.BaseListView.ItemTemplate = Resources["BasicUserItem"] as DataTemplate;
            UserListControl.LoadMoreButton.Click += (a, b) => { LoadUsers(); };
            UserListControl.BaseListView.ItemClick += (a, b) => { UserPage.user = b.ClickedItem as User; UserPage.website = website; Frame.Navigate(typeof(UserPage)); };
            CommentListControl.BaseListView.ItemTemplate = Resources["BasicCommentItem"] as DataTemplate;
            CommentListControl.LoadMoreButton.Click += (a, b) => { LoadComments(); };
            
            PostListControl.BaseListView.ItemsSource = website.ViewCacheData.Posts;
            CategoryListControl.BaseListView.ItemsSource = website.ViewCacheData.Categories;
            UserListControl.BaseListView.ItemsSource = website.ViewCacheData.Users;
            TagListControl.BaseListView.ItemsSource = website.ViewCacheData.Tags;
            CommentListControl.BaseListView.ItemsSource = website.ViewCacheData.Comments;

            if (UserListControl.BaseListView.Items.Count != 0) {
                if (PostListControl.BaseListView.Items.Count % 10 != 0)
                {
                    PostListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
                }
                if (CategoryListControl.BaseListView.Items.Count % 10 != 0)
                {
                    CategoryListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
                }
                if (UserListControl.BaseListView.Items.Count % 10 != 0)
                {
                    UserListControl.LoadMoreButton.Visibility = Visibility.Collapsed;                }
                if (TagListControl.BaseListView.Items.Count % 10 != 0)
                {
                    TagListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
                }
                if (CommentListControl.BaseListView.Items.Count % 10 != 0)
                {
                    CommentListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
                    
                }
                return;
            }
            LoadPosts();
            LoadCategories();
            LoadTags();
            LoadUsers();
            LoadComments();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        

        public async void LoadPosts()
        {
            
                
            PostListControl.loadingControl.StartLoading();
            PostListControl.LoadMoreButton.IsEnabled = false;
            List<Post> newPosts = await new PostManager(website).GetPostsAsync(website.ViewCacheData.Posts.Count / 10 + 1);
            Debug.WriteLine(newPosts.Count);
            website.ViewCacheData.Posts.AddRange(newPosts);
            
            PostListControl.BaseListView.ItemsSource = null;
            PostListControl.BaseListView.ItemsSource = website.ViewCacheData.Posts;
            PostListControl.loadingControl.EndLoading();
            PostListControl.LoadMoreButton.IsEnabled = true;
            if (PostListControl.BaseListView.Items.Count % 10 != 0)
                PostListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
        }

        public async void LoadCategories()
        {
            
            CategoryListControl.loadingControl.StartLoading();
            CategoryListControl.LoadMoreButton.IsEnabled = false;
            List<Category> newCategories = await new CategoryManager(website).GetCategoriesAsync(website.ViewCacheData.Categories.Count / 10 + 1);
            Debug.WriteLine(newCategories.Count);
            website.ViewCacheData.Categories.AddRange(newCategories);
            Debug.WriteLine(website.ViewCacheData.Categories.Count);
            CategoryListControl.BaseListView.ItemsSource = null;
            CategoryListControl.BaseListView.ItemsSource = website.ViewCacheData.Categories;
            CategoryListControl.loadingControl.EndLoading();
            CategoryListControl.LoadMoreButton.IsEnabled = true;
            if (CategoryListControl.BaseListView.Items.Count % 10 != 0)
                CategoryListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
        }

        public async void LoadUsers()
        {
            
            UserListControl.loadingControl.StartLoading();
            UserListControl.LoadMoreButton.IsEnabled = false;
            List<User> newUsers = await new UserManager(website).GetUsersAsync(website.ViewCacheData.Users.Count / 10 + 1);
            Debug.WriteLine(newUsers.Count);
            website.ViewCacheData.Users.AddRange(newUsers);
            Debug.WriteLine(website.ViewCacheData.Users.Count);
            UserListControl.BaseListView.ItemsSource = null;
            UserListControl.BaseListView.ItemsSource = website.ViewCacheData.Users;
            UserListControl.loadingControl.EndLoading();
            UserListControl.LoadMoreButton.IsEnabled = true;
            if (UserListControl.BaseListView.Items.Count % 10 != 0)
                UserListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
        }

        public async void LoadTags()
        {
            
            TagListControl.loadingControl.StartLoading();
            TagListControl.LoadMoreButton.IsEnabled = false;
            List<Tag> newTags = await new TagManager(website).GetTagsAsync(website.ViewCacheData.Tags.Count / 10 + 1);
            Debug.WriteLine(newTags.Count);
            website.ViewCacheData.Tags.AddRange(newTags);
            Debug.WriteLine(website.ViewCacheData.Tags.Count);
            TagListControl.BaseListView.ItemsSource = null;
            TagListControl.BaseListView.ItemsSource = website.ViewCacheData.Tags;
            TagListControl.loadingControl.EndLoading();
            TagListControl.LoadMoreButton.IsEnabled = true;
            if (TagListControl.BaseListView.Items.Count % 10 != 0)
                TagListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
        }

        public async Task LoadComments()
        {
            
            CommentListControl.loadingControl.StartLoading();
            CommentListControl.LoadMoreButton.IsEnabled = false;
            List<Comment> newComments = await new CommentManager(website).GetCommentsAsync(website.ViewCacheData.Comments.Count / 10 + 1);
            Debug.WriteLine(newComments.Count);
            website.ViewCacheData.Comments.AddRange(newComments);
            Debug.WriteLine(website.ViewCacheData.Comments.Count);
            CommentListControl.BaseListView.ItemsSource = null;
            CommentListControl.BaseListView.ItemsSource = website.ViewCacheData.Comments;
            CommentListControl.loadingControl.EndLoading();
            CommentListControl.LoadMoreButton.IsEnabled = true;
            if (CommentListControl.BaseListView.Items.Count % 10 != 0)
                CommentListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
        }
    }
}
