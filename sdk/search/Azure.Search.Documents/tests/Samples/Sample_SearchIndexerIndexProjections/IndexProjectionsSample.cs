// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using AzureDevOps.Copilot.ETL;
using NUnit.Framework;

namespace Azure.Search.Documents.Tests.Samples
{
    // https://learn.microsoft.com/en-us/dotnet/api/azure.search.documents.indexes.models.searchindexerskillset?view=azure-dotnet
    public partial class IndexProjectionsSample : SearchTestBase
    {
        public IndexProjectionsSample(bool async, SearchClientOptions.ServiceVersion serviceVersion)
            : base(async, SearchClientOptions.ServiceVersion.V2023_10_01_Preview, RecordedTestMode.Record /* to re-record */)
        {
        }

        [Test]
        public async Task IndexProjectionsTest()
        {
            await using SearchResources resources = await SearchResources.CreateWithBlobStorageAsync(this, populate: true, true);
            Environment.SetEnvironmentVariable("SEARCH_INDEX", Recording.Random.GetName());

            var indexClient = new SearchIndexClient(new Uri(resources.Endpoint.ToString()), new AzureKeyCredential(resources.PrimaryApiKey));
            var searchIndexerClient = new SearchIndexerClient(new Uri(resources.Endpoint.ToString()), new AzureKeyCredential(resources.PrimaryApiKey));

            // INDEX
            var index = CreateIndex();
            await indexClient.CreateOrUpdateIndexAsync(index).ConfigureAwait(false);

            // SKILLSET
            var skillset = CreateSkillset(resources);
            await searchIndexerClient.CreateOrUpdateSkillsetAsync(skillset).ConfigureAwait(false);
        }

        // https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/search/Azure.Search.Documents/samples/Sample02_Service.md
        private SearchIndex CreateIndex()
        {
            var fieldBuilder = new FieldBuilder();

            var fields = fieldBuilder.Build(typeof(AzureDevOpsPubDoc));

            var semanticTitleField = new SemanticField(AzureDevOpsPubDoc.TitleFieldName);
            var keywordSemanticField = new SemanticField(AzureDevOpsPubDoc.KeyphrasesFieldName);
            var contentSemanticField = new SemanticField(AzureDevOpsPubDoc.ChunkFieldName);
            var indexName = Environment.GetEnvironmentVariable("SEARCH_INDEX");
            return new SearchIndex(indexName)
            {
                Fields = fields,
                VectorSearch = new()
                {
                    Profiles = { new VectorSearchProfile("my-vector-config-profile", "my-vector-config") },
                    Algorithms = { new HnswAlgorithmConfiguration("my-vector-config") }
                },
                SemanticSearch = new()
                {
                    Configurations = {
                    new("my-semantic-config", new()
                    {
                        TitleField = semanticTitleField,
                        KeywordsFields = { keywordSemanticField },
                        ContentFields = { contentSemanticField }
                    })
                }
                }
            };
        }

        private SearchIndexerSkillset CreateSkillset(SearchResources resources)
        {
            var chunkSource = $"/document/{AzureDevOpsPubDoc.ChunkFieldName}/*";

            var s1Input = new InputFieldMappingEntry("text")
            {
                Source = chunkSource
            };
            var s1Output = new OutputFieldMappingEntry("urls")
            {
                TargetName = AzureDevOpsPubDoc.LinksFieldName
            };
            var s1 = new EntityRecognitionSkill(new List<InputFieldMappingEntry> { s1Input }, new List<OutputFieldMappingEntry> { s1Output }, EntityRecognitionSkill.SkillVersion.V3)
            {
                Name = "extract links"
            };

            var indexName = Environment.GetEnvironmentVariable("SEARCH_INDEX");
            var indexProjections = new SearchIndexerIndexProjections(
                new List<SearchIndexerIndexProjectionSelector>
                {
                new SearchIndexerIndexProjectionSelector(indexName, AzureDevOpsPubDoc.ParentIdFieldName, chunkSource,
                    new List<InputFieldMappingEntry>
                    {
                        new(AzureDevOpsPubDoc.ChunkFieldName) { Source = chunkSource },
                    })
                }
            )
            {
                Parameters = new SearchIndexerIndexProjectionsParameters()
                {
                    ProjectionMode = IndexProjectionMode.SkipIndexingParentDocuments
                }
            };

            var skillsetName = Recording.Random.GetName();
            return new SearchIndexerSkillset(skillsetName, new List<SearchIndexerSkill> { s1 })
            {
                Description = "Skillset for indexing AzureDevOps Public Documentation",
                IndexProjections = indexProjections
            };
        }
    }
}
