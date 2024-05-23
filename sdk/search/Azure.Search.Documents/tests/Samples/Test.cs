// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using Azure.AI.OpenAI;
using Azure.Search.Documents.Models;
using NUnit.Framework;

namespace Azure.Search.Documents.Tests.Samples
{
    internal class Test
    {
        [Test]
        public static void MyMethod()
        {
            MyEmbeddings embeddings = new();
            EmbeddingVector vector = embeddings.Vector;

            if (vector.Type == EmbeddingType.Single)
            {
                ReadOnlyMemory<float> floatEmbeddings = vector.ToFloat32Memory();
            }

            Uri endpoint = new Uri(Environment.GetEnvironmentVariable("SEARCH_ENDPOINT"));
            AzureKeyCredential credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("SEARCH_API_KEY"));
            SearchClient searchClient = new SearchClient(endpoint, "indexName", credential);

            SearchResults<Hotel> response = searchClient.Search<Hotel>(
                    new SearchOptions
                    {
                        VectorSearch = new()
                        {
                            Queries = { new VectorizedQuery(null) { KNearestNeighborsCount = 3, Fields = { "DescriptionVector" } } }
                        }
                    });
        }
    }
}
