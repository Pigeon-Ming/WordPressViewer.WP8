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
    public sealed partial class CommentPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public static Website website { get; set; }
        public static Post post { get; set; }

        public CommentPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            LittleHeader.Text = post.title;
            Loaded += CommentPage_Loaded;
        }

        private void CommentPage_Loaded(object sender, RoutedEventArgs e)
        {
            CommentListControl.BaseListView.ItemTemplate = Resources["BasicCommentItem"] as DataTemplate;
            CommentListControl.LoadMoreButton.Click += (a, b) => { LoadComments(); };
            LoadComments();
        }

        public List<Comment> comments = new List<Comment>();

        private async Task LoadComments()
        {
            if (CommentListControl.BaseListView.Items.Count % 10 != 0)
            {
                CommentListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
                return;
            }
            CommentListControl.loadingControl.StartLoading();
            CommentListControl.LoadMoreButton.IsEnabled = false;
            List<Comment> newComments = await new CommentManager(website).GetCommentsByPost(post.id,comments.Count / 10 + 1);
            Debug.WriteLine(newComments.Count);
            comments.AddRange(newComments);
            Debug.WriteLine(comments.Count);
            CommentListControl.BaseListView.ItemsSource = null;
            CommentListControl.BaseListView.ItemsSource = comments;
            CommentListControl.loadingControl.EndLoading();
            CommentListControl.LoadMoreButton.IsEnabled = true;
            if (CommentListControl.BaseListView.Items.Count % 10 != 0)
                CommentListControl.LoadMoreButton.Visibility = Visibility.Collapsed;
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
