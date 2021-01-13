// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ReverseProxy.Utilities;

namespace Microsoft.ReverseProxy.Abstractions
{
    /// <summary>
    /// Describes a route that matches incoming requests based on a the <see cref="Match"/> criteria
    /// and proxies matching requests to the cluster identified by its <see cref="ClusterId"/>.
    /// </summary>
    public sealed record ProxyRoute
    {
        /// <summary>
        /// Globally unique identifier of the route.
        /// </summary>
        public string RouteId { get; init; }

        public ProxyMatch Match { get; init; } = new ProxyMatch();

        /// <summary>
        /// Optionally, an order value for this route. Routes with lower numbers take precedence over higher numbers.
        /// </summary>
        public int? Order { get; init; }

        /// <summary>
        /// Gets or sets the cluster that requests matching this route
        /// should be proxied to.
        /// </summary>
        public string ClusterId { get; init; }

        /// <summary>
        /// The name of the AuthorizationPolicy to apply to this route.
        /// If not set then only the FallbackPolicy will apply.
        /// Set to "Default" to enable authorization with the applications default policy.
        /// Set to "Anonymous" to disable all authorization checks for this route.
        /// </summary>
        public string AuthorizationPolicy { get; init; }

        /// <summary>
        /// The name of the CorsPolicy to apply to this route.
        /// If not set then the route won't be automatically matched for cors preflight requests.
        /// Set to "Default" to enable cors with the default policy.
        /// Set to "Disable" to refuses cors requests for this route.
        /// </summary>
        public string CorsPolicy { get; init; }

        /// <summary>
        /// Arbitrary key-value pairs that further describe this route.
        /// </summary>
        public IReadOnlyDictionary<string, string> Metadata { get; init; }

        /// <summary>
        /// Parameters used to transform the request and response. See <see cref="Service.ITransformBuilder"/>.
        /// </summary>
        public IReadOnlyList<IReadOnlyDictionary<string, string>> Transforms { get; init; }

        public static bool Equals(ProxyRoute proxyRoute1, ProxyRoute proxyRoute2)
        {
            if (proxyRoute1 == null && proxyRoute2 == null)
            {
                return true;
            }

            if (proxyRoute1 == null || proxyRoute2 == null)
            {
                return false;
            }

            return proxyRoute1.Order == proxyRoute2.Order
                && string.Equals(proxyRoute1.RouteId, proxyRoute2.RouteId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(proxyRoute1.ClusterId, proxyRoute2.ClusterId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(proxyRoute1.AuthorizationPolicy, proxyRoute2.AuthorizationPolicy, StringComparison.OrdinalIgnoreCase)
                && string.Equals(proxyRoute1.CorsPolicy, proxyRoute2.CorsPolicy, StringComparison.OrdinalIgnoreCase)
                && ProxyMatch.Equals(proxyRoute1.Match, proxyRoute2.Match)
                && CaseInsensitiveEqualHelper.Equals(proxyRoute1.Metadata, proxyRoute2.Metadata)
                && CaseInsensitiveEqualHelper.Equals(proxyRoute1.Transforms, proxyRoute2.Transforms);
        }
    }
}
