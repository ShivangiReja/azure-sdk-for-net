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
using Newtonsoft.Json.Linq;

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
        public async Task TestGeographyPointOption1()
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
                Location = GeographyPoint.Create(-75.5646879643, 39.7093928328)
            };

            SearchClient searchClient = resources.GetSearchClient(clientOptions);
            await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(new[] { document }));
            await resources.WaitForIndexingAsync();

            // Search the docuement
            Response<SearchResults<Mountain>> results = await searchClient.SearchAsync<Mountain>("Rainier");
            await foreach (SearchResult<Mountain> result in results.Value.GetResultsAsync())
            {
                Mountain mountain = result.Document;

                string locationJson = System.Text.Json.JsonSerializer.Serialize(mountain.Location);
                JToken actual = JToken.Parse(locationJson);

                string expactedLocationJson =
                            @"{
                                ""Latitude"": -75.5646879643,
                                ""Longitude"": 39.7093928328,
                                ""Z"": null,
                                ""M"": null,
                                ""CoordinateSystem"": {
                                     ""EpsgId"": 4326,
                                     ""Id"": ""4326"",
                                     ""Name"": ""WGS84""
                                    },
                               ""IsEmpty"": false,
                              }";
                JToken expected = JToken.Parse(expactedLocationJson);

                Assert.AreEqual(expected, actual);
            }
        }
        public class Mountain
        {
            [SimpleField(IsKey = true)]
            public string Id { get; set; }

            [SearchableField(IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
            public string Name { get; set; }

            [SimpleField(IsFilterable = true)]
            public Microsoft.Spatial.GeographyPoint Location { get; set; }
        }

        [Test]
        public async Task SearchGeographyPointUsingSerializer()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            //------- Create Index ----------------
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("Id", SearchFieldDataType.String) { IsKey = true },
                    new SearchableField("Name") { IsFilterable = true, IsSortable = false },
                    new SimpleField("Location", SearchFieldDataType.GeographyPoint) { IsFilterable = true },
                },
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            await indexClient.CreateIndexAsync(index);

            SearchIndex createdIndex = await indexClient.GetIndexAsync(indexName);
            Assert.AreEqual(indexName, createdIndex.Name);

            //------- Upload data ------------------

            SearchDocument document = new SearchDocument
            {
                ["Id"] = "1",
                ["Name"] = "Rainier",
                ["Location"] = GeographyPoint.Create(-75.5646879643, 39.7093928328)
            };

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                Converters =
                {
                    new NewtonsoftJsonMicrosoftSpatialGeoJsonConverter()
                }
            };
            SearchClientOptions clientOptions = new SearchClientOptions
            {
                Serializer = new NewtonsoftJsonObjectSerializer(serializerSettings)
            };
            SearchClient searchClient = resources.GetSearchClient(clientOptions);
            await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(new[] { document }));
            await resources.WaitForIndexingAsync();

            //--------- Search the docuement ------------
            Response<SearchResults<SearchDocument>> results = await searchClient.SearchAsync<SearchDocument>("Rainier");
            await foreach (SearchResult<SearchDocument> result in results.Value.GetResultsAsync())
            {
                SearchDocument doc = result.Document;
                dynamic location = doc["Location"];

                string locationJsonString =
                    @"{
                        ""type"": ""Point"",
                        ""coordinates"": [
                            39.7093928328,
                            -75.5646879643
                         ],
                        ""crs"": {
                            ""type"": ""name"",
                            ""properties"": {
                                ""name"": ""EPSG:4326""
                            }
                         }
                     }";

                Console.WriteLine(locationJsonString);
            }
        }
    }
}
