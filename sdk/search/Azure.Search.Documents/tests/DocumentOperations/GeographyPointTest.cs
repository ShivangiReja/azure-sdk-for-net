// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.TestFramework;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using NUnit.Framework;
using System.Threading.Tasks;
using Azure.Search.Documents.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Azure.Core.Serialization;
using Microsoft.Spatial;
using System;

namespace Azure.Search.Documents.Tests
{
    [ClientTestFixture(SearchClientOptions.ServiceVersion.V2023_10_01_Preview)]
    public partial class GeographyPointTest : SearchTestBase
    {
        public GeographyPointTest(bool async, SearchClientOptions.ServiceVersion serviceVersion)
            : base(async, serviceVersion, null /* RecordedTestMode.Record /* to re-record */)
        {
        }

        [Test]
        public async Task TestGeographyPoint()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters =
                {
                    new NewtonsoftJsonMicrosoftSpatialGeoJsonConverter()
                }
            };
            SearchClientOptions clientOptions = new SearchClientOptions
            {
                Serializer = new NewtonsoftJsonObjectSerializer(serializerSettings)
            };

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields = new FieldBuilder().Build(typeof(Mountain)),
            };

            SearchIndexClient indexClient = resources.GetIndexClient(clientOptions);
            await indexClient.CreateIndexAsync(index);

            SearchIndex createdIndex = await indexClient.GetIndexAsync(indexName);
            Assert.AreEqual(indexName, createdIndex.Name);

            // Upload data
            Mountain document = new Mountain
            {
               Id = "1",
               Name = "Rainier",
               Summit = TestExtensions.CreatePoint(-75.5646879643, 39.7093928328)
            };

            SearchClient searchClient = resources.GetSearchClient(clientOptions);
            await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(new[] { document }));
            await resources.WaitForIndexingAsync();

            // Search the docuement
            Response<SearchResults<Mountain>> results = await searchClient.SearchAsync<Mountain>("Rainier");
            await foreach (SearchResult<Mountain> result in results.Value.GetResultsAsync())
            {
                Mountain mountain = result.Document;
                Console.WriteLine("https://www.bing.com/maps?cp={0}~{1}&sp=point.{0}_{1}_{2}",
                    mountain.Summit.Latitude,
                    mountain.Summit.Longitude,
                    Uri.EscapeDataString(mountain.Name));
            }
        }
        public class Mountain
        {
            [SimpleField(IsKey = true)]
            public string Id { get; set; }

            [SearchableField(IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
            public string Name { get; set; }

            [SimpleField(IsFilterable = true)]
            public Microsoft.Spatial.GeographyPoint Summit { get; set; }
        }
    }
}
