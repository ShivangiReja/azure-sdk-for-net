// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;

namespace Azure.ResourceManager.OracleDatabase
{
    public partial class AutonomousDatabaseResource : IJsonModel<AutonomousDatabaseData>
    {
        private static AutonomousDatabaseData s_dataDeserializationInstance;
        private static AutonomousDatabaseData DataDeserializationInstance => s_dataDeserializationInstance ??= new();

        void IJsonModel<AutonomousDatabaseData>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options) => ((IJsonModel<AutonomousDatabaseData>)Data).Write(writer, options);

        AutonomousDatabaseData IJsonModel<AutonomousDatabaseData>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options) => ((IJsonModel<AutonomousDatabaseData>)DataDeserializationInstance).Create(ref reader, options);

        BinaryData IPersistableModel<AutonomousDatabaseData>.Write(ModelReaderWriterOptions options) => ModelReaderWriter.Write<AutonomousDatabaseData>(Data, options, AzureResourceManagerOracleDatabaseContext.Default);

        AutonomousDatabaseData IPersistableModel<AutonomousDatabaseData>.Create(BinaryData data, ModelReaderWriterOptions options) => ModelReaderWriter.Read<AutonomousDatabaseData>(data, options, AzureResourceManagerOracleDatabaseContext.Default);

        string IPersistableModel<AutonomousDatabaseData>.GetFormatFromOptions(ModelReaderWriterOptions options) => ((IPersistableModel<AutonomousDatabaseData>)DataDeserializationInstance).GetFormatFromOptions(options);
    }
}
