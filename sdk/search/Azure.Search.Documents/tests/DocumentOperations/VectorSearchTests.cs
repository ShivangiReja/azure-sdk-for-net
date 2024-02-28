// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using NUnit.Framework;

namespace Azure.Search.Documents.Tests
{
    [ClientTestFixture(SearchClientOptions.ServiceVersion.V2023_11_01, SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
    public partial class VectorSearchTests : SearchTestBase
    {
        public VectorSearchTests(bool async, SearchClientOptions.ServiceVersion serviceVersion)
            : base(async, serviceVersion, RecordedTestMode.Live /* to re-record */)
        {
        }

        private async Task AssertKeysEqual<T>(
            SearchResults<T> response,
            Func<SearchResult<T>, string> keyAccessor,
            params string[] expectedKeys)
        {
            List<SearchResult<T>> docs = await response.GetResultsAsync().ToListAsync();
            CollectionAssert.AreEquivalent(expectedKeys, docs.Select(keyAccessor));
        }

        [Test]
        public async Task SingleVectorSearch()
        {
            await using SearchResources resources = await SearchResources.CreateWithHotelsIndexAsync(this);

            var vectorizedResult = VectorSearchEmbeddings.SearchVectorizeDescription; // "Top hotels in town"
            await Task.Delay(TimeSpan.FromSeconds(1));

            SearchResults<Hotel> response = await resources.GetSearchClient().SearchAsync<Hotel>(
                   new SearchOptions
                   {
                       VectorSearch = new()
                       {
                           Queries = { new VectorizedQuery(vectorizedResult) { KNearestNeighborsCount = 3, Fields = { "descriptionVector" } } }
                       },
                       Select = { "hotelId", "hotelName" }
                   });

            await AssertKeysEqual(
                response,
                h => h.Document.HotelId,
                "3", "5", "1");
        }

        [Test]
        public async Task SingleVectorSearchWithFilter()
        {
            await using SearchResources resources = await SearchResources.CreateWithHotelsIndexAsync(this);

            var vectorizedResult = VectorSearchEmbeddings.SearchVectorizeDescription; // "Top hotels in town"

            SearchResults<Hotel> response = await resources.GetSearchClient().SearchAsync<Hotel>(
                    new SearchOptions
                    {
                        VectorSearch = new()
                        {
                            Queries = { new VectorizedQuery(vectorizedResult) { KNearestNeighborsCount = 3, Fields = { "descriptionVector" } } }
                        },
                        Filter = "category eq 'Budget'",
                        Select = { "hotelId", "hotelName", "category" }
                    });

            await AssertKeysEqual(
                response,
                h => h.Document.HotelId,
                "3", "5", "4");
        }

        [Test]
        public async Task SimpleHybridSearch()
        {
            await using SearchResources resources = await SearchResources.CreateWithHotelsIndexAsync(this);

            var vectorizedResult = VectorSearchEmbeddings.SearchVectorizeDescription; // "Top hotels in town"

            SearchResults<Hotel> response = await resources.GetSearchClient().SearchAsync<Hotel>(
                    "Top hotels in town",
                    new SearchOptions
                    {
                        VectorSearch = new()
                        {
                            Queries = { new VectorizedQuery(vectorizedResult) { KNearestNeighborsCount = 3, Fields = { "descriptionVector" } } }
                        },
                        Select = { "hotelId", "hotelName" },
                    });

            await AssertKeysEqual(
                response,
                h => h.Document.HotelId,
                "3", "1", "2", "10", "4", "5", "9");
        }

        [Test]
        [ServiceVersion(Min = SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
        [PlaybackOnly("The availability of Semantic Search is limited to specific regions, as indicated in the list provided here: https://azure.microsoft.com/explore/global-infrastructure/products-by-region/?products=search. Due to this limitation, the deployment of resources for weekly test pipeline for setting the \"semanticSearch\": \"free\" fails in the UsGov and China cloud regions.")]
        public async Task SemanticHybridSearch()
        {
            await using SearchResources resources = await SearchResources.CreateWithHotelsIndexAsync(this);

            var vectorizedResult = VectorSearchEmbeddings.SearchVectorizeDescription; // "Top hotels in town"

            SearchResults<Hotel> response = await resources.GetSearchClient().SearchAsync<Hotel>(
                    "Is there any hotel located on the main commercial artery of the city in the heart of New York?",
                    new SearchOptions
                    {
                        VectorSearch = new()
                        {
                            Queries = { new VectorizedQuery(vectorizedResult) { KNearestNeighborsCount = 3, Fields = { "descriptionVector" } } }
                        },
                        SemanticSearch = new()
                        {
                            SemanticConfigurationName = "my-semantic-config",
                            QueryCaption = new(QueryCaptionType.Extractive),
                            QueryAnswer = new(QueryAnswerType.Extractive),
                            MaxWait = TimeSpan.FromMilliseconds(1000),
                            ErrorMode = SemanticErrorMode.Partial
                        },
                        QueryType = SearchQueryType.Semantic,
                        Select = { "hotelId", "hotelName", "description", "category" },
                        QueryLanguage = QueryLanguage.EnUs
                    });

            Assert.NotNull(response.SemanticSearch.Answers);
            Assert.AreEqual(1, response.SemanticSearch.Answers.Count);
            Assert.AreEqual("9", response.SemanticSearch.Answers[0].Key);
            Assert.NotNull(response.SemanticSearch.Answers[0].Highlights);
            Assert.NotNull(response.SemanticSearch.Answers[0].Text);

            await foreach (SearchResult<Hotel> result in response.GetResultsAsync())
            {
                Hotel doc = result.Document;

                Assert.NotNull(result.SemanticSearch.Captions);

                var caption = result.SemanticSearch.Captions.FirstOrDefault();
                Assert.NotNull(caption.Highlights, "Caption highlight is null");
                Assert.NotNull(caption.Text, "Caption text is null");
            }

            await AssertKeysEqual(
                response,
                h => h.Document.HotelId,
                "9", "3", "2", "5", "10", "1", "4");
        }

        [Test]
        public async Task UpdateExistingIndexToAddVectorFields()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new SearchableField("name") { IsFilterable = true, IsSortable = false },
                },
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            await indexClient.CreateIndexAsync(index);

            // Upload data
            SearchDocument document = new SearchDocument
            {
                ["id"] = "1",
                ["name"] = "Countryside Hotel"
            };

            SearchClient searchClient = resources.GetSearchClient();
            await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(new[] { document }));
            await resources.WaitForIndexingAsync();

            // Get the document
            Response<SearchDocument> response = await searchClient.GetDocumentAsync<SearchDocument>((string)document["id"]);
            Assert.AreEqual(document["id"], response.Value["id"]);
            Assert.AreEqual(document["name"], response.Value["name"]);

            // Update created index to add vector field

            // Get created index
            SearchIndex createdIndex = await indexClient.GetIndexAsync(indexName);

            // Add vector
            var vectorField = new VectorSearchField("descriptionVector", 1536, "my-vector-profile");
            createdIndex.Fields.Add(vectorField);

            createdIndex.VectorSearch = new()
            {
                Profiles =
                    {
                        new VectorSearchProfile("my-vector-profile", "my-hsnw-vector-config")
                    },
                Algorithms =
                    {
                        new HnswAlgorithmConfiguration("my-hsnw-vector-config")
                    }
            };

            // Update index
            SearchIndex updatedIndex = await indexClient.CreateOrUpdateIndexAsync(createdIndex);
            Assert.AreEqual(indexName, updatedIndex.Name);
            Assert.AreEqual(3, updatedIndex.Fields.Count);

            // Update document to add vector field's data

            // Get the dcoument
            SearchDocument resultDoc = await searchClient.GetDocumentAsync<SearchDocument>((string)document["id"]);

            // Update document to add vector field data
            resultDoc.Add("descriptionVector", VectorSearchEmbeddings.DefaultVectorizeDescription);

            await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Merge(new[] { resultDoc }));
            await resources.WaitForIndexingAsync();

            // Get the document
            response = await searchClient.GetDocumentAsync<SearchDocument>((string)document["id"]);
            Assert.AreEqual(document["id"], response.Value["id"]);
            Assert.AreEqual(document["name"], response.Value["name"]);
            Assert.IsNotNull(response.Value["descriptionVector"]);

            Assert.AreEqual(updatedIndex.Name, createdIndex.Name);
        }

        [Test]
        public async Task UpdatingVectorProfileNameThrows()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields = new FieldBuilder().Build(typeof(Model)),
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("my-vector-profile", "my-hsnw-vector-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("my-hsnw-vector-config")
                    }
                },
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            SearchIndex createdIndex = await indexClient.CreateIndexAsync(index);

            createdIndex.VectorSearch.Profiles[0].Name = "updating-vector-profile-name";

