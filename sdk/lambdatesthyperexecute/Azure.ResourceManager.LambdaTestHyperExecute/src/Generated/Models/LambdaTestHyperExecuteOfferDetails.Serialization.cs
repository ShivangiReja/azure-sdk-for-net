// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.LambdaTestHyperExecute.Models
{
    public partial class LambdaTestHyperExecuteOfferDetails : IUtf8JsonSerializable, IJsonModel<LambdaTestHyperExecuteOfferDetails>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<LambdaTestHyperExecuteOfferDetails>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<LambdaTestHyperExecuteOfferDetails>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected virtual void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LambdaTestHyperExecuteOfferDetails>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(LambdaTestHyperExecuteOfferDetails)} does not support writing '{format}' format.");
            }

            writer.WritePropertyName("publisherId"u8);
            writer.WriteStringValue(PublisherId);
            writer.WritePropertyName("offerId"u8);
            writer.WriteStringValue(OfferId);
            writer.WritePropertyName("planId"u8);
            writer.WriteStringValue(PlanId);
            if (Optional.IsDefined(PlanName))
            {
                writer.WritePropertyName("planName"u8);
                writer.WriteStringValue(PlanName);
            }
            if (Optional.IsDefined(TermUnit))
            {
                writer.WritePropertyName("termUnit"u8);
                writer.WriteStringValue(TermUnit);
            }
            if (Optional.IsDefined(TermId))
            {
                writer.WritePropertyName("termId"u8);
                writer.WriteStringValue(TermId);
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

        LambdaTestHyperExecuteOfferDetails IJsonModel<LambdaTestHyperExecuteOfferDetails>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LambdaTestHyperExecuteOfferDetails>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(LambdaTestHyperExecuteOfferDetails)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeLambdaTestHyperExecuteOfferDetails(document.RootElement, options);
        }

        internal static LambdaTestHyperExecuteOfferDetails DeserializeLambdaTestHyperExecuteOfferDetails(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string publisherId = default;
            string offerId = default;
            string planId = default;
            string planName = default;
            string termUnit = default;
            string termId = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> rawDataDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("publisherId"u8))
                {
                    publisherId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("offerId"u8))
                {
                    offerId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("planId"u8))
                {
                    planId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("planName"u8))
                {
                    planName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("termUnit"u8))
                {
                    termUnit = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("termId"u8))
                {
                    termId = property.Value.GetString();
                    continue;
                }
                if (options.Format != "W")
                {
                    rawDataDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = rawDataDictionary;
            return new LambdaTestHyperExecuteOfferDetails(
                publisherId,
                offerId,
                planId,
                planName,
                termUnit,
                termId,
                serializedAdditionalRawData);
        }

        BinaryData IPersistableModel<LambdaTestHyperExecuteOfferDetails>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LambdaTestHyperExecuteOfferDetails>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options, AzureResourceManagerLambdaTestHyperExecuteContext.Default);
                default:
                    throw new FormatException($"The model {nameof(LambdaTestHyperExecuteOfferDetails)} does not support writing '{options.Format}' format.");
            }
        }

        LambdaTestHyperExecuteOfferDetails IPersistableModel<LambdaTestHyperExecuteOfferDetails>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<LambdaTestHyperExecuteOfferDetails>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeLambdaTestHyperExecuteOfferDetails(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(LambdaTestHyperExecuteOfferDetails)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<LambdaTestHyperExecuteOfferDetails>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
