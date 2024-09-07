using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
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

namespace WordPressViewer.WP8.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public AboutPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);

            InitData();
        }

        void InitData()
        {
            VersionTextBlock.Text = "当前版本："+string.Format("{0}.{1}.{2}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build);
        }

        async Task CheckUpdate()
        {
            int latestVersion = Convert.ToInt32(await HttpClientManager.GetRequest("https://pigeon-ming.github.io/Versions/WordPressViewerWP8.txt"));
            if (latestVersion > Package.Current.Id.Version.Build)
                UpdateTextBlock.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>

    }
}
