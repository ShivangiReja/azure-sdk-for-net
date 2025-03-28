// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;

namespace Azure.Messaging.EventGrid.SystemEvents
{
    public partial class DeviceTwinInfoProperties
    {
        internal static DeviceTwinInfoProperties DeserializeDeviceTwinInfoProperties(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            DeviceTwinProperties desired = default;
            DeviceTwinProperties reported = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("desired"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    desired = DeviceTwinProperties.DeserializeDeviceTwinProperties(property.Value);
                    continue;
                }
                if (property.NameEquals("reported"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    reported = DeviceTwinProperties.DeserializeDeviceTwinProperties(property.Value);
                    continue;
                }
            }
            return new DeviceTwinInfoProperties(desired, reported);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static DeviceTwinInfoProperties FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content, ModelSerializationExtensions.JsonDocumentOptions);
            return DeserializeDeviceTwinInfoProperties(document.RootElement);
        }
    }
}
