// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.Elastic.Models
{
    /// <summary> Plan details of the monitor resource. </summary>
    public partial class ElasticPlanDetails
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
        private IDictionary<string, BinaryData> _serializedAdditionalRawData;

        /// <summary> Initializes a new instance of <see cref="ElasticPlanDetails"/>. </summary>
        public ElasticPlanDetails()
        {
        }

        /// <summary> Initializes a new instance of <see cref="ElasticPlanDetails"/>. </summary>
        /// <param name="offerId"> Offer ID of the plan. </param>
        /// <param name="publisherId"> Publisher ID of the plan. </param>
        /// <param name="termId"> Term ID of the plan. </param>
        /// <param name="planId"> Plan ID. </param>
        /// <param name="planName"> Plan Name. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal ElasticPlanDetails(string offerId, string publisherId, string termId, string planId, string planName, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            OfferId = offerId;
            PublisherId = publisherId;
            TermId = termId;
            PlanId = planId;
            PlanName = planName;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> Offer ID of the plan. </summary>
        public string OfferId { get; set; }
        /// <summary> Publisher ID of the plan. </summary>
        public string PublisherId { get; set; }
        /// <summary> Term ID of the plan. </summary>
        public string TermId { get; set; }
        /// <summary> Plan ID. </summary>
        public string PlanId { get; set; }
        /// <summary> Plan Name. </summary>
        public string PlanName { get; set; }
    }
}
