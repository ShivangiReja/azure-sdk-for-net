// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Data.AppConfiguration.Tests
{
    internal class CustomRequestPolicy : HttpPipelineSynchronousPolicy
    {
        public override void OnSendingRequest(HttpMessage message)
        {
            message.Request.Headers.Add(HttpHeader.Names.UserAgent, "test-shivangi");
        }
    }
}
