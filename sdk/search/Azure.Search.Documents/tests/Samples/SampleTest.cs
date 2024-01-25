// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using NUnit.Framework;
using Azure.Core.TestFramework;
using Azure.Search.Documents.Models;

namespace Azure.Search.Documents.Tests.Samples
{
    public partial class SampleTest : SearchTestBase
    {
        public SampleTest(bool async, SearchClientOptions.ServiceVersion serviceVersion)
            : base(async, serviceVersion, RecordedTestMode.Live /* to re-record */)
        {
        }

        [Test]
        public async Task TestSearchableStringCollection()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);
            try
            {
                var indexName = Recording.Random.GetName();
                resources.IndexName = indexName;

                //--------Create Index-------------
                SearchIndex searchIndex = GetIndex(indexName);

                Uri endpoint = new(resources.Endpoint.ToString());
                string key = resources.PrimaryApiKey;
                AzureKeyCredential credential = new(key);

                SearchIndexClient indexClient = InstrumentClient(new SearchIndexClient(endpoint, credential, GetSearchClientOptions()));
                await indexClient.CreateIndexAsync(searchIndex);

                SearchIndex createdIndex = await indexClient.GetIndexAsync(indexName);
                Assert.AreEqual(indexName, createdIndex.Name);
                Assert.AreEqual(searchIndex.Fields.Count, createdIndex.Fields.Count);

                //--------Upload data-------------
                SearchClient searchClient = resources.GetSearchClient();
                var documents = GetDocuments();
                await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(documents));
                await resources.WaitForIndexingAsync();

                //--------Query index-------------
                SearchResults<MyDocument> response = await searchClient.SearchAsync<MyDocument>("luxury");

                int count = 0;
                Console.WriteLine($"Search Results:");
                await foreach (SearchResult<MyDocument> result in response.GetResultsAsync())
                {
                    count++;
                    MyDocument doc = result.Document;
                    Console.WriteLine($"{doc.Id}: {doc.Tags}");
                }
                Console.WriteLine($"Total number of search results:{count}"); // Returns document 1 and 3
            }
            finally
            {
                await resources.GetIndexClient().DeleteIndexAsync(resources.IndexName);
            }
        }

        public static SearchIndex GetIndex(string name) =>
            new SearchIndex(name)
            {
                Fields =
                {
                    new SimpleField("Id", SearchFieldDataType.String) { IsKey = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new SearchableField("Tags", collection: true) { IsFilterable = true, IsFacetable = true },
                },
            };

        public class MyDocument
        {
            public string Id { get; set; }

            public string[] Tags { get; set; }
        }

        public static MyDocument[] GetDocuments()
        {
            return new[]
            {
                new MyDocument()
                {
                    Id = "1",
                    Tags = new[] { "luxury hotel", "pool", "view", "wifi", "concierge" },
                },
                new MyDocument()
                {
                     Id = "2",
                     Tags = new[] { "motel", "budget" },
                },
                new MyDocument()
                {
                    Id = "3",
                    Tags = new[] { "luxury hotel", "24 hour front desk service", "coffee in lobby", "restaurant" },
                }
            };
        }
    }
}
