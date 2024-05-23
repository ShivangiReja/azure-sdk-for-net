// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

public class EmbeddingVectorTest
{
    private byte[] _data;

    private EmbeddingVectorTest(byte[] data)
    {
        _data = data;
    }

    public static EmbeddingVectorTest FromFloat32(float[] data)
    {
        byte[] quantizedData = QuantizeToBinary(data);
        return new EmbeddingVectorTest(quantizedData);
    }

    public static EmbeddingVectorTest FromInt16(short[] data)
    {
        byte[] quantizedData = QuantizeToBinary(data);
        return new EmbeddingVectorTest(quantizedData);
    }

    public float[] ToFloat32Array()
    {
        return DequantizeFromBinary<float>(_data);
    }

    public short[] ToInt16Array()
    {
        return DequantizeFromBinary<short>(_data);
    }

    private static byte[] QuantizeToBinary<T>(T[] data) where T : struct, IComparable<T>
    {
        byte[] quantizedData = new byte[data.Length];
        T zero = default(T);
        for (int i = 0; i < data.Length; i++)
        {
            quantizedData[i] = (byte)(data[i].CompareTo(zero) >= 0 ? 1 : 0);
        }
        return quantizedData;
    }

    private static T[] DequantizeFromBinary<T>(byte[] data) where T : struct
    {
        T[] dequantizedData = new T[data.Length];
        T one = (T)Convert.ChangeType(1, typeof(T));
        for (int i = 0; i < data.Length; i++)
        {
            dequantizedData[i] = data[i] == 1 ? one : default(T);
        }
        return dequantizedData;
    }
}
