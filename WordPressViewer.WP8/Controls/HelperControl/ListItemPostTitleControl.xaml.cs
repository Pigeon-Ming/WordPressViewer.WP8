using System;
using System.Collections.Generic;
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

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上有介绍

namespace WordPressViewer.WP8.Controls.HelperControl
{
    public sealed partial class ListItemPostTitleControl : UserControl
    {
        public int PostId { get; set; }
        public Website website { get; set; }

        public ListItemPostTitleControl()
        {
            this.InitializeComponent();
            Loaded += ListItemPostTitleControl_Loaded;
        }

        private void ListItemPostTitleControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        async Task GetData()
        {
            MainTextBlock.Text = (await new PostManager(website).GetPostByIdAsync(PostId)).title;
        }
    }
}
