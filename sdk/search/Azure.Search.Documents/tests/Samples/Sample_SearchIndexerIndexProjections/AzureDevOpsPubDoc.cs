// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure.Search.Documents.Indexes;

namespace AzureDevOps.Copilot.ETL;

internal class AzureDevOpsPubDoc
{
    public const string ParentIdFieldName = "parent_id";
    public const string TitleFieldName = "metadata_storage_name";
    public const string ChunkFieldName = "chunk";
    public const string LinksFieldName = "links";
    public const string KeyphrasesFieldName = "keyphrases";
    public const string VectorFieldName = "vector";

    [JsonPropertyName("id")]
    [SearchableField(IsKey = true, IsFilterable = true, AnalyzerName = "en.microsoft")]
    public string Id { get; set; }

    [JsonPropertyName(ParentIdFieldName)]
    [SearchableField(IsFilterable = true)]
    public string ParentId { get; set; }

    [JsonPropertyName("metadata_storage_path")]
    [SearchableField(IsSortable = true, IsFilterable = true)]
    public string MetadataStoragePath { get; set; }

    [JsonPropertyName("metadata_storage_name")]
    [SearchableField(IsSortable = true, IsFilterable = true)]
    public string MetadataStorageName { get; set; }

    [JsonPropertyName(ChunkFieldName)]
    [SearchableField(AnalyzerName = "en.microsoft")]
    public string Chunk { get; set; }

    [JsonPropertyName(LinksFieldName)]
    [SearchableField]
    public IReadOnlyList<string> Links { get; set; }

    [JsonPropertyName(KeyphrasesFieldName)]
    [SearchableField]
    public IReadOnlyList<string> Keyphrases { get; set; }

    [JsonPropertyName(VectorFieldName)]
    [VectorSearchField(VectorSearchProfileName = "my-vector-config", VectorSearchDimensions = 1536)]
    public ReadOnlyMemory<float> Vector { get; set; }
}
