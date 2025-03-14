// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable enable

using Azure.Provisioning.Primitives;
using System;

namespace Azure.Provisioning.EventGrid;

/// <summary>
/// NumberLessThanOrEquals Advanced Filter.
/// </summary>
public partial class NumberLessThanOrEqualsAdvancedFilter : AdvancedFilter
{
    /// <summary>
    /// The filter value.
    /// </summary>
    public BicepValue<double> Value 
    {
        get { Initialize(); return _value!; }
        set { Initialize(); _value!.Assign(value); }
    }
    private BicepValue<double>? _value;

    /// <summary>
    /// Creates a new NumberLessThanOrEqualsAdvancedFilter.
    /// </summary>
    public NumberLessThanOrEqualsAdvancedFilter() : base()
    {
    }

    /// <summary>
    /// Define all the provisionable properties of
    /// NumberLessThanOrEqualsAdvancedFilter.
    /// </summary>
    protected override void DefineProvisionableProperties()
    {
        base.DefineProvisionableProperties();
        DefineProperty<string>("operatorType", ["operatorType"], defaultValue: "NumberLessThanOrEquals");
        _value = DefineProperty<double>("Value", ["value"]);
    }
}
