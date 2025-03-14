// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Search.Documents.Models
{
    public partial class HybridSearch : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(MaxTextRecallSize))
            {
                writer.WritePropertyName("maxTextRecallSize"u8);
                writer.WriteNumberValue(MaxTextRecallSize.Value);
            }
            if (Optional.IsDefined(CountAndFacetMode))
            {
                writer.WritePropertyName("countAndFacetMode"u8);
                writer.WriteStringValue(CountAndFacetMode.Value.ToString());
            }
            writer.WriteEndObject();
        }

        internal static HybridSearch DeserializeHybridSearch(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            int? maxTextRecallSize = default;
            HybridCountAndFacetMode? countAndFacetMode = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("maxTextRecallSize"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    maxTextRecallSize = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("countAndFacetMode"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    countAndFacetMode = new HybridCountAndFacetMode(property.Value.GetString());
                    continue;
                }
            }
            return new HybridSearch(maxTextRecallSize, countAndFacetMode);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static HybridSearch FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content, ModelSerializationExtensions.JsonDocumentOptions);
            return DeserializeHybridSearch(document.RootElement);
        }

        /// <summary> Convert into a <see cref="RequestContent"/>. </summary>
        internal virtual RequestContent ToRequestContent()
        {
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(this);
            return content;
        }
    }
}
