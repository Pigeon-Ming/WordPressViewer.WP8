using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WordPressViewer.WP8.Views.ContentPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CategoryPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public static Website website { get; set; }
        public static Category category { get; set; }

        public CategoryPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            Loaded += UserPage_Loaded;
        }

        private void UserPage_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTextblock.Text = category.name;
            PostListControl.BaseListView.ItemTemplate = Resources["BasicPostItem"] as DataTemplate;
            PostListControl.LoadMoreButton.Click += (a, b) => { LoadPosts(); };
            PostListControl.BaseListView.ItemClick += (a, b) => { PostPage.post = b.ClickedItem as Post; PostPage.website = website; Frame.Navigate(typeof(PostPage)); };
            PostListControl.BaseListView.ItemsSource = posts;
            LoadPosts();
        }

        List<Post> posts = new List<Post>();

        async Task LoadPosts()
        {
            if (PostListControl.BaseListView.Items.Count % 10 != 0)
            {
                PostListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
                return;
            }

            PostListControl.loadingControl.StartLoading();
            PostListControl.LoadMoreButton.IsEnabled = false;
            List<Post> newPosts = await new PostManager(website).GetPostsByCategory(category.id,posts.Count / 10 + 1);
            posts.AddRange(newPosts);
            PostListControl.BaseListView.ItemsSource = null;
            PostListControl.BaseListView.ItemsSource = posts;
            PostListControl.loadingControl.EndLoading();
            PostListControl.LoadMoreButton.IsEnabled = true;
            if (PostListControl.BaseListView.Items.Count % 10 != 0)
                PostListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
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
    }
}
