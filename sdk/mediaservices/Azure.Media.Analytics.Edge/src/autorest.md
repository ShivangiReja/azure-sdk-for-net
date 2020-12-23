# Generated code configuration

Run `dotnet build /t:GenerateCode` to generate code.


``` yaml
input-file:
- .\swagger\LiveVideoAnalytics.json
- .\swagger\LiveVideoAnalyticsSdkDefinitions.json
```

```yaml
directive:
- from: swagger-document
  where: $.definitions.*
  transform: >
    $["x-csharp-usage"] = "model,input,output";
    $["x-csharp-formats"] = "json";
```