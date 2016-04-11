using System;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClientApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AppServiceConnection inventoryService;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            // add the connection
            if (this.inventoryService == null)
            {
                this.inventoryService = new AppServiceConnection();
                this.inventoryService.AppServiceName = "com.microsoft.inventory";
                this.inventoryService.PackageFamilyName = "bb1a8478-8005-46869923-e525ceaa26fc_4sz2ag3dcq60a";

                var status = await this.inventoryService.OpenAsync();

                if (status != AppServiceConnectionStatus.Success)
                {
                    button.Content = "Failed to connect";
                    return;
                }
            }

            // Call the service. 
            int idx = int.Parse(textBox.Text);
            var message = new ValueSet();

            message.Add("Command", "Item");
            message.Add("ID", idx);

            AppServiceResponse response = await
               this.inventoryService.SendMessageAsync(message);
            string result = "";

            if (response.Status == AppServiceResponseStatus.Success)
            {
                // Get the data  that the service sent  to us. 
                if (response.Message["Status"] as string == "OK")
                {
                    result = response.Message["Result"] as string;
                }
            }

            message.Clear();
            message.Add("Command", "Price");
            message.Add("ID", idx);

            response = await this.inventoryService.SendMessageAsync(message);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                // Get the data that the service sent to us. 
                if (response.Message["Status"] as string == "OK")
                {
                    result += " : Price = " + "$" + response.Message["Result"] as string;
                }
            }

            textBlock.Text = result;
        }
    }
}
