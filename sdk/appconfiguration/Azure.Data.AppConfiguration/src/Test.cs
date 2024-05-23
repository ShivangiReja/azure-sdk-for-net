using System;
using System.Text.Json;

EmbeddingVector vector = EmbeddingVector.FromJson("[1.0,2.0,3.0]"u8);
EmbeddingVector<float> floats = vector.To<float>();
foreach (float scalar in floats.Scalars.Span)
{
    Console.WriteLine(scalar);
}

namespace System.AI
{
    public abstract class EmbeddingVector
    {
        public virtual EmbeddingVector<T> To<T>() where T : struct
            => throw new NotSupportedException();

        public static EmbeddingVector FromJson(ReadOnlySpan<byte> jsonArray)
            => new JsonArrayVector(jsonArray.ToArray());

        public static EmbeddingVector<T> FromScalars<T>(ReadOnlyMemory<T> scalars) where T : struct
            => new EmbeddingVector<T>(scalars);

        public abstract void Write(Utf8JsonWriter writer, string format);
    }

    public class EmbeddingVector<T> : EmbeddingVector where T : struct
    {
        private readonly ReadOnlyMemory<T> _scalars;

        public EmbeddingVector(ReadOnlyMemory<T> scalars) => _scalars = scalars;

        public ReadOnlyMemory<T> Scalars => _scalars;

        public override EmbeddingVector<TTarget> To<TTarget>()
        {
            if (typeof(T) == typeof(TTarget))
                return (EmbeddingVector<TTarget>)(object)this;

            throw new NotSupportedException($"Conversion from {typeof(T)} to {typeof(TTarget)} is not supported.");
        }

        public override void Write(Utf8JsonWriter writer, string format)
        {
            if (!format.Equals("J", StringComparison.Ordinal))
                throw new NotSupportedException();

            writer.WriteStartArray();

            foreach (var scalar in _scalars.Span)
            {
                if (typeof(T) == typeof(float))
                {
                    writer.WriteNumberValue((float)(object)scalar);
                }
#if NET5_0_OR_GREATER
                else if (typeof(T) == typeof(Half))
                {
                    writer.WriteNumberValue((double)(Half)(object)scalar!);
                }
#endif
                else
                {
                    throw new NotSupportedException($"Type {typeof(T)} is not supported.");
                }
            }

            writer.WriteEndArray();
        }
    }

    internal class JsonArrayVector : EmbeddingVector
    {
        private readonly ReadOnlyMemory<byte> _jsonArray;

        public JsonArrayVector(ReadOnlyMemory<byte> jsonArray) => _jsonArray = jsonArray;

        public override EmbeddingVector<T> To<T>()
        {
            if (typeof(T) == typeof(float))
            {
                return (EmbeddingVector<T>)(object)ToSingle();
            }
#if NET5_0_OR_GREATER
            if (typeof(T) == typeof(Half))
            {
                return (EmbeddingVector<T>)(object)ToHalf();
            }
#endif
            throw new NotSupportedException($"Conversion to {typeof(T)} is not supported.");
        }

        private EmbeddingVector<float> ToSingle()
        {
            var buffer = ParseJson<float>((reader) => reader.GetSingle());
            return new EmbeddingVector<float>(buffer.AsMemory());
        }

#if NET5_0_OR_GREATER
        private EmbeddingVector<Half> ToHalf()
        {
            var buffer = ParseJson<Half>((reader) => (Half)reader.GetDouble());
            return new EmbeddingVector<Half>(buffer.AsMemory());
        }
#endif

        private T[] ParseJson<T>(Func<Utf8JsonReader, T> parseValue)
        {
            var sizeHint = ComputeSize();
            var buffer = new T[sizeHint];
            var reader = new Utf8JsonReader(_jsonArray.Span);
            int index = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    buffer[index++] = parseValue(reader);
                }
            }
            Array.Resize(ref buffer, index); // Adjust size to actual number of elements
            return buffer;
        }

        private int ComputeSize()
        {
            int sizeHint = 1;
            foreach (byte token in _jsonArray.Span) if (token == ',') sizeHint++;
            return sizeHint;
        }

        public override void Write(Utf8JsonWriter writer, string format)
        {
            if (!format.Equals("J", StringComparison.Ordinal)) throw new NotSupportedException();
            writer.WriteRawValue(_jsonArray.Span);
        }
    }
}
