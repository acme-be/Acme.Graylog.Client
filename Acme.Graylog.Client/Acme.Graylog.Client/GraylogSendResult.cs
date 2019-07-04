// -----------------------------------------------------------------------
//  <copyright file="GraylogSendResult.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represent a result of data being sent by the client
    /// </summary>
    public class GraylogSendResult
    {
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

        /// <summary>
        /// Gets or sets the operation reference.
        /// </summary>
        /// <value>
        /// The operation reference.
        /// </value>
        public Guid OperationReference { get; set; }
    }
}