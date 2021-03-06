// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Compute.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Properties of the disk for which update is pending.
    /// </summary>
    public partial class PropertyUpdatesInProgress
    {
        /// <summary>
        /// Initializes a new instance of the PropertyUpdatesInProgress class.
        /// </summary>
        public PropertyUpdatesInProgress()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PropertyUpdatesInProgress class.
        /// </summary>
        /// <param name="targetTier">The target performance tier of the disk if
        /// a tier change operation is in progress.</param>
        public PropertyUpdatesInProgress(string targetTier = default(string))
        {
            TargetTier = targetTier;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the target performance tier of the disk if a tier
        /// change operation is in progress.
        /// </summary>
        [JsonProperty(PropertyName = "targetTier")]
        public string TargetTier { get; set; }

    }
}
