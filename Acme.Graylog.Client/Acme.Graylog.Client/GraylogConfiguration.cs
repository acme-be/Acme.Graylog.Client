// -----------------------------------------------------------------------
//  <copyright file="GraylogConfiguration.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Information about the graylog configuration
    /// </summary>
    [DataContract]
    public class GraylogConfiguration
    {
        /// <summary>
        /// Gets or sets the client certificate name.
        /// </summary>
        /// <value>
        /// The client certificate name.
        /// </value>
        [DataMember(Name = "clientCertificateName")]
        public string ClientCertificateName { get; set; }

        /// <summary>
        /// Gets or sets the client certificate password.
        /// </summary>
        /// <value>
        /// The client certificate password.
        /// </value>
        [DataMember(Name = "clientCertificatePassword")]
        public string ClientCertificatePassword { get; set; }

        /// <summary>
        /// Gets or sets the client certificate path.
        /// </summary>
        /// <value>
        /// The client certificate path.
        /// </value>
        [DataMember(Name = "clientCertificatePath")]
        public string ClientCertificatePath { get; set; }

        /// <summary>
        /// Gets or sets the facility.
        /// </summary>
        /// <value>
        /// The facility.
        /// </value>
        [DataMember(Name = "facility")]
        public string Facility { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        [DataMember(Name = "host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        [DataMember(Name = "port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the request timeout.
        /// </summary>
        /// <value>
        /// The request timeout.
        /// </value>
        [DataMember(Name = "requestTimeout")]
        public int RequestTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client must use compression.
        /// </summary>
        /// <value>
        /// <c>true</c> if the client must use compression; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "useCompression")]
        public bool UseCompression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client must use ssl.
        /// </summary>
        /// <value>
        /// <c>true</c> if the client must use ssl; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "useSsl")]
        public bool UseSsl { get; set; }
    }
}