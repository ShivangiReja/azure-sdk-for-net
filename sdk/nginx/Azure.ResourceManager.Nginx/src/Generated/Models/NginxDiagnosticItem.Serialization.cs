// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Nginx.Models
{
    public partial class NginxDiagnosticItem : IUtf8JsonSerializable, IJsonModel<NginxDiagnosticItem>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<NginxDiagnosticItem>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<NginxDiagnosticItem>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected virtual void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<NginxDiagnosticItem>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(NginxDiagnosticItem)} does not support writing '{format}' format.");
            }

            if (Optional.IsDefined(Id))
            {
                writer.WritePropertyName("id"u8);
                writer.WriteStringValue(Id);
            }
            writer.WritePropertyName("directive"u8);
            writer.WriteStringValue(Directive);
            writer.WritePropertyName("description"u8);
            writer.WriteStringValue(Description);
            writer.WritePropertyName("file"u8);
            writer.WriteStringValue(File);
            writer.WritePropertyName("line"u8);
            writer.WriteNumberValue(Line);
            writer.WritePropertyName("message"u8);
            writer.WriteStringValue(Message);
            writer.WritePropertyName("rule"u8);
            writer.WriteStringValue(Rule);
            writer.WritePropertyName("level"u8);
            writer.WriteStringValue(Level.ToString());
            if (Optional.IsDefined(Category))
            {
                writer.WritePropertyName("category"u8);
                writer.WriteStringValue(Category);
            }
            if (options.Format != "W" && _serializedAdditionalRawData != null)
            {
                foreach (var item in _serializedAdditionalRawData)
                {
                    writer.WritePropertyName(item.Key);
#if NET6_0_OR_GREATER
				writer.WriteRawValue(item.Value);
#else
                    using (JsonDocument document = JsonDocument.Parse(item.Value, ModelSerializationExtensions.JsonDocumentOptions))
                    {
                        JsonSerializer.Serialize(writer, document.RootElement);
                    }
#endif
                }
            }
        }

        NginxDiagnosticItem IJsonModel<NginxDiagnosticItem>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<NginxDiagnosticItem>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(NginxDiagnosticItem)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeNginxDiagnosticItem(document.RootElement, options);
        }

        internal static NginxDiagnosticItem DeserializeNginxDiagnosticItem(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string directive = default;
            string description = default;
            string file = default;
            float line = default;
            string message = default;
            string rule = default;
            NginxDiagnosticLevel level = default;
            string category = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> rawDataDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("directive"u8))
                {
                    directive = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("description"u8))
                {
                    description = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("file"u8))
                {
                    file = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("line"u8))
                {
                    line = property.Value.GetSingle();
                    continue;
                }
                if (property.NameEquals("message"u8))
                {
                    message = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("rule"u8))
                {
                    rule = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("level"u8))
                {
                    level = new NginxDiagnosticLevel(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("category"u8))
                {
                    category = property.Value.GetString();
                    continue;
                }
                if (options.Format != "W")
                {
                    rawDataDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = rawDataDictionary;
            return new NginxDiagnosticItem(
                id,
                directive,
                description,
                file,
                line,
                message,
                rule,
                level,
                category,
                serializedAdditionalRawData);
        }

        BinaryData IPersistableModel<NginxDiagnosticItem>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<NginxDiagnosticItem>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options, AzureResourceManagerNginxContext.Default);
                default:
                    throw new FormatException($"The model {nameof(NginxDiagnosticItem)} does not support writing '{options.Format}' format.");
            }
        }

        NginxDiagnosticItem IPersistableModel<NginxDiagnosticItem>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<NginxDiagnosticItem>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeNginxDiagnosticItem(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(NginxDiagnosticItem)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<NginxDiagnosticItem>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
