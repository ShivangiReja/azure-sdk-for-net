// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;

namespace Azure.ResourceManager.Synapse
{
    public partial class SynapseSqlPoolTableResource : IJsonModel<SynapseSqlPoolTableData>
    {
        private static SynapseSqlPoolTableData s_dataDeserializationInstance;
        private static SynapseSqlPoolTableData DataDeserializationInstance => s_dataDeserializationInstance ??= new();

        void IJsonModel<SynapseSqlPoolTableData>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options) => ((IJsonModel<SynapseSqlPoolTableData>)Data).Write(writer, options);

        SynapseSqlPoolTableData IJsonModel<SynapseSqlPoolTableData>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options) => ((IJsonModel<SynapseSqlPoolTableData>)DataDeserializationInstance).Create(ref reader, options);

        BinaryData IPersistableModel<SynapseSqlPoolTableData>.Write(ModelReaderWriterOptions options) => ModelReaderWriter.Write<SynapseSqlPoolTableData>(Data, options, AzureResourceManagerSynapseContext.Default);

        SynapseSqlPoolTableData IPersistableModel<SynapseSqlPoolTableData>.Create(BinaryData data, ModelReaderWriterOptions options) => ModelReaderWriter.Read<SynapseSqlPoolTableData>(data, options, AzureResourceManagerSynapseContext.Default);

        string IPersistableModel<SynapseSqlPoolTableData>.GetFormatFromOptions(ModelReaderWriterOptions options) => ((IPersistableModel<SynapseSqlPoolTableData>)DataDeserializationInstance).GetFormatFromOptions(options);
    }
}
