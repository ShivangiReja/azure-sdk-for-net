// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
namespace Azure.Search.Documents.Models
{
    /// <summary>
    /// </summary>
    public partial class EmbeddingsTest
    {
        internal EmbeddingsTest(byte[] data) { }
        internal EmbeddingsTest(ReadOnlyMemory<byte> data) { }

        /// <summary>
        /// </summary>
        public static EmbeddingsTest FromFloat32(float[] data) { throw null; }
        /// <summary>
        /// </summary>
        public static EmbeddingsTest FromFloat32(ReadOnlyMemory<float> data) { throw null; }

        /// <summary>
        /// </summary>
        public ReadOnlyMemory<float> ToFloat32Memory() { throw null; }

        /// <summary>
        /// </summary>
        public float[] ToFloat32Array() { throw null; }

        // We can define more convineice methods when we will have float8 in future.
    }
}
