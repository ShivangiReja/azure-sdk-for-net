// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.Search.Documents.Models
{
    /// <summary> Specifies how the LLM should format the response. Possible values: 'text' (plain string), 'json_object' (arbitrary JSON), or 'json_schema' (adheres to provided schema). </summary>
    public readonly partial struct ChatCompletionResponseFormatType : IEquatable<ChatCompletionResponseFormatType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="ChatCompletionResponseFormatType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ChatCompletionResponseFormatType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string TextValue = "text";
        private const string JsonObjectValue = "jsonObject";
        private const string JsonSchemaValue = "jsonSchema";

        /// <summary> text. </summary>
        public static ChatCompletionResponseFormatType Text { get; } = new ChatCompletionResponseFormatType(TextValue);
        /// <summary> jsonObject. </summary>
        public static ChatCompletionResponseFormatType JsonObject { get; } = new ChatCompletionResponseFormatType(JsonObjectValue);
        /// <summary> jsonSchema. </summary>
        public static ChatCompletionResponseFormatType JsonSchema { get; } = new ChatCompletionResponseFormatType(JsonSchemaValue);
        /// <summary> Determines if two <see cref="ChatCompletionResponseFormatType"/> values are the same. </summary>
        public static bool operator ==(ChatCompletionResponseFormatType left, ChatCompletionResponseFormatType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ChatCompletionResponseFormatType"/> values are not the same. </summary>
        public static bool operator !=(ChatCompletionResponseFormatType left, ChatCompletionResponseFormatType right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="ChatCompletionResponseFormatType"/>. </summary>
        public static implicit operator ChatCompletionResponseFormatType(string value) => new ChatCompletionResponseFormatType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ChatCompletionResponseFormatType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ChatCompletionResponseFormatType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
