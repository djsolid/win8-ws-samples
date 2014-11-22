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
            public static async Task<IEnumerable<TwitterModel>> LoadTweets(string searchKey, ResultType resultType) {
                //Prepare the url
                string query = string.Format("http://search.twitter.com/search.json?q=%23{0}&result_type={1}", searchKey, resultType.ToString());
                var client = new HttpClient();
                //Make the request
                var httpResponse = await client.GetAsync(new Uri(query));
                //Read the response
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                // Parse the response (JSON)
                return ParseResponse(responseContent);
            }

            private static ObservableCollection<TwitterModel> ParseResponse(string json) {
                
                var model = JsonConvert.DeserializeObject<TwitterDTO>(json);
                
                ObservableCollection<TwitterModel> tweets = new ObservableCollection<TwitterModel>();
                foreach (var entry in model.results) {
                    tweets.Add(new TwitterModel(Convert.ToInt64(entry.id),
                                                                  entry.created_at,
                                                                  entry.profile_image_url,
                                                                  entry.text,
                                                                  entry.from_user_name));
                }
                return tweets;
            }
        }
    }
}

