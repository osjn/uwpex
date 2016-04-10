using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPAdaptiveCode
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Color? DefaultTitleBarButtonsBGColor;
        private Color? DefaultTitleBarBGColor;

        public MainPage()
        {
            this.InitializeComponent();

            // Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().
            //VisibleBoundsCh anged += MainPage_VisibleBoundsChanged;

            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            DefaultTitleBarBGColor = viewTitleBar.BackgroundColor;
            DefaultTitleBarButtonsBGColor = viewTitleBar.ButtonBackgroundColor;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Bottom AppBar shows on Desktop and Mobile
            if (ShowAppBarRadioButton != null)
            {
                if (ShowAppBarRadioButton.IsChecked.HasValue && (ShowAppBarRadioButton.IsChecked.Value == true))
                {
                    commandBar.Visibility = Visibility.Visible;
                    commandBar.Opacity = 1;
                }
                else
                {
                    commandBar.Visibility = Visibility.Collapsed;
                }
            }

            if (ShowOpaqueAppBarRadioButton != null)
            {
                if (ShowOpaqueAppBarRadioButton.IsChecked.HasValue && (ShowOpaqueAppBarRadioButton.IsChecked.Value == true))
                {
                    commandBar.Visibility = Visibility.Visible;
                    commandBar.Background.Opacity = 0;
                }
                else
                {
                    commandBar.Background.Opacity = 1;
                }
            }
        }

        private void StatusBarBackgroundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // statusbar is mobile only
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = Colors.Blue;
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
            }
        }

        private void StatusBarBackgroundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 0;
            }
        }

        private void StatusBarHiddenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var ignore = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            }
        }

        private void StatusBarHiddenCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var ignore = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ShowAsync();
            }
        }
    }

    public class DeviceFamilyTrigger : StateTriggerBase
    {
        // private variables
        private string _deviceFamily;

        // public property
        public string DeviceFamily
        {
            get
            {
                return _deviceFamily;
            }

            set
            {
                _deviceFamily = value;
                var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;

                if (qualifiers.ContainsKey("DeviceFamily"))
                {
                    SetActive(qualifiers["DeviceFamily"] == _deviceFamily);
                }
                else
                {
                    SetActive(false);
                }
            }
        }
    }
}

