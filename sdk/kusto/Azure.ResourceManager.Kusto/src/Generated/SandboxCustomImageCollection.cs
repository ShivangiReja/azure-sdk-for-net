// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Autorest.CSharp.Core;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.ResourceManager.Kusto
{
    /// <summary>
    /// A class representing a collection of <see cref="SandboxCustomImageResource"/> and their operations.
    /// Each <see cref="SandboxCustomImageResource"/> in the collection will belong to the same instance of <see cref="KustoClusterResource"/>.
    /// To get a <see cref="SandboxCustomImageCollection"/> instance call the GetSandboxCustomImages method from an instance of <see cref="KustoClusterResource"/>.
    /// </summary>
    public partial class SandboxCustomImageCollection : ArmCollection, IEnumerable<SandboxCustomImageResource>, IAsyncEnumerable<SandboxCustomImageResource>
    {
        private readonly ClientDiagnostics _sandboxCustomImageClientDiagnostics;
        private readonly SandboxCustomImagesRestOperations _sandboxCustomImageRestClient;

        /// <summary> Initializes a new instance of the <see cref="SandboxCustomImageCollection"/> class for mocking. </summary>
        protected SandboxCustomImageCollection()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="SandboxCustomImageCollection"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the parent resource that is the target of operations. </param>
        internal SandboxCustomImageCollection(ArmClient client, ResourceIdentifier id) : base(client, id)
        {
            _sandboxCustomImageClientDiagnostics = new ClientDiagnostics("Azure.ResourceManager.Kusto", SandboxCustomImageResource.ResourceType.Namespace, Diagnostics);
            TryGetApiVersion(SandboxCustomImageResource.ResourceType, out string sandboxCustomImageApiVersion);
            _sandboxCustomImageRestClient = new SandboxCustomImagesRestOperations(Pipeline, Diagnostics.ApplicationId, Endpoint, sandboxCustomImageApiVersion);
#if DEBUG
			ValidateResourceId(Id);
#endif
        }

        internal static void ValidateResourceId(ResourceIdentifier id)
        {
            if (id.ResourceType != KustoClusterResource.ResourceType)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource type {0} expected {1}", id.ResourceType, KustoClusterResource.ResourceType), nameof(id));
        }

        /// <summary>
        /// Creates or updates a sandbox custom image.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_CreateOrUpdate</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="data"> The sandbox custom image parameters. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> or <paramref name="data"/> is null. </exception>
        public virtual async Task<ArmOperation<SandboxCustomImageResource>> CreateOrUpdateAsync(WaitUntil waitUntil, string sandboxCustomImageName, SandboxCustomImageData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = await _sandboxCustomImageRestClient.CreateOrUpdateAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, data, cancellationToken).ConfigureAwait(false);
                var operation = new KustoArmOperation<SandboxCustomImageResource>(new SandboxCustomImageOperationSource(Client), _sandboxCustomImageClientDiagnostics, Pipeline, _sandboxCustomImageRestClient.CreateCreateOrUpdateRequest(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, data).Request, response, OperationFinalStateVia.Location);
                if (waitUntil == WaitUntil.Completed)
                    await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Creates or updates a sandbox custom image.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_CreateOrUpdate</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="data"> The sandbox custom image parameters. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> or <paramref name="data"/> is null. </exception>
        public virtual ArmOperation<SandboxCustomImageResource> CreateOrUpdate(WaitUntil waitUntil, string sandboxCustomImageName, SandboxCustomImageData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = _sandboxCustomImageRestClient.CreateOrUpdate(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, data, cancellationToken);
                var operation = new KustoArmOperation<SandboxCustomImageResource>(new SandboxCustomImageOperationSource(Client), _sandboxCustomImageClientDiagnostics, Pipeline, _sandboxCustomImageRestClient.CreateCreateOrUpdateRequest(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, data).Request, response, OperationFinalStateVia.Location);
                if (waitUntil == WaitUntil.Completed)
                    operation.WaitForCompletion(cancellationToken);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Returns a sandbox custom image
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_Get</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> is null. </exception>
        public virtual async Task<Response<SandboxCustomImageResource>> GetAsync(string sandboxCustomImageName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.Get");
            scope.Start();
            try
            {
                var response = await _sandboxCustomImageRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new SandboxCustomImageResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Returns a sandbox custom image
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_Get</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> is null. </exception>
        public virtual Response<SandboxCustomImageResource> Get(string sandboxCustomImageName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.Get");
            scope.Start();
            try
            {
                var response = _sandboxCustomImageRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, cancellationToken);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new SandboxCustomImageResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Returns the list of the existing sandbox custom images of the given Kusto cluster.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_ListByCluster</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="SandboxCustomImageResource"/> that may take multiple service requests to iterate over. </returns>
        public virtual AsyncPageable<SandboxCustomImageResource> GetAllAsync(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _sandboxCustomImageRestClient.CreateListByClusterRequest(Id.SubscriptionId, Id.ResourceGroupName, Id.Name);
            return GeneratorPageableHelpers.CreateAsyncPageable(FirstPageRequest, null, e => new SandboxCustomImageResource(Client, SandboxCustomImageData.DeserializeSandboxCustomImageData(e)), _sandboxCustomImageClientDiagnostics, Pipeline, "SandboxCustomImageCollection.GetAll", "value", null, cancellationToken);
        }

        /// <summary>
        /// Returns the list of the existing sandbox custom images of the given Kusto cluster.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_ListByCluster</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="SandboxCustomImageResource"/> that may take multiple service requests to iterate over. </returns>
        public virtual Pageable<SandboxCustomImageResource> GetAll(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _sandboxCustomImageRestClient.CreateListByClusterRequest(Id.SubscriptionId, Id.ResourceGroupName, Id.Name);
            return GeneratorPageableHelpers.CreatePageable(FirstPageRequest, null, e => new SandboxCustomImageResource(Client, SandboxCustomImageData.DeserializeSandboxCustomImageData(e)), _sandboxCustomImageClientDiagnostics, Pipeline, "SandboxCustomImageCollection.GetAll", "value", null, cancellationToken);
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_Get</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> is null. </exception>
        public virtual async Task<Response<bool>> ExistsAsync(string sandboxCustomImageName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.Exists");
            scope.Start();
            try
            {
                var response = await _sandboxCustomImageRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, cancellationToken: cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_Get</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> is null. </exception>
        public virtual Response<bool> Exists(string sandboxCustomImageName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.Exists");
            scope.Start();
            try
            {
                var response = _sandboxCustomImageRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, cancellationToken: cancellationToken);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Tries to get details for this resource from the service.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_Get</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> is null. </exception>
        public virtual async Task<NullableResponse<SandboxCustomImageResource>> GetIfExistsAsync(string sandboxCustomImageName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.GetIfExists");
            scope.Start();
            try
            {
                var response = await _sandboxCustomImageRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    return new NoValueResponse<SandboxCustomImageResource>(response.GetRawResponse());
                return Response.FromValue(new SandboxCustomImageResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Tries to get details for this resource from the service.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Kusto/clusters/{clusterName}/sandboxCustomImages/{sandboxCustomImageName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SandboxCustomImages_Get</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2024-04-13</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="SandboxCustomImageResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sandboxCustomImageName"> The name of the sandbox custom image. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sandboxCustomImageName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sandboxCustomImageName"/> is null. </exception>
        public virtual NullableResponse<SandboxCustomImageResource> GetIfExists(string sandboxCustomImageName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sandboxCustomImageName, nameof(sandboxCustomImageName));

            using var scope = _sandboxCustomImageClientDiagnostics.CreateScope("SandboxCustomImageCollection.GetIfExists");
            scope.Start();
            try
            {
                var response = _sandboxCustomImageRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, Id.Name, sandboxCustomImageName, cancellationToken: cancellationToken);
                if (response.Value == null)
                    return new NoValueResponse<SandboxCustomImageResource>(response.GetRawResponse());
                return Response.FromValue(new SandboxCustomImageResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        IEnumerator<SandboxCustomImageResource> IEnumerable<SandboxCustomImageResource>.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IAsyncEnumerator<SandboxCustomImageResource> IAsyncEnumerable<SandboxCustomImageResource>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAllAsync(cancellationToken: cancellationToken).GetAsyncEnumerator(cancellationToken);
        }
    }
}
