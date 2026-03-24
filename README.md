# Bcf-Client-CSharp

## Code Generation 

Models and API clients are generated from `bcf-openapi.yaml` (BCF API v3.0).
To regenerate:
```bash
java -jar openapi-generator-cli.jar generate `
  -i bcf-openapi.yaml `
  -g csharp `
  -o ./Generated `
  --additional-properties="packageName=BcfClient.Generated,targetFramework=net8.0,nullableReferenceTypes=true,library=httpclient"
```
