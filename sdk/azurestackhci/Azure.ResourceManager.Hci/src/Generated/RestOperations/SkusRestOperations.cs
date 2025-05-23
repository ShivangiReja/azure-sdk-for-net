// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager.Hci.Models;

namespace Azure.ResourceManager.Hci
{
    internal partial class SkusRestOperations
    {
        private readonly TelemetryDetails _userAgent;
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;
        private readonly string _apiVersion;

        /// <summary> Initializes a new instance of SkusRestOperations. </summary>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="applicationId"> The application id to use for user agent. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="apiVersion"> Api Version. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="pipeline"/> or <paramref name="apiVersion"/> is null. </exception>
        public SkusRestOperations(HttpPipeline pipeline, string applicationId, Uri endpoint = null, string apiVersion = default)
        {
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _endpoint = endpoint ?? new Uri("https://management.azure.com");
            _apiVersion = apiVersion ?? "2024-04-01";
            _userAgent = new TelemetryDetails(GetType().Assembly, applicationId);
        }

        internal RequestUriBuilder CreateListByOfferRequestUri(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand)
        {
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.AzureStackHCI/clusters/", false);
            uri.AppendPath(clusterName, true);
            uri.AppendPath("/publishers/", false);
            uri.AppendPath(publisherName, true);
            uri.AppendPath("/offers/", false);
            uri.AppendPath(offerName, true);
            uri.AppendPath("/skus", false);
            uri.AppendQuery("api-version", _apiVersion, true);
            if (expand != null)
            {
                uri.AppendQuery("$expand", expand, true);
            }
            return uri;
        }

        internal HttpMessage CreateListByOfferRequest(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.AzureStackHCI/clusters/", false);
            uri.AppendPath(clusterName, true);
            uri.AppendPath("/publishers/", false);
            uri.AppendPath(publisherName, true);
            uri.AppendPath("/offers/", false);
            uri.AppendPath(offerName, true);
            uri.AppendPath("/skus", false);
            uri.AppendQuery("api-version", _apiVersion, true);
            if (expand != null)
            {
                uri.AppendQuery("$expand", expand, true);
            }
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            _userAgent.Apply(message);
            return message;
        }

        /// <summary> List Skus available for a offer within the HCI Cluster. </summary>
        /// <param name="subscriptionId"> The ID of the target subscription. The value must be an UUID. </param>
        /// <param name="resourceGroupName"> The name of the resource group. The name is case insensitive. </param>
        /// <param name="clusterName"> The name of the cluster. </param>
        /// <param name="publisherName"> The name of the publisher available within HCI cluster. </param>
        /// <param name="offerName"> The name of the offer available within HCI cluster. </param>
        /// <param name="expand"> Specify $expand=content,contentVersion to populate additional fields related to the marketplace offer. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is an empty string, and was expected to be non-empty. </exception>
        public async Task<Response<HciSkuList>> ListByOfferAsync(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Argument.AssertNotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            Argument.AssertNotNullOrEmpty(clusterName, nameof(clusterName));
            Argument.AssertNotNullOrEmpty(publisherName, nameof(publisherName));
            Argument.AssertNotNullOrEmpty(offerName, nameof(offerName));

            using var message = CreateListByOfferRequest(subscriptionId, resourceGroupName, clusterName, publisherName, offerName, expand);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        HciSkuList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, ModelSerializationExtensions.JsonDocumentOptions, cancellationToken).ConfigureAwait(false);
                        value = HciSkuList.DeserializeHciSkuList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> List Skus available for a offer within the HCI Cluster. </summary>
        /// <param name="subscriptionId"> The ID of the target subscription. The value must be an UUID. </param>
        /// <param name="resourceGroupName"> The name of the resource group. The name is case insensitive. </param>
        /// <param name="clusterName"> The name of the cluster. </param>
        /// <param name="publisherName"> The name of the publisher available within HCI cluster. </param>
        /// <param name="offerName"> The name of the offer available within HCI cluster. </param>
        /// <param name="expand"> Specify $expand=content,contentVersion to populate additional fields related to the marketplace offer. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is an empty string, and was expected to be non-empty. </exception>
        public Response<HciSkuList> ListByOffer(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Argument.AssertNotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            Argument.AssertNotNullOrEmpty(clusterName, nameof(clusterName));
            Argument.AssertNotNullOrEmpty(publisherName, nameof(publisherName));
            Argument.AssertNotNullOrEmpty(offerName, nameof(offerName));

            using var message = CreateListByOfferRequest(subscriptionId, resourceGroupName, clusterName, publisherName, offerName, expand);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        HciSkuList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream, ModelSerializationExtensions.JsonDocumentOptions);
                        value = HciSkuList.DeserializeHciSkuList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal RequestUriBuilder CreateGetRequestUri(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string skuName, string expand)
        {
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.AzureStackHCI/clusters/", false);
            uri.AppendPath(clusterName, true);
            uri.AppendPath("/publishers/", false);
            uri.AppendPath(publisherName, true);
            uri.AppendPath("/offers/", false);
            uri.AppendPath(offerName, true);
            uri.AppendPath("/skus/", false);
            uri.AppendPath(skuName, true);
            uri.AppendQuery("api-version", _apiVersion, true);
            if (expand != null)
            {
                uri.AppendQuery("$expand", expand, true);
            }
            return uri;
        }