            // Update index
            RequestFailedException ex = await CatchAsync<RequestFailedException>(
                async () => await indexClient.CreateOrUpdateIndexAsync(createdIndex));
            Assert.AreEqual(400, ex.Status);
            Assert.AreEqual("InvalidRequestParameter", ex.ErrorCode);
        }

        [Test]
        [PlaybackOnly("The availability of Semantic Search is limited to specific regions, as indicated in the list provided here: https://azure.microsoft.com/explore/global-infrastructure/products-by-region/?products=search. Due to this limitation, the deployment of resources for weekly test pipeline for setting the \"semanticSearch\": \"free\" fails in the UsGov and China cloud regions.")]
        public async Task CanContinueWithNextPage()
        {
            const int size = 150;

            await using SearchResources resources = await SearchResources.CreateLargeHotelsIndexAsync(this, size, true);
            SearchClient client = resources.GetQueryClient();

            ReadOnlyMemory<float> vectorizedResult = VectorSearchEmbeddings.DefaultVectorizeDescription;
            SearchResults<SearchDocument> response = await client.SearchAsync<SearchDocument>("Suggest some hotels",
                    new SearchOptions
                    {
                        VectorSearch = new()
                        {
                            Queries = { new VectorizedQuery(vectorizedResult) { KNearestNeighborsCount = 50, Fields = { "descriptionVector" } } }
                        },
                        SemanticSearch = new()
                        {
                            SemanticConfigurationName = "my-semantic-config",
                            QueryCaption = new(QueryCaptionType.Extractive),
                            QueryAnswer = new(QueryAnswerType.Extractive)
                        },
                        QueryType = SearchQueryType.Semantic,
                        Select = new[] { "hotelId" }
                    });

            int totalDocsCount = 0;
            int pageCount = 0;

            await foreach (Page<SearchResult<SearchDocument>> page in response.GetResultsAsync().AsPages())
            {
                pageCount++;
                int docsPerPageCount = 0;
                foreach (SearchResult<SearchDocument> result in page.Values)
                {
                    docsPerPageCount++;
                    totalDocsCount++;
                }
                Assert.LessOrEqual(docsPerPageCount, 50);
            }

            Assert.LessOrEqual(totalDocsCount, 150);
            Assert.GreaterOrEqual(pageCount, 2);
        }

        [Test]
        [ServiceVersion(Min = SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
        public async Task VectorFieldNotStoredNotHiddenThrows()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new VectorSearchField("descriptionVector", 1536, "test-profile") { IsHidden = false, IsStored = false },
                },
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("test-profile", "test-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("test-config")
                    }
                }
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            RequestFailedException ex = await CatchAsync<RequestFailedException>(
               async () => await indexClient.CreateIndexAsync(index));

            Assert.AreEqual(400, ex.Status);
            Assert.AreEqual("InvalidRequestParameter", ex.ErrorCode);
        }

        [Test]
        [ServiceVersion(Min = SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
        public async Task VectorFieldStoredNotHidden()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new VectorSearchField("descriptionVector", 1536, "test-profile") { IsHidden = false, IsStored = true },
                },
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("test-profile", "test-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("test-config")
                    }
                }
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            SearchIndex createdIndex = await indexClient.CreateIndexAsync(index);
            Assert.AreEqual(indexName, createdIndex.Name);
            Assert.IsTrue(createdIndex.Fields[1].IsStored);
            Assert.IsFalse(createdIndex.Fields[1].IsHidden);
        }

        [Test]
        [ServiceVersion(Min = SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
        public async Task VectorFieldStoredAndHidden()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new VectorSearchField("descriptionVector", 1536, "test-profile") { IsHidden = true, IsStored = true },
                },
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("test-profile", "test-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("test-config")
                    }
                }
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            SearchIndex createdIndex = await indexClient.CreateIndexAsync(index);
            Assert.AreEqual(indexName, createdIndex.Name);
            Assert.IsTrue(createdIndex.Fields[1].IsStored);
            Assert.IsTrue(createdIndex.Fields[1].IsHidden);
        }

        [Test]
        [ServiceVersion(Min = SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
        public async Task CannotUpdateIsStoredAfterIndexCreation()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new VectorSearchField("descriptionVector", 1536, "test-profile") { IsHidden = true, IsStored = false },
                },
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("test-profile", "test-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("test-config")
                    }
                }
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            SearchIndex createdIndex = await indexClient.CreateIndexAsync(index);
            Assert.AreEqual(indexName, createdIndex.Name);
            Assert.IsFalse(createdIndex.Fields[1].IsStored);

            createdIndex.Fields[1].IsStored = true;

            // Update index
            RequestFailedException ex = await CatchAsync<RequestFailedException>(
                async () => await indexClient.CreateOrUpdateIndexAsync(createdIndex));
            Assert.AreEqual(400, ex.Status);
            Assert.AreEqual("OperationNotAllowed", ex.ErrorCode);
        }

        [Test]
        [ServiceVersion(Min = SearchClientOptions.ServiceVersion.V2024_03_01_Preview)]
        public async Task CanUpdateIsHiddenAfterIndexCreation()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new VectorSearchField("descriptionVector", 1536, "test-profile") { IsHidden = true, IsStored = true },
                },
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("test-profile", "test-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("test-config")
                    }
                }
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            SearchIndex createdIndex = await indexClient.CreateIndexAsync(index);
            Assert.AreEqual(indexName, createdIndex.Name);
            Assert.IsTrue(createdIndex.Fields[1].IsHidden);

            createdIndex.Fields[1].IsHidden = false;
            SearchIndex updatedIndex = await indexClient.CreateOrUpdateIndexAsync(createdIndex);
            Assert.AreEqual(indexName, createdIndex.Name);
            Assert.IsFalse(createdIndex.Fields[1].IsHidden);
        }

        [Test]
        public async Task CreateIndexUsingFieldBuilder()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // Create Index
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields = new FieldBuilder().Build(typeof(Model)),
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("my-vector-profile", "my-hsnw-vector-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("my-hsnw-vector-config")
                    }
                },
            };

            SearchIndexClient indexClient = resources.GetIndexClient();
            await indexClient.CreateIndexAsync(index);

            SearchIndex createdIndex = await indexClient.GetIndexAsync(indexName);
            Assert.AreEqual(indexName, createdIndex.Name);
            Assert.AreEqual(index.Fields.Count, createdIndex.Fields.Count);
        }

        private class Model
        {
            [SimpleField(IsKey = true, IsFilterable = true, IsSortable = true)]
            public string Id { get; set; }

            [SearchableField(IsFilterable = true, IsSortable = true)]
            public string Name { get; set; }

            [SearchableField(AnalyzerName = "en.microsoft")]
            public string Description { get; set; }

            [VectorSearchField(VectorSearchDimensions = 1536, VectorSearchProfileName = "my-vector-profile")]
            public IReadOnlyList<float> DescriptionVector { get; set; }
        }

        [Test]
        public async Task QueryFloatForHalfVectorField()
        {
            await using SearchResources resources = SearchResources.CreateWithNoIndexes(this);

            string indexName = Recording.Random.GetName();
            resources.IndexName = indexName;

            // ---------- Create Index---------------
            SearchIndex index = new SearchIndex(indexName)
            {
                Fields =
                {
                    new SimpleField("id", SearchFieldDataType.String) { IsKey = true },
                    new SearchableField("description") { IsFilterable = true },
                    new SearchField("descriptionVector", SearchFieldDataType.Collection(SearchFieldDataType.Half)) { VectorSearchDimensions = 1536, VectorSearchProfileName = "test-profile" },
                },
                VectorSearch = new()
                {
                    Profiles =
                    {
                        new VectorSearchProfile("test-profile", "test-config")
                    },
                    Algorithms =
                    {
                        new HnswAlgorithmConfiguration("test-config")
                    }
                }
            };

            SearchClientOptions options = new SearchClientOptions(SearchClientOptions.ServiceVersion.V2024_03_01_Preview);
            SearchIndexClient indexClient = resources.GetIndexClient(options);
            SearchIndex createdIndex = await indexClient.CreateIndexAsync(index);
            Assert.AreEqual(indexName, createdIndex.Name);

            // -----------------Upload data------------------------
            ReadOnlyMemory<float> vectorizeDescription = new float[] { -0.009445918f, -0.030015873f, 0.035673045f, 0.014168876f, -0.007413142f, -0.023268789f, 0.003706571f, 0.041347515f, 0.029254664f, -0.007486668f, -0.024358703f, 0.018044123f, -0.005380367f, -0.03529244f, 0.022230776f, 0.019653045f, -0.0483714f, -0.00083257287f, 0.016936911f, 0.011072137f, -0.020898659f, 0.015699945f, -0.0075904694f, -0.014220777f, 0.008481431f, 0.020915959f, -0.041520517f, -0.004528331f, 0.00765967f, 0.027022935f, 0.015803747f, -0.01050123f, 0.03129609f, -0.007006587f, -0.0100427745f, 0.019635744f, 0.015630744f, 0.021971272f, 0.023095787f, -0.0072141895f, 0.022628682f, 0.03466963f, -0.015379891f, -0.015423141f, 0.010864535f, -0.008131101f, -0.010605032f, 0.023545593f, 0.0123264035f, 0.010821285f, -0.018805334f, -0.008472781f, 0.0017862472f, -0.011418141f, -0.025119912f, 0.023113087f, 0.0029129237f, -0.033251014f, 0.014151576f, -0.018580431f, 0.013373066f, 0.026330927f, -0.0036871084f, 0.008485756f, -0.025154512f, 0.010544481f, -0.0022706531f, 0.01291461f, -0.0042839656f, -0.0022274028f, 0.02096786f, 0.009177764f, -0.023683995f, -0.00040655505f, -0.008533331f, 0.005458218f, -0.014333228f, -0.013762321f, -0.013442267f, 0.007806722f, -0.0008368979f, -0.01285406f, -0.009575669f, 0.02899516f, -0.00520304f, -0.0034254426f, -0.0009569181f, 7.832402E-05f, 0.02010285f, 0.049617015f, 0.021417666f, 0.0030102374f, 0.0070541627f, 0.013632569f, -0.007006587f, -0.0159681f, 0.013399016f, 0.0044504805f, -0.011184589f, 0.025552418f, 0.018857235f, -0.0049392115f, 0.026434729f, -0.0008006756f, 0.014904135f, -0.03454853f, 0.016919611f, -0.016746609f, 0.029202763f, 0.008529006f, -0.02646933f, -0.0063232286f, -0.026884533f, 0.008066225f, -0.023891596f, -0.009740021f, -0.03626125f, 0.004727284f, -0.032749306f, 0.01620165f, 0.022299977f, -0.0072098644f, 0.019877948f, 0.004072038f, -0.02899516f, -0.022144275f, -0.013165464f, -0.00039249862f, -0.014558131f, -0.011790097f, -0.012153401f, -0.0010293628f, -0.04186652f, -0.0048094597f, 0.035050236f, 0.012975161f, -0.026763432f, 0.0072877156f, -0.001752728f, -0.020587256f, 0.0030426753f, -0.016296802f, 0.005687446f, -0.00044007422f, 0.014160226f, -0.036745656f, -0.0042839656f, 0.03404682f, -0.014194827f, 0.029202763f, -0.015578844f, -0.0019484367f, 0.02816475f, -0.0076856203f, 0.0067341086f, -0.0046278075f, -0.006530831f, -0.000783916f, 0.0145321805f, 0.017326165f, -0.029150862f, 0.0029691495f, -0.005869098f, 0.0023679668f, 0.008689033f, -0.00765102f, -0.021348465f, -0.0013083287f, -0.014385128f, -0.00012373709f, 0.01684176f, 0.017871123f, -0.011677645f, 0.049444016f, 0.02657313f, 0.009999524f, -0.0020327752f, 0.00886636f, 0.0053414414f, -0.004305591f, 0.0066389577f, -0.0241857f, 0.050862633f, -0.013329816f, 0.0031378265f, 0.015137688f, -0.0031140386f, 0.017836522f, 0.0052722404f, 0.023666695f, -0.011963098f, 0.006115626f, -0.024635507f, -0.01932434f, -0.003505456f, 0.02249028f, -0.021988573f, 0.004584557f, 0.0089442115f, -0.011980399f, 0.0006925492f, 0.004645108f, 0.017516468f, 0.006288628f, 0.019168638f, -0.019168638f, 0.0017429966f, 0.021088962f, -0.012464805f, -0.011625744f, -0.029704468f, 0.0019268114f, 0.014618682f, 0.016469805f, -0.003838485f, -0.009895723f, -0.0075428938f, -0.0043293787f, -0.007828347f, 0.001448893f, 0.013130863f, 0.006755734f, 0.019462742f, -0.018199826f, -0.03788747f, 0.013926673f, 0.008926911f, 0.00035357315f, -0.009938974f, 0.023839695f, 0.0040136497f, -0.0051943897f, 0.011833347f, -0.03357972f, 0.023614794f, 0.0027096462f, -0.005306841f, 0.01365852f, -0.0077634715f, -0.016980162f, -0.015146338f, -0.0161757f, -0.013442267f, -0.019428141f, -0.001046663f, -0.009195064f, 0.022628682f, 0.0403095f, 0.014566781f, 0.0027399214f, -0.022594081f, -0.0008666326f, -0.002007906f, -0.033908423f, 0.006699508f, 0.010959686f, -0.017750021f, 0.021486867f, -0.013987224f, 0.003927149f, -0.018234426f, 0.008490081f, 0.002163608f, 0.0017397528f, -0.0046148323f, -0.0039833747f, -0.023251489f, 0.0063837795f, -0.004182327f, -0.024254901f, -0.017429966f, -0.023130387f, 0.020431554f, -0.03688406f, 0.013848822f, 0.0021463078f, -0.007888898f, -0.03357972f, 0.007849973f, -0.00029950996f, 0.0063102534f, -0.046952784f, 0.0320054f, 0.0241857f, -0.0072012143f, -2.9481324E-05f, -0.003135664f, -0.026330927f, -0.011392192f, 0.01927244f, 0.0050905882f, 0.005397667f, -0.018130625f, -0.006349179f, -0.0242203f, 0.026019523f, 0.016608207f, -0.019601144f, 0.027524643f, -0.0067341086f, 0.016391953f, 0.009195064f, 0.01127109f, -0.005959924f, -0.017352115f, 0.009419967f, 0.01766352f, 0.017274264f, 0.0050473376f, -0.008221927f, 0.0025561068f, -0.00682061f, 0.008139751f, -0.0011861459f, -0.020587256f, -0.025483217f, -0.019774146f, 0.012707008f, -0.05307706f, 0.0015353941f, 0.027022935f, 0.018199826f, 0.0033065036f, 0.0120669f, 0.0048051346f, -0.032645505f, 0.0006665989f, 0.018857235f, -0.020033648f, -0.011919848f, 0.051969845f, 0.029462267f, 0.02250758f, 0.0036092573f, -0.045464966f, -0.0032740657f, -0.017957624f, 0.0069892867f, 0.008468456f, -0.0013321165f, 0.007560194f, -0.022732483f, 0.009774622f, 0.01285406f, 0.014142926f, -0.007183914f, -0.025742719f, 0.010985636f, 0.022126975f, 0.0063232286f, 0.0055101183f, -0.0040547377f, -0.01283676f, 0.016270852f, 0.00322649f, -0.016417904f, 0.0011958773f, -0.004536981f, -0.04158972f, -0.02333799f, -0.01524149f, -0.034323625f, -0.0058301724f, 0.01039743f, 0.013632569f, 0.023753196f, -0.038441077f, 0.0039898623f, 0.016668757f, 0.016971512f, -0.014350528f, -0.014341878f, 0.021763671f, -0.010293628f, -0.00072714966f, 0.028493455f, 0.018718833f, 0.0009163708f, 0.007404492f, -0.01928974f, 0.007573169f, 0.009506468f, -0.017602969f, -0.043042935f, 0.036745656f, -0.01683311f, -0.0019722246f, -0.02100246f, -0.017326165f, 0.00521169f, 0.010778034f, -0.029271964f, 0.003382192f, 0.00081256946f, 0.007871598f, 0.005730696f, 0.0036049322f, -0.0075818193f, 0.022005873f, -0.012559956f, 0.02562162f, -0.03463503f, 0.0077029206f, -0.03626125f, 0.016331403f, -0.040447902f, -0.023493692f, -0.039098486f, -0.005380367f, 0.0019873623f, 0.0319881f, -0.0022749782f, -0.0071190386f, 0.0009082613f, 0.0028285852f, 0.03934069f, -0.00724879f, -0.0054365927f, -0.0004089879f, 0.010302278f, 0.012499405f, 0.02505071f, -0.012499405f, 0.00073039345f, 0.008027299f, 0.0011569519f, 0.016236251f, -0.018666932f, -0.018078724f, -0.025189113f, -0.0043531666f, -0.00561392f, 0.01613245f, 0.005492818f, -0.040171098f, 0.03458313f, 0.0027766845f, 0.0009617838f, 0.014783034f, -0.0040136497f, -0.0161411f, -0.03795667f, -0.002240378f, -0.0007147151f, 0.028510755f, 0.020223951f, 0.029566068f, 0.032766607f, 0.01291461f, 0.00054765993f, -0.012655107f, 0.010103325f, -0.004722959f, 0.01284541f, -0.019895248f, 0.011340291f, -0.0029258989f, -0.0071060634f, 0.009947624f, -0.010700183f, 0.014757084f, 0.010310928f, 0.011582494f, -0.01286271f, 0.033458617f, -0.028735656f, 0.0029280614f, 0.018891836f, -0.0063318787f, 0.029669868f, -0.035569243f, -0.014073725f, 0.029116262f, 0.00603345f, 0.01681581f, -0.038406476f, 0.0069676614f, 0.029583368f, 0.013148163f, -0.026244426f, 0.000515222f, -0.011729546f, -0.006336204f, -0.007132014f, -0.0017332652f, -0.64827365f, -0.020691058f, -0.009990874f, -0.009861123f, -0.00967947f, -0.0051684394f, 0.012378304f, -0.04266233f, 0.084217444f, -0.0056744707f, 0.01856313f, -0.030292677f, -0.005453893f, 0.03460043f, -0.018891836f, 0.032057296f, -0.013442267f, -0.0029604994f, 0.030725181f, -0.0049478617f, -0.005955599f, 0.0047402587f, -0.0056614955f, -0.028441554f, -0.0022274028f, 0.014004524f, 0.004043925f, -0.017767321f, -0.006898461f, -0.010103325f, 0.005077613f, 0.02567352f, 0.0029691495f, -0.0056441952f, -0.036538053f, -0.0072271647f, 0.022922784f, -0.0014770059f, 0.018995635f, 0.009869773f, 0.013009761f, -0.0135633685f, -0.072314896f, 0.00058171974f, 0.0052765654f, -0.013900722f, -0.026106024f, 0.023182288f, 0.0015689132f, -0.0051814145f, -0.009835172f, 0.02252488f, -0.008550631f, 0.023770496f, 0.008909611f, 0.015518293f, 0.0015808071f, 0.0067470837f, -0.02020665f, -0.007573169f, -0.01291461f, -0.018390128f, -0.0010536913f, 0.007884573f, 0.021348465f, -0.009169114f, 0.034929134f, -0.011322991f, -0.016193f, -0.013269265f, -0.007067138f, -0.002117114f, 0.010034124f, 0.024029998f, 0.01703206f, 0.0094891675f, 0.007932149f, 0.009731371f, 0.00010380129f, 0.003468693f, 0.019860648f, 0.018372828f, 0.027092136f, 0.023926197f, 0.012490755f, -0.010812635f, 0.014082375f, -0.007720221f, 0.21673709f, 0.0068076346f, 0.0021409015f, 0.16552846f, -0.013416316f, -0.018286327f, 0.022005873f, 0.024600906f, -0.01127109f, 0.0028739981f, 0.0040742005f, 0.0027723594f, -0.012430204f, 0.027645744f, 0.022715183f, 0.033406716f, 0.026832633f, -0.011418141f, 0.023130387f, -0.0063837795f, -0.010025474f, 0.01611515f, -0.007841323f, -0.035015635f, 3.2049324E-05f, 0.019601144f, 0.034271725f, -0.031849697f, 0.0007433686f, -0.02904706f, -0.0014045612f, -0.013502818f, 0.012412904f, -0.023268789f, 0.01851123f, -0.0002438249f, -0.009073962f, 0.007867273f, -0.036849458f, 0.011824697f, 0.010916436f, -0.0070411875f, 0.027974447f, -0.03541354f, -0.0067946594f, 0.011028887f, -0.018372828f, -0.012378304f, 0.005856123f, 0.0049305614f, 0.010847235f, 0.006336204f, 0.024029998f, 0.019497342f, -0.030898184f, 0.011963098f, 0.0042839656f, 0.027836045f, 0.031797796f, 0.029150862f, 0.008883661f, -0.0240992f, 0.0402057f, -0.013442267f, 0.015604794f, -0.023234189f, 0.008265178f, -0.010553131f, 0.002380942f, -0.0067903344f, 0.007015237f, 0.0024847435f, 0.012508055f, 0.016340053f, 0.021088962f, 0.0018597731f, -0.0008931236f, -0.0026534204f, -0.011418141f, 0.0013504981f, 0.04262773f, 0.0037779345f, 0.014065075f, 0.011470042f, -0.0026988336f, 0.0016067575f, -0.00062118587f, -4.1527273E-05f, 0.023043886f, -0.026711531f, -0.0006038856f, -0.021902071f, -0.0024285177f, -0.023701295f, -0.011859298f, -0.009264265f, -0.004128264f, 0.018113324f, 0.0040201372f, -0.017784622f, 0.013312516f, 0.014592731f, -0.0021203577f, 0.0014597056f, -0.008472781f, -0.036572654f, -0.011971748f, 0.0070282123f, 0.0160892f, -0.006029125f, -0.0037714469f, 0.026417429f, 0.0077807717f, 0.003090251f, 0.01127974f, 0.00885771f, -0.010786684f, 0.00842088f, 0.005717721f, 0.0055274186f, 0.008442505f, 0.009065312f, -0.018874535f, -0.006894136f, -0.019774146f, -0.0013991549f, 0.014238077f, -0.0123264035f, -0.0029604994f, 0.0058431476f, -0.008745259f, 0.0055879694f, 0.0074477424f, 0.022715183f, -0.0013591482f, 0.014653282f, -0.006046425f, 0.009151814f, -0.015933499f, 0.012369654f, -0.022230776f, -0.016616857f, -0.0051251887f, -0.0061415765f, 0.010233077f, -0.019756846f, -0.019774146f, -0.015129038f, 0.02498151f, -0.0050473376f, 0.009246965f, 0.006180502f, 0.007988375f, 0.022144275f, 0.019722246f, 0.0050949133f, 0.0074520675f, -0.01928974f, -0.0046105073f, -0.013027062f, -0.0087322835f, 0.012611857f, -0.01051853f, 0.022213476f, 0.009091263f, -0.018632332f, 0.010146576f, -0.008256528f, -0.01930704f, -0.02984287f, -0.018666932f, 0.01844203f, -0.04657218f, 0.021417666f, -0.03127879f, -0.039790496f, 0.02731704f, -0.011677645f, -0.09016872f, 0.028060948f, 0.0043531666f, -0.008304103f, -0.028891359f, 0.014696533f, 0.0028134475f, 0.016495755f, 0.0240992f, -0.004327216f, 0.013364416f, 0.025535118f, 0.011504643f, 0.008468456f, 0.0077807717f, -0.003533569f, 0.0026036825f, -0.009004761f, -0.0085938815f, 0.0021019762f, 0.030742481f, 0.012153401f, -0.016028648f, 0.01047528f, -0.009082613f, -0.006708158f, -0.01691096f, -0.024843108f, 0.011694945f, 0.017516468f, -0.0038428102f, 0.008433855f, -0.012767559f, 0.018320927f, 0.023043886f, 0.0033194788f, -0.012447504f, 0.007914849f, 0.0027031587f, -0.002417705f, 0.0064962306f, 0.0039422866f, -0.030344578f, 0.01609785f, 0.0028999485f, 0.0146705825f, -0.018822635f, -0.021538768f, -0.00040628473f, 0.020518055f, 0.011504643f, 0.018217126f, 0.0056744707f, 0.00042358495f, -0.002244703f, -0.0067427587f, 0.004223415f, 0.007547219f, -0.001970062f, 0.023268789f, 0.010959686f, -0.005882073f, 0.0005136001f, -0.008827435f, 0.02020665f, -0.018857235f, -0.006708158f, -0.024843108f, -0.014999286f, -0.0007887817f, 0.003693596f, 0.001736509f, 0.0024977184f, -0.009030712f, 0.023735896f, -0.01595945f, -0.013243315f, 0.0044504805f, 0.013537418f, -0.018649632f, 0.014341878f, -0.017750021f, 0.028233951f, -0.030898184f, -0.016279502f, -0.011989049f, -0.01292326f, 0.025915721f, 0.009774622f, 0.022715183f, 0.012516705f, -0.04677978f, 0.01050123f, -0.013243315f, -0.01844203f, 0.004597532f, -0.026313627f, 0.016642807f, 0.0053457664f, 0.017300215f, 0.009835172f, 0.0483368f, -0.023476392f, -0.005004087f, 0.013087613f, -0.0069763116f, 0.026261726f, -0.03070788f, -0.009073962f, -0.014973336f, 0.0075126183f, 0.020656457f, -0.023441792f, -0.030379178f, 0.0053457664f, -0.0072704153f, -0.01527609f, 0.0018997799f, -0.0020446691f, 0.007958099f, -0.0017040712f, -0.009117213f, -0.00043034286f, -0.017905723f, -0.0030686257f, 0.0031010634f, 0.030846283f, 0.008714983f, -0.0075255935f, 0.02082946f, -0.002445818f, 0.00039574242f, 0.0025388065f, -0.009964923f, -0.029150862f, -0.015051187f, 0.021556068f, 0.024635507f, 0.008048925f, 0.0028047974f, 0.016443854f, 0.012655107f, -0.0013677982f, 0.012663757f, -0.0012488592f, 0.0107347835f, 0.026971035f, -0.004536981f, 0.004121776f, 0.015864298f, -0.004649433f, -0.0033605667f, 0.02565622f, -0.005484168f, 0.025742719f, -0.0011342453f, 0.0043661417f, 0.022126975f, -0.0038190226f, -0.0068076346f, -0.008628482f, -0.020639157f, 0.013874772f, -0.0095583685f, 0.02494691f, 0.010172526f, -0.020293152f, -0.041797318f, 0.007802397f, -0.022092374f, 0.005159789f, -0.013727721f, 0.004895961f, -0.001613245f, -0.0042515276f, -0.02103706f, 0.007261765f, 0.008706333f, -0.0036265575f, -0.0018446355f, 0.011054837f, -0.010276328f, 0.0023895921f, 0.01286271f, -0.014540831f, 0.004011487f, 0.0065524564f, -0.008537656f, 0.019030236f, 0.00482676f, 0.017334815f, -0.016244901f, 0.017135862f, -0.024773907f, -0.002417705f, 0.0033605667f, 0.00044548055f, 0.014713833f, 0.010795334f, 0.016936911f, -0.0081786765f, 3.5377587E-05f, 0.0048570354f, -0.024497105f, -0.013796922f, -0.0061372514f, 0.0063708043f, -0.006587057f, -0.0024198676f, -0.006223752f, -0.007871598f, 0.0022101025f, 0.005700421f, 0.012802159f, -0.009731371f, 0.0058258474f, 0.029202763f, -0.007166614f, -0.009298866f, -0.013900722f, 0.016487105f, 0.029098962f, 0.017499167f, -0.025241014f, 0.039721295f, 0.02010285f, -0.009463218f, 0.0023917546f, -0.01534529f, -0.009186414f, 0.018891836f, -0.009056662f, -0.017222364f, 0.018424729f, 0.0241511f, -0.016253551f, 0.058993734f, 0.0049219113f, -0.01937624f, 0.011409491f, -0.022905484f, 0.0012412905f, 0.028891359f, -0.013528768f, -0.009324816f, 0.013390366f, 0.13639489f, -0.0242203f, 0.0321957f, -0.0070930882f, 0.0070455126f, -0.011504643f, -0.022801684f, 0.0077115707f, -0.005319816f, 0.003111876f, 0.0044634556f, 0.0074607176f, -0.012646457f, 0.005683121f, 0.014333228f, -0.029704468f, 0.019082136f, 0.004731609f, -0.00040304093f, 0.013805572f, -0.015068487f, 0.0010012499f, -0.006189152f, -0.010648282f, 0.009740021f, 0.024289502f, 0.021538768f, -0.024825808f, -0.0021841521f, 0.022248076f, -0.008074875f, 0.035673045f, -0.0013775296f, 0.0052765654f, -0.020898659f, -0.00766832f, -0.0057523213f, -0.00076769706f, 0.013520118f, -0.015673995f, 0.015942149f, 0.004173677f, 0.02731704f, 0.0045586065f, 0.0037930722f, -0.0017700283f, -0.0070368624f, -0.008892311f, 0.021331165f, -0.003706571f, 0.019116737f, 0.035084836f, -0.028649155f, 0.01700611f, -0.00563987f, -0.008667408f, -0.00885771f, -0.04158972f, -0.00043169444f, 0.0004292616f, -0.0077115707f, -0.00019462741f, -0.021953972f, -0.00140348f, -0.00966217f, 0.002159283f, -0.04594937f, -0.011963098f, -0.003103226f, -0.022594081f, 0.011037537f, -0.043492742f, -0.0120669f, 0.028510755f, 0.011089438f, -0.00015570193f, 0.007728871f, 0.01685041f, -0.01364122f, -0.020691058f, 0.0015775634f, 0.039928894f, 0.05238505f, -0.000825004f, -0.0005487412f, -0.011132688f, 0.004485081f, -0.026901834f, -0.009964923f, 0.032541703f, -0.010155226f, 0.015630744f, -0.010613682f, -0.009800572f, 0.039790496f, -0.008425205f, 0.035603844f, -0.008598207f, -0.013918023f, 0.017222364f, -0.0040504127f, -0.029341165f, -0.011046187f, 0.002171177f, -0.00024747418f, 0.029566068f, 0.0030426753f, -4.954383E-06f, -0.013433617f, -0.023995398f, -0.010388779f, 0.00023936469f, -0.019255139f, -0.018684233f, 0.024722008f, -0.0030707882f, 0.030967385f, -0.030984685f, 0.012473455f, -0.0021365765f, -0.0070238872f, -0.011919848f, -0.008810135f, 0.008952862f, -0.0012650782f, 0.012698358f, -0.0032524404f, 0.0028545354f, 0.0028675105f, -0.019601144f, -0.013009761f, 0.014212127f, -0.0020295314f, 0.020448854f, -0.0018619356f, -0.015500993f, -0.00032978534f, 0.009480517f, 0.0039249863f, -0.008356004f, 0.01679851f, -0.0010131438f, 0.018597731f, 0.01207555f, -0.0058344975f, 0.0028523728f, -0.03295691f, 0.01597675f, -0.010605032f, 0.022905484f, 0.06331879f, 0.031105787f, -0.039652094f, 0.01845933f, 0.0067816842f, 0.003922824f, -0.012482105f, 0.0027009961f, 0.03288771f, -0.023562893f, 0.009982224f, -0.0063967546f, 0.0032762282f, -0.025189113f, -0.0005876667f, -0.01699746f, 0.012611857f, -0.0051814145f, 0.004085013f, -0.02255948f, -0.0005487412f, 0.01686771f, 0.0034816682f, -0.0062453775f, 0.020033648f, -0.0009390773f, -0.014774384f, -0.023735896f, -0.014627332f, -0.0045542815f, -0.029462267f, 0.0037779345f, 0.0025431316f, -0.029237363f, 0.02982557f, 0.005985874f, -0.02894326f, -0.012508055f, 0.0023398541f, 0.018805334f, -0.00071687764f, -0.01768947f, -0.009307516f, -0.0007974318f, 0.024808507f, 0.014073725f, -0.04774859f, -0.0006271328f, 0.004753234f, 0.0032113525f, 0.005696096f, -0.00026815332f, -0.01051853f, 0.0069200858f, 0.004480756f, 0.021832872f, -0.024860408f, -0.03622665f, -0.0042363903f, 0.0034016548f, 0.009039362f, 0.0048051346f, 0.0077634715f, 0.0020511567f, 0.042420126f, -0.017161813f, -0.012594556f, 0.03143449f, -0.012637807f, 0.005739346f, 0.0034622054f, 0.027490042f, 0.011098088f, -0.0038730856f, 0.001986281f, -0.030811682f, 0.0096275695f, 0.03539624f, -0.024722008f, -0.014783034f, -0.010985636f, -0.0049262363f, 0.03465233f, -0.013104913f, -0.006275653f, -0.019168638f, 0.017179113f, 0.014212127f, 0.01528474f, 0.0012704845f, -0.01601135f, 0.00443318f, -0.0066389577f, -0.02250758f, 0.020708358f, 0.015059837f, 0.010068725f, 0.013373066f, -0.011072137f, -0.0025928698f, 0.011426792f, -0.00484406f, -0.048406f, 0.042939134f, -0.008217602f, -0.032039996f, -0.00362007f, 0.0077115707f, -0.022784384f, -0.0014099675f, -0.021365765f, -0.009610269f, 0.015673995f, -0.0013667169f, 0.00762507f, 0.009307516f, -0.024445204f, 0.006098326f, 0.021279264f, -0.0029431991f, -0.0020533192f, -0.020535355f, -0.0113575915f, -0.0001406994f, 0.035638444f, -0.013027062f, -0.0067816842f, 0.0028999485f, -0.0055749943f, 0.009099913f, 0.019635744f, 0.0045542815f, -0.00093529286f, -0.003784422f, -0.003377867f, 0.0019376241f, -0.019116737f, 0.005683121f, 0.016573606f, 0.008074875f, 0.022282677f, -0.0048656855f, 0.015924849f, -0.010224427f, 0.032610904f, 0.02249028f, -0.021556068f, -0.008273828f, 0.022991985f, 0.017239664f, -0.024410604f, -0.005553369f, 0.016625507f, -0.018891836f, -0.014160226f, -0.0109423855f, 0.018978335f, -0.0014661932f, 0.0037930722f, -0.000964487f, -0.00029248177f, 0.012975161f, -0.009411317f, -0.010622332f, 0.023407191f, 0.015397191f, -0.017352115f, -0.0013386041f, -0.01761162f, 7.649939E-05f, -0.0018846422f, 0.01765487f, -0.012594556f, 0.016063249f, 0.0029950996f, 0.015483692f, -0.026227126f, 0.0011407329f, 0.009714071f, -0.01930704f, -0.008399255f, 0.004173677f, 0.009913024f, 0.0135633685f, 0.02570812f, -0.006405405f, -0.010293628f, -0.014592731f, -0.010319578f, 0.0020262876f, -0.022645982f, -0.0053544166f, 0.022005873f, 0.011712246f, -0.019012935f, 0.003739009f, 0.0016792021f, 0.0008217602f, -0.0036308826f, -0.0161411f, 0.0027615468f, -0.014981986f, 0.021590669f, 0.011617094f, -0.019255139f, 0.02577732f, 0.024064599f, -0.001567832f, 0.024791207f, -0.015622094f, -0.014748434f, -0.018199826f, 0.0042515276f, -0.008005675f, -0.007573169f, -8.433855E-05f, -0.0012045274f, 0.025984922f, 0.009740021f, -0.014990636f, -0.002906436f, -0.038095072f, -0.011617094f, -0.005877748f, 0.012681058f, -0.030292677f, -0.0021246825f, 0.039133087f, -0.023545593f, -0.043215938f, 0.0039249863f, 0.033372115f, 0.021988573f, -0.04598397f, -0.012663757f, 0.005389017f, -0.01448893f, -0.0049521867f, -0.0323687f, 0.009342116f, -0.012291803f, -0.0037498216f, 0.0020511567f, -0.016400604f, 0.007499643f, 0.01849393f, 0.012023649f, 0.0029583368f, 0.011521943f, 0.042385526f, -0.016019998f, 0.012802159f, 0.00403095f, 0.0027334339f, -0.0049262363f, 0.017300215f, 0.011573844f, 0.026071424f, -0.010207127f, -0.008836085f, -0.024877708f, 0.0070325374f, -0.0242203f, 0.034790732f, -0.018044123f, 0.0058215223f, 0.0001747592f, 0.024497105f, 0.0045499564f, -0.02652123f, 0.005553369f, 0.027213238f, 0.01288866f, -0.011054837f, -0.012784859f, 0.010933735f, -0.011132688f, 0.007746171f, 0.020742958f, 0.0049781366f, -0.018251726f, 0.004206115f, 0.0030361877f, -0.01124514f, -0.0060723755f, 0.0017808409f, 0.028770257f, 0.020068249f, 0.016080549f, -0.0065567815f, -0.009852473f, 0.016193f, -0.0036438578f, 0.026936434f, 0.0033173163f, 4.5176537E-05f, 0.014618682f, 0.012213952f, -0.01767217f, -0.006366479f, 0.0015894573f, -0.006297278f, -0.0082478775f, 0.0026101698f, 0.011089438f, 0.0319708f, 0.015077137f, 0.02167717f, -0.043215938f, 0.015734546f, 0.009757321f, -0.0002461226f, 0.0039206613f, 0.02169447f, -0.038510278f, -0.004822435f, 0.016651457f, 0.00963622f, 0.02252488f, -0.006522181f, 0.009324816f, -0.015777797f, 0.0109423855f, -0.0023376916f, -0.036157448f, 0.015094438f, 0.018269027f, 0.01847663f, -0.006535156f, 0.0026772083f, 0.009938974f, 0.021434966f, -0.019774146f, -0.00320919f, -0.0015224189f, -0.0006168608f, -0.0040612253f, -0.00422774f, 0.011729546f, 0.0028912984f, 0.034479327f, 0.006764384f, 0.018943734f, -0.018666932f, 0.0075385687f, 0.007374217f, -0.009835172f, 0.0047445837f, 0.013770971f, 0.010890486f, 0.0047402587f, 0.025587019f, 0.024704708f, -0.0162103f, 0.0031594518f, 0.008213277f, 0.0068681855f, -0.037368465f, -0.02986017f, -0.015639395f, 0.015950799f, 0.0120842f, 0.0031572892f, 0.0028588604f, -0.027524643f, 0.01616705f, -0.04273153f, 0.0034838307f, 0.023822395f, 0.0016381141f, 0.0006325391f, 0.02498151f, -0.0036092573f, -0.00966217f, 0.003948774f, -0.038198873f, 0.02738624f, 0.01599405f, 0.02567352f, 0.018839935f, 0.004848385f, 0.0052808905f, 0.002651258f, 0.0012856222f, 0.0109423855f, 0.011418141f, -0.031659395f, 0.0029777994f, 0.0012088525f };

            SearchDocument document = new SearchDocument
            {
                ["id"] = "1",
                ["description"] = "Best hotel in town if you like luxury hotels. They have an amazing infinity pool, a spa, and a really helpful concierge. The location is perfect -- right downtown, close to all the tourist attractions. We highly recommend this hotel.",
                ["descriptionVector"] = vectorizeDescription
            };

            SearchClient searchClient = resources.GetSearchClient(options);
            await searchClient.IndexDocumentsAsync(IndexDocumentsBatch.Upload(new[] { document }));
            await resources.WaitForIndexingAsync();

            // ------------Query the document-----------------------------
            // "Top hotels in town"
            ReadOnlyMemory<float> searchVectorizeDescription = new float[] { -0.011113605f, -0.01902812f, 0.047524072f, -0.004909588f, -0.02193134f, -0.0024247447f, -0.017521033f, 0.020359533f, 0.008362942f, -0.0029471396f, -0.009930126f, 0.023872986f, 0.0032314518f, -0.006559986f, -3.7381E-05f, 0.017169688f, -0.027478898f, 0.0033747638f, 0.017373098f, 0.0042461925f, -0.009306027f, 0.012500495f, -0.0072580534f, -0.017974084f, 0.0152372895f, 0.010521866f, -0.034838658f, 0.014386664f, 0.023614101f, 0.020082155f, 0.009440092f, -0.013702465f, 0.029531494f, -4.2112315E-05f, -0.021764915f, 0.011612886f, 0.0010904416f, 0.030400611f, -0.0018688332f, -0.00045536197f, 0.02710906f, 0.012824102f, -0.033063438f, -0.0021751046f, -0.00647215f, -0.0067633963f, -0.015144831f, -0.0032823044f, -0.003850929f, 0.024723612f, 0.0061346735f, -0.018149758f, 0.020341042f, -0.031066319f, -0.017400837f, 0.013711711f, 0.013841154f, -0.028458966f, 0.024427742f, -0.013656236f, -0.0042901104f, 0.03385859f, 0.024335282f, -0.023799019f, -0.004003487f, -0.016651917f, -0.007475333f, 0.009435469f, -0.01843638f, 0.023392199f, 0.016836835f, -0.013480563f, -0.020470485f, 0.0032337634f, 0.0155793885f, 0.011427967f, -0.0038255027f, -0.0055568027f, 0.0042762416f, -0.0011950362f, -0.022578556f, 0.00080439576f, 0.00058162666f, 0.027460406f, -0.007849793f, -0.006157788f, -0.016411522f, 0.0013614629f, 0.0066200844f, 0.021210158f, 0.006291854f, 0.0066663143f, 0.015098601f, 0.0049142106f, -0.0027113685f, -0.011187573f, 0.014321943f, -0.01877848f, 0.016466998f, 0.035116035f, 0.041088905f, 0.0062733623f, -0.0036082235f, 0.0051777195f, -0.011538918f, 0.005663131f, 0.016245095f, -0.0070130364f, 0.0041514216f, 0.0025333844f, -0.014710272f, -0.021949833f, -0.019693827f, 0.016642671f, -0.021395078f, -0.00067841995f, -0.045563933f, -0.0076602516f, -0.012065936f, 0.04186556f, 0.025740664f, -0.0032684356f, 0.018676775f, 0.007701858f, -0.0063935593f, 0.010900949f, -0.014081549f, -0.03594817f, 7.56288E-05f, -0.0202116f, -0.004877227f, 0.004551308f, -0.019638352f, 0.014922928f, 0.022079276f, 0.013452825f, -0.007230316f, 0.0034880263f, -0.0012458888f, 0.006430543f, -0.009745209f, -0.0009673552f, 0.004089012f, 0.0064952644f, 0.010078061f, -0.03187996f, 0.00884373f, 0.019379465f, -0.010900949f, 0.042420316f, -0.013942859f, 0.013526793f, 0.020895798f, -0.009370748f, -0.010373931f, -0.01941645f, -0.012472757f, -0.016309816f, 0.02538932f, 0.014959912f, -0.023503149f, -0.014220238f, -0.017262148f, 0.016827589f, 0.0028939755f, 0.0069252f, -0.017631985f, 0.0044426685f, -0.0069945445f, -0.014811977f, 0.025814632f, 0.007724973f, 0.008409171f, 0.05321956f, 0.017317623f, 0.017104967f, -0.0077388417f, -0.012213871f, -0.032989472f, 0.007253431f, -0.01757651f, -0.01355453f, 0.039905425f, -0.009985602f, 0.016642671f, 0.020156123f, 0.0031621074f, 0.008362942f, 0.020988258f, 0.0007292726f, 0.009735962f, 0.00022392483f, -0.033729147f, -0.0004244459f, -0.0065877237f, 0.0067079207f, -0.02152452f, 0.0028477458f, 0.007859039f, -0.03415446f, 0.0026119747f, 0.00070442416f, 0.0032314518f, 0.010355439f, 0.0029933692f, -0.02011914f, -0.008251991f, 0.016836835f, -0.014266467f, -0.02450171f, -0.024982497f, 0.00016859372f, 0.023799019f, 0.026276927f, -0.019841762f, 0.00884373f, -0.046377577f, 0.0038278142f, -0.0072488077f, 0.030511564f, 0.016457751f, 0.0010476792f, 0.03489413f, -0.011622132f, -0.013989089f, 0.020082155f, -0.012158396f, 0.0026397125f, -0.0021704817f, 0.00041982293f, 0.012472757f, -0.014460632f, 0.009278289f, -0.038204174f, 0.023096329f, 0.008423041f, 0.022449113f, 0.013794925f, -0.030567039f, -0.023262756f, -0.023669576f, -0.023928462f, -0.0030650252f, -0.014035319f, -0.005649262f, -0.017539525f, 0.03975749f, 0.014340434f, 0.004988178f, 0.010725277f, -0.021395078f, 0.0204335f, 0.020969765f, -0.001508242f, 0.022338163f, 0.025111942f, -0.0220238f, -0.011150589f, -0.02113619f, 0.010170521f, -0.04145874f, 0.0031875337f, 0.019397957f, -0.014987649f, 0.0052239494f, -0.0027760898f, -0.027719293f, -0.0041930284f, 0.003980372f, -0.018880185f, -0.010503374f, 0.0015810537f, 0.021709438f, -0.02840349f, 0.026055025f, 0.0195274f, -0.014802731f, -0.033063438f, 0.002169326f, -0.030234184f, 0.017724445f, -0.024483217f, 0.030141726f, -0.0010927531f, 0.0030003036f, -0.009033272f, -0.022892918f, -0.013009021f, -0.032342255f, 0.011520427f, -0.0029956808f, 0.009976356f, -0.02231967f, -0.01941645f, 0.0033747638f, 0.021450553f, 0.002815385f, -0.021247143f, 0.023188788f, -0.026776208f, 0.0052470644f, 0.022948394f, 0.0055984096f, 0.009185829f, -0.0126854135f, 0.010373931f, 0.009680486f, 0.003423305f, -0.008321336f, 0.015070863f, 0.0077989404f, -0.02720152f, 0.022005308f, -0.019379465f, -0.021358093f, -0.035226986f, -0.01464555f, 0.021284126f, -0.0141092865f, -0.01364699f, 0.02172793f, 0.003841683f, 0.0023057032f, 0.008154908f, 0.009782192f, -0.024982497f, 0.0041560447f, 0.021265635f, -0.020341042f, 0.016411522f, 0.038389094f, 0.02729398f, 0.0078914f, 0.02899523f, -0.026665257f, 0.01569034f, -0.0048125056f, 0.01921304f, -0.017225165f, 0.016217358f, 0.023170296f, -0.012824102f, 0.015255782f, 0.008191892f, 0.0035781742f, -0.011326262f, -0.014756502f, 0.008293598f, -0.0021658586f, 0.010189013f, 0.02440925f, -0.0067633963f, -0.0093984855f, 0.007466087f, -0.007734219f, -0.017511789f, -0.0019751615f, 0.016115652f, -0.026572797f, 0.0039965524f, -0.011372492f, -0.024353774f, 0.0071378564f, -0.029365068f, 0.023003869f, 0.027460406f, -0.042087466f, 0.005626147f, 0.013203185f, -0.004863358f, -0.0013695532f, -0.02699811f, 0.0052424413f, 0.0038278142f, -0.009532552f, 0.01593998f, 0.022301178f, -8.660834E-05f, 0.015727324f, -0.019360973f, -0.0072580534f, -0.0061485423f, -0.014987649f, -0.042494286f, 0.0055891634f, -0.008621828f, 0.009865405f, -0.008977796f, -0.010170521f, -0.0018595873f, -0.010207505f, -0.0077527105f, -0.00094539614f, 0.002038727f, 0.014793485f, -0.012361806f, 0.007831302f, -0.0063935593f, 0.028274048f, 0.0011100892f, 0.006079198f, -0.012093674f, -0.0008309778f, -0.026221452f, -0.0070592663f, -0.031824484f, -0.04282714f, -0.01809428f, -0.0019127514f, -0.007026905f, 0.018011067f, 7.5267635E-05f, -0.0035619938f, 0.0058804103f, -0.015320503f, 0.039794475f, -0.02093278f, -0.008436909f, -0.015394471f, -0.0030858286f, -0.017141951f, 0.021783406f, -0.024316791f, -0.0004230012f, 0.012250855f, -0.008173401f, 0.007734219f, -0.029328084f, -0.026332403f, -0.012703905f, -0.019268515f, 0.0047708987f, -0.01569034f, -0.0014539222f, -0.0315656f, 0.013258661f, -0.004542062f, -0.0017682838f, 0.00438257f, -0.014081549f, -0.028052146f, -0.0015648734f, -0.016356047f, -0.011538918f, 0.011353999f, 0.008950058f, 0.038204174f, 0.008931567f, 0.032064877f, 0.010549604f, -0.015117092f, -0.008686549f, -0.018972645f, -0.020082155f, -0.0054088677f, 0.010179766f, 0.024945514f, -0.00090610096f, -0.0018249151f, -0.015033879f, 0.027682308f, -0.0003591465f, 0.01659644f, -0.02709057f, 0.029106181f, -0.05277576f, 0.014405156f, 0.019360973f, 0.01369322f, -0.01046639f, -0.0049511944f, -0.0025911713f, -0.0028801067f, 0.003284616f, 0.021080716f, -0.019268515f, -0.0022039982f, 0.0018688332f, 0.019786285f, -0.024668137f, -0.013406596f, -0.010179766f, -0.0059728697f, -0.005774082f, -0.017974084f, -0.6083081f, -0.021154683f, 0.011095114f, 0.015033879f, -0.0062132636f, -0.032933995f, 0.008839107f, -0.026221452f, 0.0910539f, 0.016827589f, 0.02688716f, -0.023558624f, -0.012583708f, 0.02182039f, -0.014210992f, 0.037409026f, -0.009366125f, 0.008335204f, 0.01729913f, 0.007355136f, 0.007087004f, 0.0024247447f, -0.0024270562f, -0.021154683f, -0.021487538f, 0.021561505f, 0.00622251f, -0.03735355f, -0.023521641f, 0.008626451f, 0.016624179f, 0.01993422f, 0.02211626f, -0.00705002f, -0.026055025f, 0.0023669575f, 0.018343922f, -0.018538086f, 0.031140286f, 0.014756502f, 0.015431454f, 0.012842594f, -0.07677819f, 0.009948619f, -0.005995984f, -0.011788558f, -0.043603797f, 0.010225996f, -0.017114213f, 0.00452357f, -0.001958981f, 0.026850175f, 0.0019404892f, 0.023540134f, 0.021062225f, 0.0062317555f, 0.016947785f, 0.0029841233f, -0.034061998f, -0.023595609f, -0.02132111f, -0.0134713175f, 0.0064998874f, 0.0058526723f, -0.0033747638f, 0.040682085f, 0.03596666f, 0.0027067454f, -0.010789998f, -0.0141092865f, -0.013369612f, 0.004206897f, 0.016679654f, 0.019508908f, 0.014821223f, 0.03705768f, -0.000522106f, 0.020063665f, -0.017021753f, -0.033359308f, 0.033710655f, 0.0044958326f, 0.029790381f, 0.02261554f, -0.0010742613f, 0.006042214f, 0.009125731f, -0.019508908f, 0.23876685f, 0.0040266016f, -0.0061346735f, 0.2037063f, -0.0011326262f, 0.004900342f, 0.024279807f, 0.011594394f, -0.00053424126f, 0.009661995f, 0.004923457f, 0.0026165976f, -0.015459192f, 0.040682085f, 0.010142783f, 0.015967717f, 0.009458585f, 0.010318456f, 0.008002351f, 0.0017463247f, -0.021783406f, 0.018926416f, -0.009819176f, -0.041421756f, 0.003173665f, -0.018362414f, 0.024427742f, -0.020341042f, 0.020803338f, -0.027737785f, 0.012001215f, -0.016938541f, 0.029642446f, -0.024150364f, -0.0045281933f, -0.007045397f, 0.0014169385f, 0.017474804f, -0.018815463f, 0.0019612925f, 0.015819782f, -0.030641006f, 0.016235849f, -0.014987649f, 0.0046391445f, 0.0081225475f, -0.01021675f, -0.00818727f, 0.020747863f, -0.0058665415f, 0.002690565f, -0.0050806375f, 0.030825924f, 0.024446234f, -0.034043506f, 0.02529686f, 0.0046761283f, 0.013702465f, 0.05188815f, 0.015958471f, 0.015329748f, -0.017012509f, 0.0121953795f, 0.01707723f, 0.013360366f, 0.0062733623f, 0.0015810537f, -0.00047154233f, -0.0030719596f, -0.0054458515f, 0.005136113f, -0.009504814f, 0.014904436f, 0.020045172f, 0.007313529f, 0.0020052106f, 0.0071239877f, -0.0048263744f, -0.00463221f, 0.0025102694f, 0.011529672f, 0.0036845023f, 0.018898677f, 0.012528232f, 0.0057232296f, -0.02102524f, 0.030049266f, 0.002618909f, 0.02390997f, -0.025333842f, -0.014321943f, -0.016171128f, -0.020137632f, -0.008834484f, -0.03187996f, -0.048337713f, -0.0031875337f, 0.046377577f, -0.015829029f, -0.0110119f, 0.0048125056f, 0.045415998f, 0.007826678f, 0.009412355f, -0.013526793f, -0.046932332f, -0.010679047f, 0.01782615f, 0.026424862f, -0.017151197f, 0.0073736277f, 0.02341069f, 0.019397957f, 0.018464118f, -0.009000911f, -0.0003967081f, 0.012925807f, -0.012491249f, 0.007433726f, -0.016725885f, 0.00027549977f, 0.012962791f, -0.006139296f, -0.015856767f, -0.0152372895f, -0.0152372895f, 0.046340592f, 0.009444715f, 0.012306331f, 0.016124899f, -0.031121794f, 0.013378858f, 0.009745209f, 0.017696707f, 0.017770674f, -0.0030488449f, -0.003934142f, -0.008852976f, -0.027312472f, 0.025259875f, -0.017243655f, -0.010697539f, 0.0039549456f, -0.015283519f, -0.004338652f, -0.01001334f, -0.044787277f, 0.012408036f, 0.026757715f, 0.011733083f, 0.01802956f, -0.009486322f, -0.0013267907f, 0.030659497f, -0.0037376664f, 0.0082704825f, 0.019638352f, -0.011002654f, 0.016466998f, -0.016799852f, -0.007567792f, 0.013184694f, 0.0029563855f, -0.016605686f, -0.00672179f, -0.0013267907f, 0.029845856f, 0.0058526723f, -0.017752182f, -0.017493296f, -0.013092234f, 0.023392199f, -0.037224106f, 0.030234184f, -0.031898454f, -0.04375173f, 0.010734523f, -0.04094097f, -0.09734113f, 0.016836835f, -0.015329748f, -0.014377418f, -0.033026457f, 0.010669801f, 0.0014978404f, 0.025074957f, 0.04094097f, 0.0058434266f, 0.009976356f, 0.027275488f, 0.018538086f, 0.0045374394f, -0.006703298f, 0.00071193645f, -0.0072210697f, -0.009463208f, 0.011520427f, -0.015727324f, 0.0036405842f, 0.0012736266f, -0.008589467f, -0.011612886f, -0.00818727f, -0.0009945151f, -0.006828118f, -0.010669801f, 0.0141555155f, -0.0025611222f, 0.0049327025f, 0.009024026f, 0.00010871189f, 0.011446459f, 0.021543013f, 0.0022143999f, 0.0031875337f, 0.013489809f, 0.013240169f, 0.0021300307f, 0.008899205f, 0.0128333485f, -0.028015163f, 0.011520427f, 0.015209552f, 0.012269347f, -0.009065633f, -0.013748695f, 0.0001798622f, 0.01055885f, 0.007452218f, 0.037704896f, -0.0050945063f, 0.007979236f, 0.01239879f, 0.0048171286f, -0.0016076358f, 0.0037538467f, -0.0012135281f, 0.033988032f, 0.001874612f, -0.018066544f, -0.016624179f, 0.0064629037f, 0.0009985602f, -0.025019482f, -0.012001215f, -0.00759553f, -0.005002047f, -0.0071563483f, 0.010826982f, 0.010605079f, 0.014969158f, 0.0006639732f, 0.018787727f, -0.035300955f, -0.0076556285f, 0.018880185f, 0.017178934f, -0.013388104f, 0.0028408114f, -0.012269347f, 0.029790381f, -0.010503374f, -0.0029217133f, -0.015265027f, -0.017243655f, 0.005089883f, -0.006777265f, -0.003423305f, 0.023484657f, -0.017437821f, 0.005288671f, -0.016624179f, -0.016041685f, 0.014275713f, 0.0021323422f, 0.007627891f, 0.008016219f, 0.041791596f, 0.0029402052f, 0.035911184f, -0.031103302f, -0.014848961f, 0.016272834f, -0.010688293f, 0.045674887f, 0.00550595f, -0.007896023f, -0.0155331595f, 0.022800459f, 0.0049743094f, -0.035023578f, -0.016272834f, 0.00772035f, -0.008728156f, -0.022356654f, -0.013110726f, 0.0143034505f, 0.009597274f, 0.0013129218f, -0.01280561f, -0.009426224f, 0.009241305f, 0.0057001146f, 0.005995984f, 0.021062225f, 0.0078035635f, 0.01319394f, 0.0004992801f, 0.0016839147f, 0.0005472434f, 0.0038717324f, 0.008959305f, -0.0006177436f, -0.019360973f, 0.02729398f, 0.014321943f, 0.010521866f, 0.014488369f, 0.026221452f, 0.011908756f, 0.007868284f, 0.020951273f, -0.005233195f, 0.016180374f, 0.041310806f, -0.0015671848f, 0.0062456243f, 0.010484883f, -0.013480563f, 0.009689732f, 0.033655178f, 0.00055273314f, 0.016430015f, -0.02450171f, 0.025574237f, 0.008312089f, -0.011270787f, -0.015357487f, -0.020285565f, -0.033285342f, 0.004965063f, -0.006795757f, 0.013064496f, 0.011169082f, -0.018464118f, -0.023133311f, 0.009181207f, -0.004186094f, 0.0068789707f, -0.0211177f, 0.013582269f, 0.0076325135f, 0.02122865f, -0.0042901104f, 0.012426527f, -0.01080849f, -0.009661995f, -0.0028269426f, 0.0070176595f, -0.015218798f, 0.0072904145f, 0.019656843f, 0.007905268f, 0.00012337536f, 0.022375146f, -0.011640623f, 0.0033262225f, 0.029771889f, 0.015736569f, -0.034283902f, -0.0009922037f, -0.02659129f, -0.02093278f, -0.024779087f, 0.010642063f, 0.02699811f, 0.008113302f, 0.009292157f, -0.025814632f, -0.016531719f, -0.026313912f, 0.0038393717f, -0.009352257f, -0.01414627f, 0.0014539222f, 0.008090187f, 0.008265859f, -0.009985602f, 0.00809481f, -0.0027691554f, 0.011076622f, -0.011520427f, -0.0072996602f, 0.0075354315f, 0.018667528f, -0.04182858f, -0.0015197995f, 0.0121953795f, 0.013536039f, 0.038906865f, -0.005922017f, -0.033729147f, 0.017844642f, 0.03794529f, 0.017863134f, 0.013942859f, -0.016060177f, -0.0047223577f, 0.011381738f, -0.018353168f, -0.014839714f, 0.028958246f, 0.029827364f, -0.010780753f, 0.033655178f, -0.01369322f, -0.010549604f, 0.017724445f, -0.023133311f, -0.010688293f, 0.025629712f, -0.0044287997f, -0.017585754f, 0.009698979f, 0.15015388f, -0.015274273f, 0.0128333485f, -0.009865405f, 0.0024848431f, -0.015884504f, -0.0070777577f, -0.009754454f, -0.009828421f, -0.013360366f, -0.0027853358f, 0.00034874486f, -0.00647215f, 0.01584752f, -0.0015810537f, -0.014848961f, -0.010753014f, -0.009745209f, 0.013989089f, 0.014580829f, -0.014553091f, -0.003975749f, -0.0050621456f, 0.0058572954f, 0.03554135f, 0.008783632f, 0.021043733f, -0.016300572f, 0.017012509f, 0.015459192f, -0.0034256163f, 0.0019705384f, 0.00092632644f, 0.023558624f, -0.011816296f, 0.02163547f, 0.002047973f, 0.008090187f, -0.010004094f, -0.00094192894f, 0.00402429f, 0.015995456f, 0.012463511f, -0.000361458f, -0.0126854135f, -0.0066200844f, 0.0005775816f, -0.013868893f, 0.021746423f, 0.0186213f, 0.03659538f, 0.028255556f, -0.019656843f, 0.011418722f, -0.021968326f, -0.009403109f, -0.028237065f, -0.02790421f, 0.0036267154f, -0.011206065f, -0.0037469123f, -0.014756502f, -0.040977955f, 0.008654188f, -0.0034325507f, -0.013610006f, -0.03415446f, -0.01971232f, 0.000308005f, -0.0021392766f, -0.0026235322f, -0.013369612f, 0.02193134f, 0.027645325f, 0.014469878f, 0.008113302f, -0.001891948f, 0.008127171f, -0.0024478594f, -0.009671241f, -0.011381738f, 0.03816719f, 0.033821605f, 0.026739225f, 0.0029956808f, -0.0071517252f, -0.01314771f, -0.029217133f, -0.0054458515f, 0.018870939f, -0.017317623f, 0.004109815f, -0.0016943163f, -0.004003487f, 0.012491249f, -0.021506028f, 0.014294205f, 0.023484657f, 0.00077319075f, 0.02729398f, -0.0022721868f, -0.013406596f, -0.017141951f, 0.0035643054f, -0.011483443f, 0.019860253f, 0.001200815f, 0.003307731f, -0.007849793f, -0.0292911f, 0.014007581f, 0.013915122f, -0.023891479f, -0.030530054f, 0.011483443f, -0.0044287997f, 0.019582875f, -0.011474197f, 0.010420161f, -0.0020699322f, -0.00023201501f, -0.003030353f, -0.018131265f, -0.0032522553f, 0.0036036004f, 0.02489004f, -0.0034256163f, 0.0012666922f, 0.022504589f, -0.0009003222f, -0.01888943f, 0.0009303715f, 0.0019347104f, 0.013203185f, 0.0051407362f, 0.009227436f, 0.0027067454f, -0.0042161434f, 0.011132098f, -0.008002351f, 0.023725051f, 0.001089286f, 0.0073690047f, -0.010928687f, -0.0015579389f, 0.021690948f, -0.011067376f, 0.00909337f, 0.004292422f, 0.0014781927f, 0.06771718f, 0.025259875f, -0.05610429f, 0.016115652f, 0.00045998493f, 0.019656843f, -0.013878138f, 0.0014007582f, -0.0004885895f, -0.014784239f, 0.0037815846f, -0.023373706f, -0.025463287f, -0.025962567f, -0.011390983f, -0.02032255f, 0.005501327f, 0.0123433145f, 0.015496176f, -0.008885337f, -0.00091996987f, 0.001263225f, -0.024076397f, -0.011797804f, 0.022837443f, -0.006569232f, -0.014460632f, -0.0068512326f, 0.0056954916f, 0.0034787804f, -0.03567079f, -0.022338163f, -0.021469045f, -0.024427742f, 0.023651084f, 0.0050575226f, -0.029919824f, 0.014904436f, -0.014081549f, 0.0027760898f, -0.021099208f, -0.005912771f, -0.0027298604f, -0.018556578f, 0.017807657f, -0.0050575226f, -0.019952713f, 0.0015671848f, -0.011862526f, -0.013129218f, -0.03169504f, -0.010447899f, -0.016679654f, -0.017169688f, 0.006449035f, -0.000691711f, -0.017271394f, -0.048670564f, 0.017151197f, 0.017456312f, -0.003284616f, 0.00022161334f, 0.004368701f, 0.0063658217f, 0.029180149f, -0.015958471f, -0.032249797f, 0.022504589f, -0.0059636235f, -0.0060052304f, -0.005233195f, 0.03149163f, 0.024483217f, -0.012472757f, 0.015625618f, -0.021006748f, 0.0077758254f, -0.00014042253f, -0.032101862f, 0.0075169397f, 0.0060699517f, -0.009213568f, 0.014756502f, -0.010050324f, -0.023392199f, -0.016846081f, 0.0535894f, 0.0061254273f, 0.03289701f, 0.011372492f, 0.0037446008f, -0.015006142f, -0.0062410017f, -0.018454872f, 0.010392423f, 0.031916942f, 0.016208112f, 0.03912877f, -0.031010842f, 0.011280032f, 0.02113619f, -0.008936189f, -0.032046385f, 0.0356523f, -0.01389663f, -0.022097768f, -0.009236682f, -0.004731604f, -0.006569232f, -0.02091429f, -0.011196819f, -0.0083675645f, 0.039387655f, -0.02638788f, 0.0011672984f, 0.0029702545f, -0.01789087f, -0.019471925f, -0.0021230963f, 0.011668362f, -0.010133537f, -0.008191892f, 0.01921304f, 0.0034209935f, 0.03408049f, -0.002093047f, -0.0062548704f, 0.00033227555f, 0.0011239582f, -0.00028171187f, 0.0017255213f, 0.0025726794f, -0.00492808f, 0.029272608f, 0.023743544f, 0.006287231f, -0.0070222826f, 0.017511789f, 0.020488977f, 0.017058738f, 0.051555295f, -0.0072395615f, 0.017049491f, 0.010725277f, 0.030067759f, 0.010595834f, -0.016799852f, -0.008839107f, -0.004040471f, 0.011686853f, -0.03713165f, 0.0148951905f, 0.00864032f, -0.008723533f, -0.0051638507f, -0.03186147f, 0.018806217f, -0.0129812835f, 0.030493071f, -0.0002360601f, -0.023429181f, 0.02061842f, -0.008552483f, 0.010420161f, 0.02629542f, 0.0051407362f, 0.004546685f, 0.011973477f, 0.007896023f, -0.024760595f, 0.030160217f, 0.017114213f, -0.02141357f, 0.00070789136f, -0.008825239f, 0.014664042f, -0.02808913f, -0.031621072f, 0.03445033f, -0.015338995f, -0.014534599f, 0.009920881f, 0.0052054576f, 0.0016700458f, 0.023614101f, 0.013526793f, -0.0042277006f, -0.024557184f, -0.010974917f, 0.012287838f, -0.01319394f, 0.004733915f, 0.025648205f, 0.011326262f, -0.010336948f, 0.0010690604f, -0.011326262f, 0.016420769f, -0.034006525f, -0.03208337f, 0.007350513f, -0.031010842f, 0.0070777577f, -0.0135175465f, -0.042383336f, 0.016753621f, 0.027275488f, -0.009010157f, 0.034283902f, -0.00083791226f, -0.0018260708f, -0.009994849f, 0.007207201f, 0.024871547f, -0.008043958f, -0.011141343f, -0.004840243f, 0.012713151f, -0.0030118611f, -0.012260101f, -0.005954378f, -0.018288447f, -0.018020313f, 0.006388936f, -0.009994849f, -0.01235256f, -0.00818727f, 0.0020237025f, -0.0035804857f, -0.025685187f, 0.007859039f, -0.001414627f, 0.028144605f, -0.0252044f, 0.019804778f, 0.0074059884f, -0.0023172607f, -0.00074083f, -0.044713307f, 0.009772946f, -0.0044149305f, -0.012065936f, -0.017012509f, -0.015662603f, -0.004900342f, 0.05211005f, 0.012324822f, -0.010799244f, -0.017539525f, 0.035116035f, -0.008538615f, 0.0031852222f, -0.009056387f, -0.0060838205f, -0.008242745f, 0.017465558f, 0.013082989f, 0.019065104f, -0.005542934f, -0.002896287f, -0.02011914f, -0.00023331522f, -0.036225546f, 0.013526793f, 0.019601367f, -0.00018853025f, 0.030104741f, 0.0292911f, -0.0044426685f, -0.008543237f, 0.0020202354f, 0.010799244f, 0.020174615f, 0.0030534677f, -0.03408049f, -0.0045767343f, -0.01753028f, 0.007442972f, 0.021450553f, 0.028921263f, -0.024390759f, 0.007484579f, 0.0054550976f, -0.006555363f, -0.00012590353f, -0.0065414943f, 0.007211824f, 0.00829822f, 0.012546725f, 0.0046252757f, -0.0063611986f, 0.008136417f, -0.017742936f, 0.014414402f, -0.0029170904f, -0.0013683974f, 0.03824116f, 0.0013244792f, -0.0026235322f, 0.009232059f, 0.01912058f, 0.010189013f, -0.025518762f, -0.0009159247f, -0.0053071626f, 0.029457526f, 0.011372492f, -0.0053210314f, -0.022874426f, -0.008779009f, -0.004285488f, 0.029697921f, 0.025241384f, 0.022338163f, -0.02152452f, 0.0077619567f, 0.010669801f, 0.018898677f, 0.009532552f, -0.011187573f, -0.0047015543f, -0.019490417f, -0.0033655178f, 0.00060791976f, -0.045489967f, -0.0015544717f, 0.017428575f, 0.017742936f, 0.011844034f, -0.0034926494f, 0.025019482f, 0.029975299f, 0.019545892f, -0.02640637f, -0.009282912f, -0.001642308f, -0.013942859f, -0.009920881f, 0.026831683f, -0.0005507106f, 0.039202735f, 0.011714591f, 0.018149758f, 0.008617205f, 0.0051176213f, 0.0367803f, -0.0055799177f, 0.0067911344f, 0.0074291034f, -0.005649262f, -0.0018110462f, 0.010420161f, 0.006014476f, -0.016023193f, 0.017770674f, -0.007687989f, 0.006573855f, -0.028625393f, -0.017012509f, -0.031121794f, 0.023743544f, 0.008737402f, 0.02061842f, -0.0022883671f, -0.029642446f, -0.0038486177f, -0.035282463f, -0.012879577f, 0.026554305f, 0.016753621f, 0.0038070108f, 0.02660978f, -0.0030118611f, -0.011400229f, 0.013064496f, -0.015431454f, 0.01380417f, -0.010503374f, 0.0083675645f, 0.010207505f, 0.0014296516f, 0.025574237f, 0.00039064046f, 0.008552483f, 0.011788558f, -0.0014781927f, -0.0072395615f, 0.027256995f, -0.013933614f };

            SearchResults<SearchDocument> response = await searchClient.SearchAsync<SearchDocument>(
                   new SearchOptions
                   {
                       VectorSearch = new()
                       {
                           Queries = { new VectorizedQuery(searchVectorizeDescription) { KNearestNeighborsCount = 3, Fields = { "descriptionVector" } } }
                       }
                   });

            await foreach (SearchResult<SearchDocument> result in response.GetResultsAsync())
            {
                SearchDocument doc = result.Document;

                Assert.AreEqual("1", doc["id"]);
                Console.WriteLine("Hotel : " + doc["id"]);
                Console.WriteLine("Description : " + doc["description"]);
            }
        }
    }
}
