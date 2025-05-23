// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Azure.AI.FormRecognizer.DocumentAnalysis
{
    internal partial class GetDocumentClassifiersResponse
    {
        internal static GetDocumentClassifiersResponse DeserializeGetDocumentClassifiersResponse(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            IReadOnlyList<DocumentClassifierDetails> value = default;
            Uri nextLink = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"u8))
                {
                    List<DocumentClassifierDetails> array = new List<DocumentClassifierDetails>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(DocumentClassifierDetails.DeserializeDocumentClassifierDetails(item));
                    }
                    value = array;
                    continue;
                }
                if (property.NameEquals("nextLink"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    nextLink = new Uri(property.Value.GetString());
                    continue;
                }
            }
            return new GetDocumentClassifiersResponse(value, nextLink);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static GetDocumentClassifiersResponse FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content, ModelSerializationExtensions.JsonDocumentOptions);
            return DeserializeGetDocumentClassifiersResponse(document.RootElement);
        }
    }
}
