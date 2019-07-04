// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client.Tests
{
    using System;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Sample class to test
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            var configurationContent = File.ReadAllText("C:\\TMP\\Acme\\graylog-sample.json");
            var configuration = JsonConvert.DeserializeObject<GraylogConfiguration>(configurationContent);

            var client = new GrayLogHttpTlsClient(configuration);

            var oldHost = configuration.Host;
            configuration.Host = "42";

            client.SendErrorOccured += (sender, error) =>
                {
                    Console.WriteLine($"Exception : {error.Exception} when sending message");
                    configuration.Host = oldHost;
                    client.SendData(error.MessageBody);
                };

            client.SendSuccessful += (sender, result) => { Console.WriteLine("Success when sending message"); };

            client.Send($"Hello from {typeof(Program).Assembly.FullName}", null, null);

            var dummy = new Dummy { FirstName = "Simon", LastName = "Baudart" };
            var gelf = client.CreateGelfObject("This is a sample object", null, dummy);
            client.Send($"Hello from {typeof(Program).Assembly.FullName}", null, dummy);
            Console.WriteLine(gelf);

            var dummyJobject = new JObject();
            dummyJobject["firstName"] = "Simon";
            dummyJobject["lastName"] = "Baudart";
            dummyJobject["anwser"] = 42;

            client.Send($"Hello from {typeof(Program).Assembly.FullName}", null, dummyJobject);
        }

        /// <summary>
        /// Represent a dummy data
        /// </summary>
        private class Dummy
        {
            /// <summary>
            /// Gets or sets the first name.
            /// </summary>
            /// <value>
            /// The first name.
            /// </value>
            public string FirstName { get; set; }

            /// <summary>
            /// Gets or sets the last name.
            /// </summary>
            /// <value>
            /// The last name.
            /// </value>
            public string LastName { get; set; }
        }
    }
}