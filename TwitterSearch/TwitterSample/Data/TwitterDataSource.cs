using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitterSample.Enums;
using TwitterSample.Models;
using TwitterSample.Models.DTO;

namespace TwitterSample.Data
{

    namespace TwitterSample.Data
    {
        public sealed class TwitterDataSource
        {
            private static readonly TwitterDataSource _twitterDataSource = new TwitterDataSource();
            private readonly ObservableCollection<TwitterModel> _twitterItems = new ObservableCollection<TwitterModel>();
            public ObservableCollection<TwitterModel> Items { get { return _twitterItems; } }
            public static IEnumerable<TwitterModel> GetStoredTweets() { return _twitterDataSource.Items; }

            public static async Task LoadRemoteDataAsync(string timeline, ResultType resultType) {
                //Prepare the url
                var query = String.Format("http://search.twitter.com/search.json?q=%23{0}&result_type={1}", timeline, resultType.ToString());
                var client = new HttpClient();
                //Make the request
                var httpResponse = await client.GetAsync(new Uri(query));
                //Read the response
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                // Parse the response (JSON)
                PopulateCollection(responseContent);                
            }

            private static void PopulateCollection(string json) {
                var model = JsonConvert.DeserializeObject<TwitterDTO>(json);
                _twitterDataSource.Items.Clear();
                foreach (var entry in model.results) {
                    _twitterDataSource.Items.Add(new TwitterModel(Convert.ToInt64(entry.id),
                                                                  entry.created_at,
                                                                  entry.profile_image_url,
                                                                  entry.text,
                                                                  entry.from_user_name));
                }
            }
        }
    }
}

