// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.ClientModel.Primitives;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.ResourceManager.ApiManagement
{
    internal class PolicyFragmentContractOperationSource : IOperationSource<PolicyFragmentContractResource>
    {
        private readonly ArmClient _client;

        internal PolicyFragmentContractOperationSource(ArmClient client)
        {
            _client = client;
        }

        PolicyFragmentContractResource IOperationSource<PolicyFragmentContractResource>.CreateResult(Response response, CancellationToken cancellationToken)
        {
            var data = ModelReaderWriter.Read<PolicyFragmentContractData>(response.Content, ModelReaderWriterOptions.Json, AzureResourceManagerApiManagementContext.Default);
            return new PolicyFragmentContractResource(_client, data);
        }

        async ValueTask<PolicyFragmentContractResource> IOperationSource<PolicyFragmentContractResource>.CreateResultAsync(Response response, CancellationToken cancellationToken)
        {
            var data = ModelReaderWriter.Read<PolicyFragmentContractData>(response.Content, ModelReaderWriterOptions.Json, AzureResourceManagerApiManagementContext.Default);
            return await Task.FromResult(new PolicyFragmentContractResource(_client, data)).ConfigureAwait(false);
        }
    }
}
