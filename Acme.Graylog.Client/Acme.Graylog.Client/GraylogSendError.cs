// -----------------------------------------------------------------------
//  <copyright file="GraylogSendError.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client
{
    using System;
    using System.Linq;

    /// <summary>
    /// An error occured when sending the message to graylog
    /// </summary>
    public class GraylogSendError
    {
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        /// <value>
        /// The message body.
        /// </value>
        public byte[] MessageBody { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        /// <value>
        /// The content of the message.
        /// </value>
        public string MessageContent { get; set; }
    }
}