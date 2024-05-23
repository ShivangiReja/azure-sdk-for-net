# Azure SDK Code Generation for Data Plane

Run `dotnet build /t:GenerateCode` to generate code.

### AutoRest Configuration
> see https://aka.ms/autorest
``` yaml
input-file:
- https://github.com/Azure/azure-rest-api-specs/blob/9583ed6c26ce1f10bbea92346e28a46394a784b4/specification/appconfiguration/data-plane/Microsoft.AppConfiguration/stable/2023-11-01/appconfiguration.json
namespace: Azure.Data.AppConfiguration
title: ConfigurationClient
```

### Change Endpoint type to Uri
``` yaml
directive:
  from: swagger-document
  where: $["x-ms-parameterized-host"].parameters["0"]
  transform: $.format = "url"
  ```

### Relocate the Endpoint parameter

```yaml
directive:
  from: swagger-document
  where: $["x-ms-parameterized-host"].parameters["0"]
  transform: >
    $["x-ms-parameter-location"] = "client";
    $["x-ms-skip-url-encoding"] = true;
```

### Modify operation names
``` yaml
directive:
- rename-operation:
    from: PutKeyValue
    to: SetConfigurationSetting
- rename-operation:
    from: DeleteKeyValue
    to: DeleteConfigurationSetting
- rename-operation:
    from: GetKeyValue
    to: GetConfigurationSetting
- rename-operation:
    from: GetKeyValues
    to: GetConfigurationSettings
- rename-operation:
    from: PutLock
    to: CreateReadOnlyLock
- rename-operation:
    from: DeleteLock
    to: DeleteReadOnlyLock
- rename-operation:
    from: UpdateSnapshot
    to: UpdateSnapshotStatus
```

### Internalize protocol methods
``` yaml
directive:
  from: swagger-document
  where: $.paths.*.*
  transform: >
    $["x-accessibility"] = "internal"
```
