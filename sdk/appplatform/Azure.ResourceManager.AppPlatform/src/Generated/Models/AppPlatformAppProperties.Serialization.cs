// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.AppPlatform.Models
{
    public partial class AppPlatformAppProperties : IUtf8JsonSerializable, IJsonModel<AppPlatformAppProperties>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<AppPlatformAppProperties>)this).Write(writer, ModelSerializationExtensions.WireOptions);

        void IJsonModel<AppPlatformAppProperties>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            writer.WriteStartObject();
            JsonModelWriteCore(writer, options);
            writer.WriteEndObject();
        }

        /// <param name="writer"> The JSON writer. </param>
        /// <param name="options"> The client options for reading and writing models. </param>
        protected virtual void JsonModelWriteCore(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformAppProperties>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(AppPlatformAppProperties)} does not support writing '{format}' format.");
            }

            if (Optional.IsDefined(IsPublic))
            {
                writer.WritePropertyName("public"u8);
                writer.WriteBooleanValue(IsPublic.Value);
            }
            if (options.Format != "W" && Optional.IsDefined(UriString))
            {
                writer.WritePropertyName("url"u8);
                writer.WriteStringValue(UriString);
            }
            if (Optional.IsCollectionDefined(AddonConfigs))
            {
                writer.WritePropertyName("addonConfigs"u8);
                writer.WriteStartObject();
                foreach (var item in AddonConfigs)
                {
                    writer.WritePropertyName(item.Key);
                    if (item.Value == null)
                    {
                        writer.WriteNullValue();
                        continue;
                    }
                    writer.WriteStartObject();
                    foreach (var item0 in item.Value)
                    {
                        writer.WritePropertyName(item0.Key);
                        if (item0.Value == null)
                        {
                            writer.WriteNullValue();
                            continue;
                        }
#if NET6_0_OR_GREATER
				writer.WriteRawValue(item0.Value);
#else
                        using (JsonDocument document = JsonDocument.Parse(item0.Value, ModelSerializationExtensions.JsonDocumentOptions))
                        {
                            JsonSerializer.Serialize(writer, document.RootElement);
                        }
#endif
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }
            if (options.Format != "W" && Optional.IsDefined(ProvisioningState))
            {
                writer.WritePropertyName("provisioningState"u8);
                writer.WriteStringValue(ProvisioningState.Value.ToString());
            }
            if (options.Format != "W" && Optional.IsDefined(Fqdn))
            {
                writer.WritePropertyName("fqdn"u8);
                writer.WriteStringValue(Fqdn);
            }
            if (Optional.IsDefined(IsHttpsOnly))
            {
                writer.WritePropertyName("httpsOnly"u8);
                writer.WriteBooleanValue(IsHttpsOnly.Value);
            }
            if (Optional.IsDefined(TemporaryDisk))
            {
                writer.WritePropertyName("temporaryDisk"u8);
                writer.WriteObjectValue(TemporaryDisk, options);
            }
            if (Optional.IsDefined(PersistentDisk))
            {
                writer.WritePropertyName("persistentDisk"u8);
                writer.WriteObjectValue(PersistentDisk, options);
            }
            if (Optional.IsCollectionDefined(CustomPersistentDisks))
            {
                writer.WritePropertyName("customPersistentDisks"u8);
                writer.WriteStartArray();
                foreach (var item in CustomPersistentDisks)
                {
                    writer.WriteObjectValue(item, options);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(IsEndToEndTlsEnabled))
            {
                writer.WritePropertyName("enableEndToEndTLS"u8);
                writer.WriteBooleanValue(IsEndToEndTlsEnabled.Value);
            }
            if (Optional.IsCollectionDefined(LoadedCertificates))
            {
                writer.WritePropertyName("loadedCertificates"u8);
                writer.WriteStartArray();
                foreach (var item in LoadedCertificates)
                {
                    writer.WriteObjectValue(item, options);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(VnetAddons))
            {
                writer.WritePropertyName("vnetAddons"u8);
                writer.WriteObjectValue(VnetAddons, options);
            }
            if (Optional.IsDefined(IngressSettings))
            {
                writer.WritePropertyName("ingressSettings"u8);
                writer.WriteObjectValue(IngressSettings, options);
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

        AppPlatformAppProperties IJsonModel<AppPlatformAppProperties>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformAppProperties>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(AppPlatformAppProperties)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeAppPlatformAppProperties(document.RootElement, options);
        }

        internal static AppPlatformAppProperties DeserializeAppPlatformAppProperties(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= ModelSerializationExtensions.WireOptions;

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            bool? @public = default;
            string uri = default;
            IDictionary<string, IDictionary<string, BinaryData>> addonConfigs = default;
            AppPlatformAppProvisioningState? provisioningState = default;
            string fqdn = default;
            bool? httpsOnly = default;
            AppTemporaryDisk temporaryDisk = default;
            AppPersistentDisk persistentDisk = default;
            IList<AppCustomPersistentDisk> customPersistentDisks = default;
            bool? enableEndToEndTls = default;
            IList<AppLoadedCertificate> loadedCertificates = default;
            AppVnetAddons vnetAddons = default;
            AppIngressSettings ingressSettings = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> rawDataDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("public"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    @public = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("url"u8))
                {
                    uri = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("addonConfigs"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    Dictionary<string, IDictionary<string, BinaryData>> dictionary = new Dictionary<string, IDictionary<string, BinaryData>>();
                    foreach (var property0 in property.Value.EnumerateObject())
                    {
                        if (property0.Value.ValueKind == JsonValueKind.Null)
                        {
                            dictionary.Add(property0.Name, null);
                        }
                        else
                        {
                            Dictionary<string, BinaryData> dictionary0 = new Dictionary<string, BinaryData>();
                            foreach (var property1 in property0.Value.EnumerateObject())
                            {
                                if (property1.Value.ValueKind == JsonValueKind.Null)
                                {
                                    dictionary0.Add(property1.Name, null);
                                }
                                else
                                {
                                    dictionary0.Add(property1.Name, BinaryData.FromString(property1.Value.GetRawText()));
                                }
                            }
                            dictionary.Add(property0.Name, dictionary0);
                        }
                    }
                    addonConfigs = dictionary;
                    continue;
                }
                if (property.NameEquals("provisioningState"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    provisioningState = new AppPlatformAppProvisioningState(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("fqdn"u8))
                {
                    fqdn = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("httpsOnly"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    httpsOnly = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("temporaryDisk"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    temporaryDisk = AppTemporaryDisk.DeserializeAppTemporaryDisk(property.Value, options);
                    continue;
                }
                if (property.NameEquals("persistentDisk"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    persistentDisk = AppPersistentDisk.DeserializeAppPersistentDisk(property.Value, options);
                    continue;
                }
                if (property.NameEquals("customPersistentDisks"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<AppCustomPersistentDisk> array = new List<AppCustomPersistentDisk>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(AppCustomPersistentDisk.DeserializeAppCustomPersistentDisk(item, options));
                    }
                    customPersistentDisks = array;
                    continue;
                }
                if (property.NameEquals("enableEndToEndTLS"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    enableEndToEndTls = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("loadedCertificates"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<AppLoadedCertificate> array = new List<AppLoadedCertificate>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(AppLoadedCertificate.DeserializeAppLoadedCertificate(item, options));
                    }
                    loadedCertificates = array;
                    continue;
                }
                if (property.NameEquals("vnetAddons"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    vnetAddons = AppVnetAddons.DeserializeAppVnetAddons(property.Value, options);
                    continue;
                }
                if (property.NameEquals("ingressSettings"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    ingressSettings = AppIngressSettings.DeserializeAppIngressSettings(property.Value, options);
                    continue;
                }
                if (options.Format != "W")
                {
                    rawDataDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = rawDataDictionary;
            return new AppPlatformAppProperties(
                @public,
                uri,
                addonConfigs ?? new ChangeTrackingDictionary<string, IDictionary<string, BinaryData>>(),
                provisioningState,
                fqdn,
                httpsOnly,
                temporaryDisk,
                persistentDisk,
                customPersistentDisks ?? new ChangeTrackingList<AppCustomPersistentDisk>(),
                enableEndToEndTls,
                loadedCertificates ?? new ChangeTrackingList<AppLoadedCertificate>(),
                vnetAddons,
                ingressSettings,
                serializedAdditionalRawData);
        }

        BinaryData IPersistableModel<AppPlatformAppProperties>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformAppProperties>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options, AzureResourceManagerAppPlatformContext.Default);
                default:
                    throw new FormatException($"The model {nameof(AppPlatformAppProperties)} does not support writing '{options.Format}' format.");
            }
        }

        AppPlatformAppProperties IPersistableModel<AppPlatformAppProperties>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AppPlatformAppProperties>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data, ModelSerializationExtensions.JsonDocumentOptions);
                        return DeserializeAppPlatformAppProperties(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(AppPlatformAppProperties)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<AppPlatformAppProperties>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";
    }
}
