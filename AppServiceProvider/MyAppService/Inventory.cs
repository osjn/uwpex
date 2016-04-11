using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace MyAppService
{
    public sealed class Inventory : IBackgroundTask
    {
        private BackgroundTaskDeferral backgroundTaskDeferral;
        private AppServiceConnection appServiceconnection;

        private String[] inventoryItems = new string[] { "Robot vacuum", "Chair" };
        private double[] inventoryPrices = new double[] { 129.99, 88.99 };

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            this.backgroundTaskDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnTaskCanceled;
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            appServiceconnection = details.AppServiceConnection;
            appServiceconnection.RequestReceived += OnRequestReceived;
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (this.backgroundTaskDeferral != null)
            {
                // Complete the service deferral. 
                this.backgroundTaskDeferral.Complete();
            }
        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();
            ValueSet message = args.Request.Message;
            ValueSet returnData = new ValueSet();

            string command = message["Command"] as string;
            int? inventoryIndex = message["ID"] as int?;
            if (inventoryIndex.HasValue
                && inventoryIndex.Value >= 0
                && inventoryIndex.Value < inventoryItems.GetLength(0))
            {
                switch (command)
                {

                    case "Price":
                        returnData.Add("Result", inventoryPrices[inventoryIndex.Value]);
                        returnData.Add("Status", "OK");
                        break;

                    case "Item":
                        returnData.Add("Result", inventoryItems[inventoryIndex.Value]);
                        returnData.Add("Status", "OK");
                        break;

                    default:
                        returnData.Add("Status", "Fail: unknown command");
                        break;
                }
            }
            else
            {
                returnData.Add("Status", "Fail: Index out of range");
            }

            await args.Request.SendResponseAsync(returnData);
            messageDeferral.Complete();
        }
    }
}
