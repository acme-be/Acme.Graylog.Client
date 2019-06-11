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
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// Sample class to test
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <returns>The task to wait</returns>
        private static async Task Main()
        {
            var configurationContent = File.ReadAllText("C:\\TMP\\Acme\\graylog-sample.json");
            var configuration = JsonConvert.DeserializeObject<GraylogConfiguration>(configurationContent);

            var client = new GrayLogHttpTlsClient(configuration);
            await client.SendAsync($"Hello from {typeof(Program).Assembly.FullName}");
        }
    }
}