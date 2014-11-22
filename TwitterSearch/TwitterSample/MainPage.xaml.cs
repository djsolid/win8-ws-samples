using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TwitterSample.Data.TwitterSample.Data;
using TwitterSample.Enums;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TwitterSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private bool _isSearching;
        private string _errorMessage;

        public MainPage() {
            this.InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e) {
            IsSearching = true;
            ErrorMessage = string.Empty;
            //Get the search term
            string searchTerm = txtSearchKey.Text;
            //Reset the listview
            itemListView.ItemsSource = null;
            //Load data 
            try {
                itemListView.ItemsSource = await TwitterDataSource.LoadTweets(searchTerm, ResultType.Recent);
            }
            catch (Exception ex) {
                ErrorMessage = "An error occurred: " + ex.Message;
            }
            finally {
                IsSearching = false;
            }

        }

        public string ErrorMessage {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        public bool IsSearching {
            get { return _isSearching; }
            set { _isSearching = value; OnPropertyChanged("IsSearching"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtSearchKey_KeyUp_1(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Enter)
                Button_Click_1(sender, null);
        }
    }
}
