using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPLifeCycleDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public MainPage()
        {
            this.InitializeComponent();
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);
        }

        async void App_Suspending(Object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // create a simple setting
            localSettings.Values["FirstName"] = fName.Text;
            localSettings.Values["LastName"] = lName.Text;
            localSettings.Values["Email"] = email.Text;
        }

        private void App_Resuming(Object sender, Object e)
        {
            fName.Text = localSettings.Values["FirstName"].ToString();
            lName.Text = localSettings.Values["LastName"].ToString();
            email.Text = localSettings.Values["Email"].ToString();
        }
    }

    public abstract class BindableBase : INotifyPropertyChanged
    {
        private string _FirstName = default(string);

        public string FirstName
        {
            get { return _FirstName; }
            set { Set(ref _FirstName, value); }
        }

        private string _LastName = default(string);

        public string LastName
        {
            get { return _LastName; }
            set { Set(ref _LastName, value); }
        }

        private string _Email = default(string);

        public string Email
        {
            get { return _Email; }
            set { Set(ref _Email, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Set<T>(ref T storage, T value, [CallerMemberName()]string propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
