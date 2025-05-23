// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;
using Azure.ResourceManager.Cdn.Models;
using Azure.ResourceManager.Models;

namespace Azure.ResourceManager.Cdn
{
    /// <summary>
    /// A class representing the FrontDoorSecurityPolicy data model.
    /// SecurityPolicy association for AzureFrontDoor profile
    /// Serialized Name: SecurityPolicy
    /// </summary>
    public partial class FrontDoorSecurityPolicyData : ResourceData
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

        /// <summary> Initializes a new instance of <see cref="FrontDoorSecurityPolicyData"/>. </summary>
        public FrontDoorSecurityPolicyData()
        {
        }

        /// <summary> Initializes a new instance of <see cref="FrontDoorSecurityPolicyData"/>. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="provisioningState">
        /// Provisioning status
        /// Serialized Name: SecurityPolicy.properties.provisioningState
        /// </param>
        /// <param name="deploymentStatus"> Serialized Name: SecurityPolicy.properties.deploymentStatus. </param>
        /// <param name="profileName">
        /// The name of the profile which holds the security policy.
        /// Serialized Name: SecurityPolicy.properties.profileName
        /// </param>
        /// <param name="properties">
        /// object which contains security policy parameters
        /// Serialized Name: SecurityPolicy.properties.parameters
        /// Please note <see cref="SecurityPolicyProperties"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes.
        /// The available derived classes include <see cref="SecurityPolicyWebApplicationFirewall"/>.
        /// </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal FrontDoorSecurityPolicyData(ResourceIdentifier id, string name, ResourceType resourceType, SystemData systemData, FrontDoorProvisioningState? provisioningState, FrontDoorDeploymentStatus? deploymentStatus, string profileName, SecurityPolicyProperties properties, IDictionary<string, BinaryData> serializedAdditionalRawData) : base(id, name, resourceType, systemData)
        {
            ProvisioningState = provisioningState;
            DeploymentStatus = deploymentStatus;
            ProfileName = profileName;
            Properties = properties;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary>
        /// Provisioning status
        /// Serialized Name: SecurityPolicy.properties.provisioningState
        /// </summary>
        public FrontDoorProvisioningState? ProvisioningState { get; }
        /// <summary> Serialized Name: SecurityPolicy.properties.deploymentStatus. </summary>
        public FrontDoorDeploymentStatus? DeploymentStatus { get; }
        /// <summary>
        /// The name of the profile which holds the security policy.
        /// Serialized Name: SecurityPolicy.properties.profileName
        /// </summary>
        public string ProfileName { get; }
        /// <summary>
        /// object which contains security policy parameters
        /// Serialized Name: SecurityPolicy.properties.parameters
        /// Please note <see cref="SecurityPolicyProperties"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes.
        /// The available derived classes include <see cref="SecurityPolicyWebApplicationFirewall"/>.
        /// </summary>
        public SecurityPolicyProperties Properties { get; set; }
    }
}
