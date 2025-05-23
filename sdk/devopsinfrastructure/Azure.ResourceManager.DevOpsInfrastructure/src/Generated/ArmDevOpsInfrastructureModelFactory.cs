// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Core;
using Azure.ResourceManager.Models;

namespace Azure.ResourceManager.DevOpsInfrastructure.Models
{
    /// <summary> Model factory for models. </summary>
    public static partial class ArmDevOpsInfrastructureModelFactory
    {
        /// <summary> Initializes a new instance of <see cref="DevOpsInfrastructure.DevOpsPoolData"/>. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="tags"> The tags. </param>
        /// <param name="location"> The location. </param>
        /// <param name="properties"> The resource-specific properties for this resource. </param>
        /// <param name="identity"> The managed service identities assigned to this resource. </param>
        /// <returns> A new <see cref="DevOpsInfrastructure.DevOpsPoolData"/> instance for mocking. </returns>
        public static DevOpsPoolData DevOpsPoolData(ResourceIdentifier id = null, string name = null, ResourceType resourceType = default, SystemData systemData = null, IDictionary<string, string> tags = null, AzureLocation location = default, DevOpsPoolProperties properties = null, ManagedServiceIdentity identity = null)
        {
            tags ??= new Dictionary<string, string>();

            return new DevOpsPoolData(
                id,
                name,
                resourceType,
                systemData,
                tags,
                location,
                properties,
                identity,
                serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsResourceDetails"/>. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="properties"> The resource-specific properties for this resource. </param>
        /// <returns> A new <see cref="Models.DevOpsResourceDetails"/> instance for mocking. </returns>
        public static DevOpsResourceDetails DevOpsResourceDetails(ResourceIdentifier id = null, string name = null, ResourceType resourceType = default, SystemData systemData = null, DevOpsResourceDetailsProperties properties = null)
        {
            return new DevOpsResourceDetails(
                id,
                name,
                resourceType,
                systemData,
                properties,
                serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsResourceDetailsProperties"/>. </summary>
        /// <param name="status"> The status of the resource. </param>
        /// <param name="image"> The image name of the resource. </param>
        /// <param name="imageVersion"> The version of the image running on the resource. </param>
        /// <returns> A new <see cref="Models.DevOpsResourceDetailsProperties"/> instance for mocking. </returns>
        public static DevOpsResourceDetailsProperties DevOpsResourceDetailsProperties(DevOpsResourceStatus status = default, string image = null, string imageVersion = null)
        {
            return new DevOpsResourceDetailsProperties(status, image, imageVersion, serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsResourceSku"/>. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="properties"> The resource-specific properties for this resource. </param>
        /// <returns> A new <see cref="Models.DevOpsResourceSku"/> instance for mocking. </returns>
        public static DevOpsResourceSku DevOpsResourceSku(ResourceIdentifier id = null, string name = null, ResourceType resourceType = default, SystemData systemData = null, DevOpsResourceSkuProperties properties = null)
        {
            return new DevOpsResourceSku(
                id,
                name,
                resourceType,
                systemData,
                properties,
                serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsResourceSkuProperties"/>. </summary>
        /// <param name="resourceType"> The type of resource the SKU applies to. </param>
        /// <param name="tier"> The tier of virtual machines in a scale set. </param>
        /// <param name="size"> The size of the SKU. </param>
        /// <param name="family"> The family of the SKU. </param>
        /// <param name="locations"> The set of locations that the SKU is available. </param>
        /// <param name="locationInfo"> A list of locations and availability zones in those locations where the SKU is available. </param>
        /// <param name="capabilities"> Name value pairs to describe the capability. </param>
        /// <param name="restrictions"> The restrictions of the SKU. </param>
        /// <returns> A new <see cref="Models.DevOpsResourceSkuProperties"/> instance for mocking. </returns>
        public static DevOpsResourceSkuProperties DevOpsResourceSkuProperties(string resourceType = null, string tier = null, string size = null, string family = null, IEnumerable<AzureLocation> locations = null, IEnumerable<ResourceSkuLocationInfo> locationInfo = null, IEnumerable<ResourceSkuCapabilities> capabilities = null, IEnumerable<ResourceSkuRestrictions> restrictions = null)
        {
            locations ??= new List<AzureLocation>();
            locationInfo ??= new List<ResourceSkuLocationInfo>();
            capabilities ??= new List<ResourceSkuCapabilities>();
            restrictions ??= new List<ResourceSkuRestrictions>();

            return new DevOpsResourceSkuProperties(
                resourceType,
                tier,
                size,
                family,
                locations?.ToList(),
                locationInfo?.ToList(),
                capabilities?.ToList(),
                restrictions?.ToList(),
                serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.ResourceSkuLocationInfo"/>. </summary>
        /// <param name="location"> Location of the SKU. </param>
        /// <param name="zones"> List of availability zones where the SKU is supported. </param>
        /// <param name="zoneDetails"> Gets details of capabilities available to a SKU in specific zones. </param>
        /// <returns> A new <see cref="Models.ResourceSkuLocationInfo"/> instance for mocking. </returns>
        public static ResourceSkuLocationInfo ResourceSkuLocationInfo(AzureLocation location = default, IEnumerable<string> zones = null, IEnumerable<ResourceSkuZoneDetails> zoneDetails = null)
        {
            zones ??= new List<string>();
            zoneDetails ??= new List<ResourceSkuZoneDetails>();

            return new ResourceSkuLocationInfo(location, zones?.ToList(), zoneDetails?.ToList(), serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.ResourceSkuZoneDetails"/>. </summary>
        /// <param name="name"> Gets the set of zones that the SKU is available in with the specified capabilities. </param>
        /// <param name="capabilities"> A list of capabilities that are available for the SKU in the specified list of zones. </param>
        /// <returns> A new <see cref="Models.ResourceSkuZoneDetails"/> instance for mocking. </returns>
        public static ResourceSkuZoneDetails ResourceSkuZoneDetails(IEnumerable<string> name = null, IEnumerable<ResourceSkuCapabilities> capabilities = null)
        {
            name ??= new List<string>();
            capabilities ??= new List<ResourceSkuCapabilities>();

            return new ResourceSkuZoneDetails(name?.ToList(), capabilities?.ToList(), serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.ResourceSkuCapabilities"/>. </summary>
        /// <param name="name"> The name of the SKU capability. </param>
        /// <param name="value"> The value of the SKU capability. </param>
        /// <returns> A new <see cref="Models.ResourceSkuCapabilities"/> instance for mocking. </returns>
        public static ResourceSkuCapabilities ResourceSkuCapabilities(string name = null, string value = null)
        {
            return new ResourceSkuCapabilities(name, value, serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.ResourceSkuRestrictions"/>. </summary>
        /// <param name="restrictionsType"> the type of restrictions. </param>
        /// <param name="values"> The value of restrictions. If the restriction type is set to location. This would be different locations where the SKU is restricted. </param>
        /// <param name="restrictionInfo"> The information about the restriction where the SKU cannot be used. </param>
        /// <param name="reasonCode"> the reason for restriction. </param>
        /// <returns> A new <see cref="Models.ResourceSkuRestrictions"/> instance for mocking. </returns>
        public static ResourceSkuRestrictions ResourceSkuRestrictions(ResourceSkuRestrictionsType? restrictionsType = null, IEnumerable<string> values = null, ResourceSkuRestrictionInfo restrictionInfo = null, ResourceSkuRestrictionsReasonCode? reasonCode = null)
        {
            values ??= new List<string>();

            return new ResourceSkuRestrictions(restrictionsType, values?.ToList(), restrictionInfo, reasonCode, serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.ResourceSkuRestrictionInfo"/>. </summary>
        /// <param name="locations"> Locations where the SKU is restricted. </param>
        /// <param name="zones"> List of availability zones where the SKU is restricted. </param>
        /// <returns> A new <see cref="Models.ResourceSkuRestrictionInfo"/> instance for mocking. </returns>
        public static ResourceSkuRestrictionInfo ResourceSkuRestrictionInfo(IEnumerable<AzureLocation> locations = null, IEnumerable<string> zones = null)
        {
            locations ??= new List<AzureLocation>();
            zones ??= new List<string>();

            return new ResourceSkuRestrictionInfo(locations?.ToList(), zones?.ToList(), serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsResourceQuota"/>. </summary>
        /// <param name="name"> The name of the quota. </param>
        /// <param name="id"> Fully qualified ARM resource id. </param>
        /// <param name="unit"> The unit of usage measurement. </param>
        /// <param name="currentValue"> The current usage of the resource. </param>
        /// <param name="limit"> The maximum permitted usage of the resource. </param>
        /// <returns> A new <see cref="Models.DevOpsResourceQuota"/> instance for mocking. </returns>
        public static DevOpsResourceQuota DevOpsResourceQuota(DevOpsResourceQuotaName name = null, ResourceIdentifier id = null, string unit = null, long currentValue = default, long limit = default)
        {
            return new DevOpsResourceQuota(
                name,
                id,
                unit,
                currentValue,
                limit,
                serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsResourceQuotaName"/>. </summary>
        /// <param name="value"> The name of the resource. </param>
        /// <param name="localizedValue"> The localized name of the resource. </param>
        /// <returns> A new <see cref="Models.DevOpsResourceQuotaName"/> instance for mocking. </returns>
        public static DevOpsResourceQuotaName DevOpsResourceQuotaName(string value = null, string localizedValue = null)
        {
            return new DevOpsResourceQuotaName(value, localizedValue, serializedAdditionalRawData: null);
        }

        /// <summary> Initializes a new instance of <see cref="Models.DevOpsImageVersion"/>. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="imageVersion"> The resource-specific properties for this resource. </param>
        /// <returns> A new <see cref="Models.DevOpsImageVersion"/> instance for mocking. </returns>
        public static DevOpsImageVersion DevOpsImageVersion(ResourceIdentifier id = null, string name = null, ResourceType resourceType = default, SystemData systemData = null, string imageVersion = null)
        {
            return new DevOpsImageVersion(
                id,
                name,
                resourceType,
                systemData,
                imageVersion != null ? new ImageVersionProperties(imageVersion, serializedAdditionalRawData: null) : null,
                serializedAdditionalRawData: null);
        }
    }
}
