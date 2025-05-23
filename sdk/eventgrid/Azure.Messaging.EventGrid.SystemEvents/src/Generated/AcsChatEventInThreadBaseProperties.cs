// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.Messaging.EventGrid.SystemEvents
{
    /// <summary> Schema of common properties of all thread-level chat events. </summary>
    public partial class AcsChatEventInThreadBaseProperties
    {
        /// <summary>
        /// Keeps track of any properties unknown to the library.
        /// <para>
        /// To assign an object to the value of this property use <see cref="BinaryData.FromObjectAsJson{T}(T, System.Text.Json.JsonSerializerOptions?)"/>.
        /// </para>
        /// <para>
        /// To assign an already formatted json string to this property use <see cref="BinaryData.FromString(string)"/>.
        /// </para>
        /// <para>
        /// Examples:
        /// <list type="bullet">
        /// <item>
        /// <term>BinaryData.FromObjectAsJson("foo")</term>
        /// <description>Creates a payload of "foo".</description>
        /// </item>
        /// <item>
        /// <term>BinaryData.FromString("\"foo\"")</term>
        /// <description>Creates a payload of "foo".</description>
        /// </item>
        /// <item>
        /// <term>BinaryData.FromObjectAsJson(new { key = "value" })</term>
        /// <description>Creates a payload of { "key": "value" }.</description>
        /// </item>
        /// <item>
        /// <term>BinaryData.FromString("{\"key\": \"value\"}")</term>
        /// <description>Creates a payload of { "key": "value" }.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        private protected IDictionary<string, BinaryData> _serializedAdditionalRawData;

        /// <summary> Initializes a new instance of <see cref="AcsChatEventInThreadBaseProperties"/>. </summary>
        /// <param name="threadId"> The chat thread id. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="threadId"/> is null. </exception>
        internal AcsChatEventInThreadBaseProperties(string threadId)
        {
            Argument.AssertNotNull(threadId, nameof(threadId));

            ThreadId = threadId;
        }

        /// <summary> Initializes a new instance of <see cref="AcsChatEventInThreadBaseProperties"/>. </summary>
        /// <param name="transactionId"> The transaction id will be used as co-relation vector. </param>
        /// <param name="threadId"> The chat thread id. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal AcsChatEventInThreadBaseProperties(string transactionId, string threadId, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            TransactionId = transactionId;
            ThreadId = threadId;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> Initializes a new instance of <see cref="AcsChatEventInThreadBaseProperties"/> for deserialization. </summary>
        internal AcsChatEventInThreadBaseProperties()
        {
        }

        /// <summary> The transaction id will be used as co-relation vector. </summary>
        public string TransactionId { get; }
        /// <summary> The chat thread id. </summary>
        public string ThreadId { get; }
    }
}
