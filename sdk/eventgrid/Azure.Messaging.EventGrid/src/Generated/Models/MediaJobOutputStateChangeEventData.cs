// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.Messaging.EventGrid.SystemEvents
{
    /// <summary> Schema of the Data property of an EventGridEvent for a Microsoft.Media.JobOutputStateChange event. </summary>
    public partial class MediaJobOutputStateChangeEventData
    {
        /// <summary> Initializes a new instance of MediaJobOutputStateChangeEventData. </summary>
        internal MediaJobOutputStateChangeEventData()
        {
            JobCorrelationData = new ChangeTrackingDictionary<string, string>();
        }

        /// <summary> Initializes a new instance of MediaJobOutputStateChangeEventData. </summary>
        /// <param name="previousState"> The previous state of the Job. </param>
        /// <param name="output"> Gets the output. </param>
        /// <param name="jobCorrelationData"> Gets the Job correlation data. </param>
        internal MediaJobOutputStateChangeEventData(MediaJobState? previousState, MediaJobOutput output, IReadOnlyDictionary<string, string> jobCorrelationData)
        {
            PreviousState = previousState;
            Output = output;
            JobCorrelationData = jobCorrelationData;
        }

        /// <summary> The previous state of the Job. </summary>
        public MediaJobState? PreviousState { get; }
        /// <summary> Gets the output. </summary>
        public MediaJobOutput Output { get; }
        /// <summary> Gets the Job correlation data. </summary>
        public IReadOnlyDictionary<string, string> JobCorrelationData { get; }
    }
}
