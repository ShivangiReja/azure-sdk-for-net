// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using NUnit.Framework;
using Azure.Core.TestFramework;

namespace Azure.Search.Documents.Tests.Samples.VectorSearch
{
    public partial class VectorSample: SearchTestBase
    {
        public VectorSample(bool async, SearchClientOptions.ServiceVersion serviceVersion)
            : base(async, serviceVersion, RecordedTestMode.Live /* to re-record */)
        {
        }

        private class Model
        {
            public string Id { get; set; }
            public string Description { get; set; }
        }

        [Test]
        public async Task MergeOrUploadTest()
        {
            string indexName = "mysearchindex";
            SearchIndex searchIndex = new(indexName)
            {
                Fields =
                {
                    new SimpleField("Id", SearchFieldDataType.String) { IsKey = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new SearchableField("Description") { IsFilterable = true },
                }
            };

            Uri endpoint = new(Environment.GetEnvironmentVariable("SEARCH_ENDPOINT"));
            string key = Environment.GetEnvironmentVariable("SEARCH_API_KEY");
            AzureKeyCredential credential = new(key);

            SearchIndexClient indexClient = new(endpoint, credential);
            await indexClient.CreateIndexAsync(searchIndex);

            Model document = new Model()
            {
                Id = "1",
                Description =
                        "Best hotel in town if you like luxury hotels. They have an amazing infinity pool, a spa, " +
                        "and a really helpful concierge. The location is perfect -- right downtown, close to all " +
                        "the tourist attractions. We highly recommend this hotel.",
            };

            SearchClient searchClient = new(endpoint, indexName, credential);

            var batch = IndexDocumentsBatch.MergeOrUpload(new[] { document });
            Response<IndexDocumentsResult> result = await searchClient.IndexDocumentsAsync(batch);

            Assert.AreEqual(200, result.GetRawResponse().Status);
            Assert.AreEqual("1", result.Value.Results[0].Key);
        }
    }
}
