using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Win8.WebApiSample.ClientApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");
        static readonly Uri _address = new Uri(_baseAddress, "/api/hello");
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

            IsWorking = true;
            ResponseMessage = string.Empty;
            
            //Load data 
            try {
                // Call the web API and display the result
                
                //the client
                HttpClient client = new HttpClient();
                
                //your model
                var postData = new HelloModel();
                postData.Name = txtName.Text;

                //serialize as JSON
                var content = new StringContent(JsonConvert.SerializeObject(postData));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Post it!

                HttpResponseMessage response = await client.PostAsync(_address, content);
                //get response
                

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseText = await response.Content.ReadAsStringAsync();
                    ResponseMessage = responseText.Trim('"');
                }
                    
                else {
                    ResponseMessage = response.StatusCode.ToString();
                }
                if (ResponseMessage.Contains("The End")) {
                    ThreadPool.RunAsync(operation => {
                        new System.Threading.ManualResetEvent(false).WaitOne(10000);
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                            () => IsTheEnd = true);
                    });
                }


            }
            catch (Exception ex) {
                ResponseMessage = "An error occurred: " + ex.Message;
            }
            finally {
                IsWorking = false;
            }
        }

        private bool _isWorking;
        public bool IsWorking {
            get { return _isWorking; }
            set { _isWorking = value; OnPropertyChanged("IsWorking"); }
        }

        private string _responseMessage;
        private bool _isTheEnd;

        public string ResponseMessage {
            get { return _responseMessage; }
            set { _responseMessage = value; OnPropertyChanged("ResponseMessage"); }
        }

        public bool IsTheEnd {
            get { return _isTheEnd; }
            set {
                _isTheEnd = value;
                OnPropertyChanged("IsTheEnd");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtName_KeyUp_1(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Enter)
                Button_Click_1(sender, null);
        }

    }
}