        internal HttpMessage CreateGetRequest(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string skuName, string expand)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.AzureStackHCI/clusters/", false);
            uri.AppendPath(clusterName, true);
            uri.AppendPath("/publishers/", false);
            uri.AppendPath(publisherName, true);
            uri.AppendPath("/offers/", false);
            uri.AppendPath(offerName, true);
            uri.AppendPath("/skus/", false);
            uri.AppendPath(skuName, true);
            uri.AppendQuery("api-version", _apiVersion, true);
            if (expand != null)
            {
                uri.AppendQuery("$expand", expand, true);
            }
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            _userAgent.Apply(message);
            return message;
        }

        /// <summary> Get SKU resource details within a offer of HCI Cluster. </summary>
        /// <param name="subscriptionId"> The ID of the target subscription. The value must be an UUID. </param>
        /// <param name="resourceGroupName"> The name of the resource group. The name is case insensitive. </param>
        /// <param name="clusterName"> The name of the cluster. </param>
        /// <param name="publisherName"> The name of the publisher available within HCI cluster. </param>
        /// <param name="offerName"> The name of the offer available within HCI cluster. </param>
        /// <param name="skuName"> The name of the SKU available within HCI cluster. </param>
        /// <param name="expand"> Specify $expand=content,contentVersion to populate additional fields related to the marketplace offer. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/>, <paramref name="offerName"/> or <paramref name="skuName"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/>, <paramref name="offerName"/> or <paramref name="skuName"/> is an empty string, and was expected to be non-empty. </exception>
        public async Task<Response<HciSkuData>> GetAsync(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string skuName, string expand = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Argument.AssertNotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            Argument.AssertNotNullOrEmpty(clusterName, nameof(clusterName));
            Argument.AssertNotNullOrEmpty(publisherName, nameof(publisherName));
            Argument.AssertNotNullOrEmpty(offerName, nameof(offerName));
            Argument.AssertNotNullOrEmpty(skuName, nameof(skuName));

            using var message = CreateGetRequest(subscriptionId, resourceGroupName, clusterName, publisherName, offerName, skuName, expand);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        HciSkuData value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, ModelSerializationExtensions.JsonDocumentOptions, cancellationToken).ConfigureAwait(false);
                        value = HciSkuData.DeserializeHciSkuData(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 404:
                    return Response.FromValue((HciSkuData)null, message.Response);
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Get SKU resource details within a offer of HCI Cluster. </summary>
        /// <param name="subscriptionId"> The ID of the target subscription. The value must be an UUID. </param>
        /// <param name="resourceGroupName"> The name of the resource group. The name is case insensitive. </param>
        /// <param name="clusterName"> The name of the cluster. </param>
        /// <param name="publisherName"> The name of the publisher available within HCI cluster. </param>
        /// <param name="offerName"> The name of the offer available within HCI cluster. </param>
        /// <param name="skuName"> The name of the SKU available within HCI cluster. </param>
        /// <param name="expand"> Specify $expand=content,contentVersion to populate additional fields related to the marketplace offer. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/>, <paramref name="offerName"/> or <paramref name="skuName"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/>, <paramref name="offerName"/> or <paramref name="skuName"/> is an empty string, and was expected to be non-empty. </exception>
        public Response<HciSkuData> Get(string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string skuName, string expand = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Argument.AssertNotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            Argument.AssertNotNullOrEmpty(clusterName, nameof(clusterName));
            Argument.AssertNotNullOrEmpty(publisherName, nameof(publisherName));
            Argument.AssertNotNullOrEmpty(offerName, nameof(offerName));
            Argument.AssertNotNullOrEmpty(skuName, nameof(skuName));

            using var message = CreateGetRequest(subscriptionId, resourceGroupName, clusterName, publisherName, offerName, skuName, expand);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        HciSkuData value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream, ModelSerializationExtensions.JsonDocumentOptions);
                        value = HciSkuData.DeserializeHciSkuData(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 404:
                    return Response.FromValue((HciSkuData)null, message.Response);
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal RequestUriBuilder CreateListByOfferNextPageRequestUri(string nextLink, string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand)
        {
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendRawNextLink(nextLink, false);
            return uri;
        }

        internal HttpMessage CreateListByOfferNextPageRequest(string nextLink, string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendRawNextLink(nextLink, false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            _userAgent.Apply(message);
            return message;
        }

        /// <summary> List Skus available for a offer within the HCI Cluster. </summary>
        /// <param name="nextLink"> The URL to the next page of results. </param>
        /// <param name="subscriptionId"> The ID of the target subscription. The value must be an UUID. </param>
        /// <param name="resourceGroupName"> The name of the resource group. The name is case insensitive. </param>
        /// <param name="clusterName"> The name of the cluster. </param>
        /// <param name="publisherName"> The name of the publisher available within HCI cluster. </param>
        /// <param name="offerName"> The name of the offer available within HCI cluster. </param>
        /// <param name="expand"> Specify $expand=content,contentVersion to populate additional fields related to the marketplace offer. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="nextLink"/>, <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is an empty string, and was expected to be non-empty. </exception>
        public async Task<Response<HciSkuList>> ListByOfferNextPageAsync(string nextLink, string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNull(nextLink, nameof(nextLink));
            Argument.AssertNotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Argument.AssertNotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            Argument.AssertNotNullOrEmpty(clusterName, nameof(clusterName));
            Argument.AssertNotNullOrEmpty(publisherName, nameof(publisherName));
            Argument.AssertNotNullOrEmpty(offerName, nameof(offerName));

            using var message = CreateListByOfferNextPageRequest(nextLink, subscriptionId, resourceGroupName, clusterName, publisherName, offerName, expand);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        HciSkuList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, ModelSerializationExtensions.JsonDocumentOptions, cancellationToken).ConfigureAwait(false);
                        value = HciSkuList.DeserializeHciSkuList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> List Skus available for a offer within the HCI Cluster. </summary>
        /// <param name="nextLink"> The URL to the next page of results. </param>
        /// <param name="subscriptionId"> The ID of the target subscription. The value must be an UUID. </param>
        /// <param name="resourceGroupName"> The name of the resource group. The name is case insensitive. </param>
        /// <param name="clusterName"> The name of the cluster. </param>
        /// <param name="publisherName"> The name of the publisher available within HCI cluster. </param>
        /// <param name="offerName"> The name of the offer available within HCI cluster. </param>
        /// <param name="expand"> Specify $expand=content,contentVersion to populate additional fields related to the marketplace offer. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="nextLink"/>, <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="clusterName"/>, <paramref name="publisherName"/> or <paramref name="offerName"/> is an empty string, and was expected to be non-empty. </exception>
        public Response<HciSkuList> ListByOfferNextPage(string nextLink, string subscriptionId, string resourceGroupName, string clusterName, string publisherName, string offerName, string expand = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNull(nextLink, nameof(nextLink));
            Argument.AssertNotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Argument.AssertNotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            Argument.AssertNotNullOrEmpty(clusterName, nameof(clusterName));
            Argument.AssertNotNullOrEmpty(publisherName, nameof(publisherName));
            Argument.AssertNotNullOrEmpty(offerName, nameof(offerName));

            using var message = CreateListByOfferNextPageRequest(nextLink, subscriptionId, resourceGroupName, clusterName, publisherName, offerName, expand);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        HciSkuList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream, ModelSerializationExtensions.JsonDocumentOptions);
                        value = HciSkuList.DeserializeHciSkuList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }
    }
}
