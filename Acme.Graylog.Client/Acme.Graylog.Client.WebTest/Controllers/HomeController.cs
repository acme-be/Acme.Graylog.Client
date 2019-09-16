using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acme.Graylog.Client.WebTest.Controllers
{
    using Newtonsoft.Json;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var configurationContent = System.IO.File.ReadAllText("C:\\TMP\\Acme\\graylog-sample-cph-dev.json");
            var configuration = JsonConvert.DeserializeObject<GraylogConfiguration>(configurationContent);

            var client = new GrayLogHttpTlsClient(configuration);

            /*var oldHost = configuration.Host;
            configuration.Host = "42";*/

            client.SendErrorOccured += (sender, error) =>
            {
                Console.WriteLine($"Exception : {error.Exception} when sending message");
                /*configuration.Host = oldHost;
                client.SendData(error.MessageBody);*/
            };

            client.SendSuccessful += (sender, result) => { Console.WriteLine("Success when sending message"); };

            client.Send($"Hello from {typeof(HomeController).Assembly.FullName}", null, null);
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}