// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.AI.TextAnalytics.Models
{
    /// <summary> The Components15Gvwi3SchemasTasksstatePropertiesTasksPropertiesEntityrecognitiontasksItemsAllof1. </summary>
    internal partial class EntityRecognitionTasksItemProperties
    {
        /// <summary> Initializes a new instance of EntityRecognitionTasksItemProperties. </summary>
        internal EntityRecognitionTasksItemProperties()
        {
        }

        /// <summary> Initializes a new instance of EntityRecognitionTasksItemProperties. </summary>
        /// <param name="results"> . </param>
        internal EntityRecognitionTasksItemProperties(EntitiesResult results)
        {
            Results = results;
        }

        public EntitiesResult Results { get; }
    }
}
