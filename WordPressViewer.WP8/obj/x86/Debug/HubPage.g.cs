﻿

#pragma checksum "D:\WPAPPS\WordPressViewer.WP8\WordPressViewer.WP8\WordPressViewer.WP8\HubPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C8CC6D3E499A6BC7C4D6CE7523952739"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordPressViewer.WP8
{
    partial class HubPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 61 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.WebsiteListViewMenu_Edit_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 62 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.WebsiteListViewMenu_Delete_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 70 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.BottomCommandBar_About_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 76 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Hub)(target)).SectionsInViewChanged += this.Hub_SectionsInViewChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 80 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.WebsiteListView_Loaded;
                 #line default
                 #line hidden
                #line 82 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.WebsiteListView_ItemClick;
                 #line default
                 #line hidden
                #line 83 "..\..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.WebsiteListView_Holding;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


