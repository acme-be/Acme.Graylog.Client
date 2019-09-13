// -----------------------------------------------------------------------
//  <copyright file="GraylogTlsClientError.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Graylog.Client
{
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;

    public class GraylogTlsClientError
    {
        /// <summary>
        /// Gets the certificate
        /// </summary>
        public X509Certificate Certificate { get; internal set; }

        /// <summary>
        /// Gets the validation chain
        /// </summary>
        public X509Chain Chain { get; internal set; }

        /// <summary>
        /// Gets the validation errors
        /// </summary>
        public SslPolicyErrors Errors { get; internal set; }
    }
}