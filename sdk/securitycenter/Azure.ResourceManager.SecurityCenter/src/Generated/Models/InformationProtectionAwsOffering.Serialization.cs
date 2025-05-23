// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.SecurityCenter.Models
{
    public partial class InformationProtectionAwsOffering : IUtf8JsonSerializable, IJsonModel<InformationProtectionAwsOffering>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<InformationProtectionAwsOffering>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<InformationProtectionAwsOffering>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected override void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<InformationProtectionAwsOffering>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(InformationProtectionAwsOffering)} does not support writing '{format}' format.");
            }

            base.JsonModelWriteCore(writer, options);
            if (Optional.IsDefined(InformationProtection))
            {
                writer.WritePropertyName("informationProtection"u8);
                writer.WriteObjectValue(InformationProtection, options);
            }
        }

        InformationProtectionAwsOffering IJsonModel<InformationProtectionAwsOffering>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<InformationProtectionAwsOffering>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(InformationProtectionAwsOffering)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeInformationProtectionAwsOffering(document.RootElement, options);
        }

        internal static InformationProtectionAwsOffering DeserializeInformationProtectionAwsOffering(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            AwsInformationProtection informationProtection = default;
            OfferingType offeringType = default;
            string description = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> rawDataDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("informationProtection"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    informationProtection = AwsInformationProtection.DeserializeAwsInformationProtection(property.Value, options);
                    continue;
                }
                if (property.NameEquals("offeringType"u8))
                {
                    offeringType = new OfferingType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("description"u8))
                {
                    description = property.Value.GetString();
                    continue;
                }
                if (options.Format != "W")
                {
                    rawDataDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = rawDataDictionary;
            return new InformationProtectionAwsOffering(offeringType, description, serializedAdditionalRawData, informationProtection);
        }

        BinaryData IPersistableModel<InformationProtectionAwsOffering>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<InformationProtectionAwsOffering>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options, AzureResourceManagerSecurityCenterContext.Default);
                default:
                    throw new FormatException($"The model {nameof(InformationProtectionAwsOffering)} does not support writing '{options.Format}' format.");
            }
        }

        InformationProtectionAwsOffering IPersistableModel<InformationProtectionAwsOffering>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<InformationProtectionAwsOffering>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeInformationProtectionAwsOffering(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(InformationProtectionAwsOffering)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<InformationProtectionAwsOffering>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
