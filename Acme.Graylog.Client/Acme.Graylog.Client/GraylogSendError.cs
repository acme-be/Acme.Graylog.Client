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
    public class GraylogSendError : GraylogSendResult
    {
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}