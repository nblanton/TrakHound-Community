﻿#pragma checksum "..\..\..\ConfigurationPage\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7D459F993A6BB6B7CD1C885076358951"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TH_WPF.LoadingAnimation;


namespace TH_InstanceTable.ConfigurationPage {
    
    
    /// <summary>
    /// Page
    /// </summary>
    public partial class Page : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 51 "..\..\..\ConfigurationPage\Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox events_CHK;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\ConfigurationPage\Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox samples_CHK;
        
        #line default
        #line hidden
        
        
        #line 189 "..\..\..\ConfigurationPage\Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox conditions_CHK;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TH_InstanceTable;component/configurationpage/page.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ConfigurationPage\Page.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.events_CHK = ((System.Windows.Controls.CheckBox)(target));
            
            #line 51 "..\..\..\ConfigurationPage\Page.xaml"
            this.events_CHK.Checked += new System.Windows.RoutedEventHandler(this.events_CHK_Checked);
            
            #line default
            #line hidden
            
            #line 51 "..\..\..\ConfigurationPage\Page.xaml"
            this.events_CHK.Unchecked += new System.Windows.RoutedEventHandler(this.events_CHK_Unchecked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 60 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Help_MouseDown);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Help_MouseEnter);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Help_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 3:
            this.samples_CHK = ((System.Windows.Controls.CheckBox)(target));
            
            #line 120 "..\..\..\ConfigurationPage\Page.xaml"
            this.samples_CHK.Checked += new System.Windows.RoutedEventHandler(this.samples_CHK_Checked);
            
            #line default
            #line hidden
            
            #line 120 "..\..\..\ConfigurationPage\Page.xaml"
            this.samples_CHK.Unchecked += new System.Windows.RoutedEventHandler(this.samples_CHK_Unchecked);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 129 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Help_MouseDown);
            
            #line default
            #line hidden
            
            #line 129 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Help_MouseEnter);
            
            #line default
            #line hidden
            
            #line 129 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Help_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 5:
            this.conditions_CHK = ((System.Windows.Controls.CheckBox)(target));
            
            #line 189 "..\..\..\ConfigurationPage\Page.xaml"
            this.conditions_CHK.Checked += new System.Windows.RoutedEventHandler(this.conditions_CHK_Checked);
            
            #line default
            #line hidden
            
            #line 189 "..\..\..\ConfigurationPage\Page.xaml"
            this.conditions_CHK.Unchecked += new System.Windows.RoutedEventHandler(this.conditions_CHK_Unchecked);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 198 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Help_MouseDown);
            
            #line default
            #line hidden
            
            #line 198 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Help_MouseEnter);
            
            #line default
            #line hidden
            
            #line 198 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Help_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 276 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Help_MouseDown);
            
            #line default
            #line hidden
            
            #line 276 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.Help_MouseEnter);
            
            #line default
            #line hidden
            
            #line 276 "..\..\..\ConfigurationPage\Page.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.Help_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
