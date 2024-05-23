// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

public enum EmbeddingType
{
    Single,
 //   Half,
    Short,
    Byte
}

public partial struct EmbeddingVector
{
    public EmbeddingType Type;
    private object _vector;

    private EmbeddingVector(EmbeddingType type, object vector)
    {
        Type = type;
        _vector = vector;
    }

    // Initialization methods
    public static EmbeddingVector FromFloat32(ReadOnlyMemory<float> vector)
    {
        return new EmbeddingVector(EmbeddingType.Single, vector);
    }

    public static EmbeddingVector FromInt16(ReadOnlyMemory<short> vector)
    {
        return new EmbeddingVector(EmbeddingType.Short, vector);
    }

    public static EmbeddingVector FromInt8(ReadOnlyMemory<byte> vector)
    {
        return new EmbeddingVector(EmbeddingType.Byte, vector);
    }

    //public static EmbeddingVector FromBase64(ReadOnlyMemory<byte> utf8EncodedBytes, EmbeddingType type)
    //{
    //    return new EmbeddingVector(EmbeddingType.Int8, type);
    //}

    // Conversion methods
    public ReadOnlyMemory<float> ToFloat32Memory()
    {
        if (Type != EmbeddingType.Single)
            throw new InvalidOperationException("Cannot convert to ReadOnlyMemory<float>.");

        return (_vector as float[]) ?? ((ReadOnlyMemory<float>)_vector);
    }

    public ReadOnlyMemory<short> ToInt16Memory()
    {
        if (Type != EmbeddingType.Short)
            throw new InvalidOperationException("Cannot convert to ReadOnlyMemory<short>.");

        return (_vector as short[]) ?? ((ReadOnlyMemory<short>)_vector);
    }

    public ReadOnlyMemory<byte> ToInt8Memory()
    {
        if (Type != EmbeddingType.Byte)
            throw new InvalidOperationException("Cannot convert to ReadOnlyMemory<byte>.");

        return (_vector as byte[]) ?? ((ReadOnlyMemory<byte>)_vector);
    }

    // Additional convenience methods can be added in the future.
}

//public static class EmbeddingVectorExtensions
//{
//    public static byte[] QuantizeBinary(this EmbeddingVector vector)
//    {
//        return
//    }

//    private static byte[] QuantizeToBinary<T>(T[] data) where T : struct, IComparable<T>
//    {
//        byte[] quantizedData = new byte[data.Length];
//        T zero = default(T);
//        for (int i = 0; i < data.Length; i++)
//        {
//            quantizedData[i] = (byte)(data[i].CompareTo(zero) >= 0 ? 1 : 0);
//        }
//        return quantizedData;
//    }

//    private static T[] DequantizeFromBinary<T>(byte[] data) where T : struct
//    {
//        T[] dequantizedData = new T[data.Length];
//        T one = (T)Convert.ChangeType(1, typeof(T));
//        for (int i = 0; i < data.Length; i++)
//        {
//            dequantizedData[i] = data[i] == 1 ? one : default(T);
//        }
//        return dequantizedData;
//    }
//}
