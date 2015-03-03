using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;
using Microsoft.Phone.Tasks;

namespace TommyJams.View
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            Version.Text = GetVersion();
        }
        public static string GetVersion()
        {
            var versionAttribute = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).FirstOrDefault() as AssemblyFileVersionAttribute;

            if (versionAttribute != null)
            {
                return versionAttribute.Version;
            }
            return "";
        }

        private void privacy_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask()
            {
                URL = "http://tommyjams.com/mobile-privacy-policy-tommyjams.pdf"
            };
            wbt.Show();
        }

        private void review_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask oRateTask = new MarketplaceReviewTask();
            oRateTask.Show();
        }

        private void website_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask()
            {
                URL = "http://www.tommyjams.com"
            };
            wbt.Show();
        }

        private void Tech_Support_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "TommyJams Mobile App: Help!";
            emailComposeTask.To = "contact@tommyjams.com";

            emailComposeTask.Show();
        }
    }
}