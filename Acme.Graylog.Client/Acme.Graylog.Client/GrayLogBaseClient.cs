// -----------------------------------------------------------------------
//  <copyright file="GrayLogBaseClient.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;

    using Acme.Core.Extensions;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Base client for all the protocols
    /// </summary>
    public abstract class GrayLogBaseClient
    {
        /// <summary>
        /// The facility
        /// </summary>
        protected readonly string Facility;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayLogBaseClient" /> class.
        /// </summary>
        /// <param name="facility">The facility.</param>
        protected GrayLogBaseClient(string facility)
        {
            this.Facility = facility;
        }

        /// <summary>
        /// Occurs when an exception is thrown when sending a message.
        /// </summary>
        public event EventHandler<GraylogSendError> SendErrorOccured;

        /// <summary>
        /// Occurs when the sending of a message result in success operation.
        /// </summary>
        public event EventHandler<GraylogSendResult> SendSuccessful;

        /// <summary>
        /// Creates the object in GELF format.
        /// </summary>
        /// <param name="shortMessage">The short message.</param>
        /// <param name="fullMessage">The full message.</param>
        /// <param name="data">The data.</param>
        /// <returns>The JOBject in GLEF format</returns>
        public JObject CreateGelfObject(string shortMessage, string fullMessage, object data)
        {
            var log = new JObject();
            log["version"] = "1.1";
            log["host"] = Environment.MachineName;

            log["_facility"] = this.Facility;

            log["timestamp"] = ToEpoch(DateTime.UtcNow);
            log["short_message"] = shortMessage;

            if (!string.IsNullOrWhiteSpace(fullMessage))
            {
                log["full_message"] = fullMessage;
            }

            if (data is string dataString)
            {
                log["_data"] = dataString;
            }
            else
            {
                this.DumpObjectInAdditionnalData(log, data);
            }

            return log;
        }

        /// <summary>
        /// Creates the object in GELF format.
        /// </summary>
        /// <param name="shortMessage">The short message.</param>
        /// <param name="fullMessage">The full message.</param>
        /// <param name="data">The data.</param>
        /// <returns>The JOBject in GLEF format</returns>
        public JObject CreateGelfObject(string shortMessage, string fullMessage, JObject data)
        {
            var log = new JObject();
            log["version"] = "1.1";
            log["host"] = Environment.MachineName;

            log["_facility"] = this.Facility;

            log["timestamp"] = ToEpoch(DateTime.UtcNow);
            log["short_message"] = shortMessage;

            if (!string.IsNullOrWhiteSpace(fullMessage))
            {
                log["full_message"] = fullMessage;
            }

            if (data == null)
            {
                return log;
            }

            foreach (var property in data.Properties())
            {
                log["_" + property.Name] = property.Value;
            }

            return log;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="shortMessage">The short message.</param>
        /// <param name="fullMessage">The full message.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationReference">The operation reference. This reference is used in events to match the source.</param>
        public abstract void Send(string shortMessage, string fullMessage, object data, Guid? operationReference = null);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="shortMessage">The short message.</param>
        /// <param name="fullMessage">The full message.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationReference">The operation reference. This reference is used in events to match the source.</param>
        public abstract void Send(string shortMessage, string fullMessage, JObject data, Guid? operationReference = null);

        /// <summary>
        /// Sends the message in async.
        /// </summary>
        /// <param name="shortMessage">The short message.</param>
        /// <param name="fullMessage">The full message.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationReference">The operation reference. This reference is used in events to match the source.</param>
        /// <returns>
        /// The task to wait
        /// </returns>
        public abstract Task SendAsync(string shortMessage, string fullMessage, object data, Guid? operationReference = null);

        /// <summary>
        /// Sends the message in async.
        /// </summary>
        /// <param name="shortMessage">The short message.</param>
        /// <param name="fullMessage">The full message.</param>
        /// <param name="data">The data.</param>
        /// <param name="operationReference">The operation reference. This reference is used in events to match the source.</param>
        /// <returns>
        /// The task to wait
        /// </returns>
        public abstract Task SendAsync(string shortMessage, string fullMessage, JObject data, Guid? operationReference = null);

        /// <summary>
        /// Sends the data directly to graylog.
        /// This can be used to send a messageBody returned in an error event.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        public abstract void SendData(byte[] messageBody);

        /// <summary>
        /// Converts a DateTime to the long representation which is the number of seconds since the unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime to convert to epoch time.</param>
        /// <returns>The long number of seconds since the unix epoch.</returns>
        protected static long ToEpoch(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Compresses the specified raw.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <returns>
        /// The compressed data
        /// </returns>
        protected byte[] Compress(byte[] raw, CompressionLevel compressionLevel)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, compressionLevel, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Dumps the object in additionnal data.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="data">The data.</param>
        protected void DumpObjectInAdditionnalData(JObject log, object data)
        {
            log.ThrowIfNull(nameof(log));

            if (data == null)
            {
                return;
            }

            foreach (var propertyInfo in data.GetType().GetProperties())
            {
                var dataName = $"_{propertyInfo.Name}";
                var dataValue = propertyInfo.GetValue(data).ToString();

                log[dataName] = dataValue;
            }
        }

        /// <summary>
        /// Reports the send error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="messageContent">Content of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="operationReference">The operation reference.</param>
        protected void ReportSendError(Exception exception, string messageContent, byte[] messageBody, Guid operationReference)
        {
            var error = new GraylogSendError { Exception = exception, MessageContent = messageContent, MessageBody = messageBody, OperationReference = operationReference };

            this.SendErrorOccured?.Invoke(this, error);
        }

        /// <summary>
        /// Reports the send success.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="operationReference">The operation reference.</param>
        protected void ReportSendSuccess(string messageContent, byte[] messageBody, Guid operationReference)
        {
            var result = new GraylogSendResult { MessageContent = messageContent, MessageBody = messageBody, OperationReference = operationReference };

            this.SendSuccessful?.Invoke(this, result);
        }
    }
}