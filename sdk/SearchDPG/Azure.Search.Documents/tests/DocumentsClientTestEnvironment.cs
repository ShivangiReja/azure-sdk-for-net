// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.TestFramework;

namespace Azure.Search.Documents.Tests
{
    public class DocumentsClientTestEnvironment : TestEnvironment
    {
        public string Endpoint => GetRecordedVariable("Documents_ENDPOINT");

        // Add other client paramters here as above.
    }
}
