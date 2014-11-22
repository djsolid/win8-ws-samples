using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Win8.WebApiSample.ServerApp
{
    public class HelloController : ApiController
    {
        public string Post(HelloModel model) {
            try {
                if (model.Name == "The End")
                    return string.Format("{0}! That's all folks!", model.Name);
                return string.Format("Hello, {0}!!", model.Name);
            }
            catch (Exception e) {
                return e.Message;
            }
        }
    }

    class Program
    {
        static readonly Uri BaseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args) {
            HttpSelfHostServer server = null;
            try {
                // Set up server configuration 
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(BaseAddress);
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                // Create server 
                server = new HttpSelfHostServer(config);
                // Start listening 
                server.OpenAsync().Wait();
                Console.WriteLine("Listening on " + BaseAddress);
                Console.ReadLine();
            }
            catch (Exception e) {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally {
                if (server != null) {
                    // Stop listening 
                    server.CloseAsync().Wait();
                }
            }
        }
    }
}
//if (model.Name == "The End")
//    return string.Format("{0}! That's all folks!", model.Name);
