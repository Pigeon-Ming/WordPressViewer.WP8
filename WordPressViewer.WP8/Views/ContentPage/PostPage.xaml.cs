using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class PostPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public static Post post { get; set; }
        public static Website website { get; set; }

        public PostPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);

            Loaded += PostPage_Loaded;
        }

        private void PostPage_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTextBlock.Text = post.title;
            MessagesTextBlock.Text = "作者：" + post.author + "\n发布时间：" + post.date.ToString();
            UseOwnControls();
            LittleHeader.Text = post.title;
            LittleHeader.Opacity = 0;
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

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (RootScrollViewer.VerticalOffset < TitleTextBlock.ActualHeight)
                LittleHeader.Opacity = 0;
            else
                LittleHeader.Opacity = RootScrollViewer.VerticalOffset / (Double)TitleTextBlock.ActualHeight;
        }

        private void BottomCommandBar_ViewComments_Click(object sender, RoutedEventArgs e)
        {
            CommentPage.post = post;
            CommentPage.website = website;
            Frame.Navigate(typeof(CommentPage));

        }

        private void BottomCommandBar_OpenInBrowser_Click(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new System.Uri(website.url + "/?p=" + post.id));
        }

        private void BottomCommandBar_WebView_Click(object sender, RoutedEventArgs e)
        {
            if (BottomCommandBar_WebView.IsChecked == true)
            {
                BottomCommandBar_WebView.Label = "使用内置解析器查看";
                UseWebView();
                webView.Visibility = Visibility.Visible;
                ContentGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                BottomCommandBar_WebView.Label = "使用WebView查看";
                webView.Visibility = Visibility.Collapsed;
                ContentGrid.Visibility = Visibility.Visible;
            }
        }

        private void UseOwnControls()
        {
            TextBlock ContentTextBlock = new TextBlock();
            ContentTextBlock.Text = StringManager.RemoveHtmlTags(post.content);
            ContentTextBlock.Style = Resources["BodyTextBlockStyle"] as Style;
            ContentTextBlock.Margin = new Thickness(0, 0, 0, 19.5);
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(ContentTextBlock);
        }

        private void UseWebView()
        {
            //await FileManager.WriteFile(ApplicationData.Current.LocalFolder, "ReadingPost.html", post.content);
            //webView.Navigate();

            //webView.Navigate(new Uri("http://pigeonming.top/"));
            //webView.Source = new Uri("http://pigeonming.top/");
            webView.NavigateToString(post.content);
            
            Debug.WriteLine(post.content);
        }
    }
}
