﻿// -----------------------------------------------------------------------
//  <copyright file="GrayLogHttpTlsClient.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client
{
    using System;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    using Acme.Core.Extensions;

    using Newtonsoft.Json;

    /// <summary>
    /// Graylog client with https
    /// </summary>
    public class GrayLogHttpTlsClient : GrayLogBaseClient
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly GraylogConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayLogHttpTlsClient" /> class.
        /// </summary>
        /// <param name="facility">Facility to set on all sent messages.</param>
        /// <param name="host">GrayLog host name.</param>
        /// <param name="port">GrayLog HTTP port.</param>
        /// <param name="useSsl">Whether to use SSL (not supported by GrayLog at this time).</param>
        /// <param name="useCompression">if set to <c>true</c> [use compression].</param>
        /// <param name="clientCertificatePath">The client certificate path.</param>
        /// <param name="clientCertificatePassword">The client certificate password.</param>
        public GrayLogHttpTlsClient(string facility, string host, int port = 12201, bool useSsl = false, bool useCompression = true, string clientCertificatePath = null, string clientCertificatePassword = null)
            : base(facility)
        {
            this.configuration = new GraylogConfiguration
                                     {
                                         Facility = facility,
                                         Host = host,
                                         Port = port,
                                         UseSsl = useSsl,
                                         UseCompression = useCompression,
                                         ClientCertificatePath = clientCertificatePath,
                                         ClientCertificatePassword = clientCertificatePassword,
                                         RequestTimeout = 120
                                     };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayLogHttpTlsClient" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public GrayLogHttpTlsClient(GraylogConfiguration configuration)
            : base(configuration.Facility)
        {
            configuration.ThrowIfNull(nameof(configuration));
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public override async Task SendAsync(string shortMessage, string fullMessage = null, object data = null)
        {
            shortMessage.ThrowIfNull(nameof(shortMessage));
            this.Facility.ThrowIfNull(nameof(this.Facility));

            var log = this.CreateObject(shortMessage, fullMessage, data);

            var serializedLog = JsonConvert.SerializeObject(log);
            var messageBody = Encoding.UTF8.GetBytes(serializedLog);

            await this.SendData(messageBody);
        }

        /// <summary>
        /// Gets the gelf URI.
        /// </summary>
        /// <returns>The URI of the GELF path</returns>
        private Uri GetGelfUri()
        {
            return new Uri($"{(this.configuration.UseSsl ? "https://" : "http://")}{this.configuration.Host}:{this.configuration.Port}/gelf");
        }

        /// <summary>
        /// Sends the data.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <returns>The task to wait</returns>
        private async Task SendData(byte[] messageBody)
        {
            var gelfUri = this.GetGelfUri();

            if (this.configuration.UseCompression)
            {
                messageBody = this.Compress(messageBody, CompressionLevel.Optimal);
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(gelfUri);
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.ContentLength = messageBody.Length;
            httpWebRequest.Expect = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.configuration.ClientCertificatePath))
            {
                var certificates = new X509Certificate2Collection();
                certificates.Import(this.configuration.ClientCertificatePath, this.configuration.ClientCertificatePassword, X509KeyStorageFlags.Exportable);

                httpWebRequest.ClientCertificates = certificates;
            }

            if (this.configuration.RequestTimeout > 0)
            {
                httpWebRequest.ReadWriteTimeout = this.configuration.RequestTimeout;
                httpWebRequest.Timeout = this.configuration.RequestTimeout;
            }

            if (this.configuration.UseCompression)
            {
                httpWebRequest.Headers.Add(HttpRequestHeader.ContentEncoding, "gzip");
            }

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                await requestStream.WriteAsync(messageBody, 0, messageBody.Length);
            }

            await httpWebRequest.GetResponseAsync();
        }
    }
}