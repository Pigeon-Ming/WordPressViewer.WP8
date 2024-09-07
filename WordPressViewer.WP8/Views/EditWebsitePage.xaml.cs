﻿using System;
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
using WordPressViewer.WP8.Common;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace WordPressViewer.WP8.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditWebsitePage : Page
    {
        private readonly NavigationHelper navigationHelper;
        public static Website website { get; set; }

        public EditWebsitePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);


            NameTextBox.Text = website.name;
            URLTextBox.Text = website.url;
            DescriptionTextBox.Text = website.description;
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

        ContentDialog contentDialog = new ContentDialog();

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        public async Task Save()
        {
            contentDialog.Title = "无法保存网站信息";
            contentDialog.PrimaryButtonText = "关闭";
            contentDialog.PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
            if (String.IsNullOrEmpty(URLTextBox.Text))
            {
                contentDialog.Content = "网站URL地址不能为空";
                await contentDialog.ShowAsync();
                return;
            }
            else if (StringManager.CheckIsUrlFormat(URLTextBox.Text) == false)
            {
                contentDialog.Content = "URL地址格式不正确";
                await contentDialog.ShowAsync();
                return;
            }
            else if (String.IsNullOrEmpty(NameTextBox.Text))
            {
                contentDialog.Content = "网站名称不能为空";
                await contentDialog.ShowAsync();
                return;
            }

            website.name = NameTextBox.Text;
            website.url = URLTextBox.Text;
            website.description = DescriptionTextBox.Text;
            await WebsiteManager.SaveWebsiteList();
            if (Frame != null && Frame.CanGoBack)
                Frame.GoBack();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            contentDialog.Hide();
        }
    }
}
