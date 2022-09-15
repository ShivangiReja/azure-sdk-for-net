// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Core;

namespace Azure.Search.Documents.Tests.Utilities
{
    public class UnsafeForceHttpPolicy : HttpPipelineSynchronousPolicy
    {
        public override void OnSendingRequest(HttpMessage message) =>
            message.Request.Uri.Scheme = "http";
    }
}
