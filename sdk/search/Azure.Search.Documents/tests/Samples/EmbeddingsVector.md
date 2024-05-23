## Introducing a new type for Embeddings

Currently, vector embeddings in Azure SDKs use the `ROM<float>` type. However, embeddings can be of narrower types such as `int8`, `int16`, and `float16`, which consume less memory space. The Azure Search service supports all these narrow types, enabling customers to manage larger vector datasets at a lower cost while maintaining fast search capabilities. To accommodate this enhancement, I was wondering if we can define a new type with convenience methods for converting embeddings to `int8`, `int16`, `float16`, `float32` and potentially more in the future. Since Azure OpenAI is not yet GA and we haven't released OpenAI, I believe we should prioritize making embeddings a first-class feature of public OpenAI work. 

This type would also enable users to seamlessly utilize models supporting narrower data types beyond just Azure OpenAI models, such as those offered by HuggingFace, Cohere, etc. For instance, an embedding model that supports `int8` embeddings can be found at https://cohere.com/blog/int8-binary-embeddings. Additionally, significant portions of the industry are moving towards narrow type embeddings, considering the cost reduction observed compared to the drop in MTEB as outlined in the experiments referenced [here](https://huggingface.co/blog/embedding-quantization#quantization-experiments).

## Embedding Vector Type

Here's the proposed type:

```csharp
public enum EmbeddingType
{
    Single,
 //   Half,
    Short,
    Byte
}

public partial struct EmbeddingVector
{
    public EmbeddingType Type;
    private object _vector;

    private EmbeddingVector(EmbeddingType type, object vector)
    {
        Type = type;
        _vector = vector;
    }

    // Initialization methods
    public static EmbeddingVector FromFloat32(ReadOnlyMemory<float> vector)
    {
        return new EmbeddingVector(EmbeddingType.Single, vector);
    }

    public static EmbeddingVector FromInt16(ReadOnlyMemory<short> vector)
    {
        return new EmbeddingVector(EmbeddingType.Short, vector);
    }

    public static EmbeddingVector FromInt8(ReadOnlyMemory<byte> vector)
    {
        return new EmbeddingVector(EmbeddingType.Byte, vector);
    }

    // Conversion methods
    public ReadOnlyMemory<float> ToFloat32Memory()
    {
        if (Type != EmbeddingType.Single)
            throw new InvalidOperationException("Cannot convert to ReadOnlyMemory<float>.");

        return (_vector as float[]) ?? ((ReadOnlyMemory<float>)_vector);
    }

    public ReadOnlyMemory<short> ToInt16Memory()
    {
        if (Type != EmbeddingType.Short)
            throw new InvalidOperationException("Cannot convert to ReadOnlyMemory<short>.");

        return (_vector as short[]) ?? ((ReadOnlyMemory<short>)_vector);
    }

    public ReadOnlyMemory<byte> ToInt8Memory()
    {
        if (Type != EmbeddingType.Byte)
            throw new InvalidOperationException("Cannot convert to ReadOnlyMemory<byte>.");

        return (_vector as byte[]) ?? ((ReadOnlyMemory<byte>)_vector);
    }

    // Additional convenience methods can be added in the future.
}
```

## Library to add this type
Now, the question arises: where should this type be located?

We could include this type in the BCL or System.Memory.Data or System.ClientModel, or somewhere else?

## Current Usage of Embedding Type in OpenAI

```csharp
public class Embedding 
{
    public ReadOnlyMemory<float> Vector { get; }
    //Other Embedding properties		
}
```

### Example
```csharp
EmbeddingClient embeddingClient = new("text-embedding-3-small", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

string description = "Best hotel in town if you like luxury hotels.";
Embedding embedding = embeddingClient.GenerateEmbedding(description);
ReadOnlyMemory<float> vector = embedding.Vector;
```

### Vector Search using OpenAI method to get embeddings

```csharp
EmbeddingClient embeddingClient = new("text-embedding-3-small", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

string description = "Best hotel in town if you like luxury hotels.";
Embedding embedding = embeddingClient.GenerateEmbedding(description);
ReadOnlyMemory<float> vector = embedding.Vector;

Uri endpoint = new Uri(Environment.GetEnvironmentVariable("SEARCH_ENDPOINT"));
AzureKeyCredential credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("SEARCH_API_KEY"));
SearchClient searchClient = new SearchClient(endpoint, "indexName", credential);

SearchResults<Hotel> response = searchClient.Search<Hotel>(new SearchOptions
{
    VectorSearch = new()
    {
        Queries = { new VectorizedQuery(vector) { KNearestNeighborsCount = 3, Fields = { "DescriptionVector" }}}
    }
});
```

##  Usage with the New Embedding Type in OpenAI

```csharp
public class Embedding 
{
    public EmbeddingVector Vector { get; }
    //Other Embedding properties		
}
```

### Example
```csharp
EmbeddingClient embeddingClient = new("text-embedding-3-small", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

string description = "Best hotel in town if you like luxury hotels.";
Embedding embedding = embeddingClient.GenerateEmbedding(description);
EmbeddingVector vector = embedding.Vector;
if (vector.Type == EmbeddingType.Single)
{
    ReadOnlyMemory<float> floatEmbeddings = vector.ToFloat32Memory();
}
```

### Vector Search using OpenAI method to get embeddings

```csharp
EmbeddingClient embeddingClient = new("text-embedding-3-small", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

string description = "Best hotel in town if you like luxury hotels.";
Embedding embedding = embeddingClient.GenerateEmbedding(description);
EmbeddingVector vector = embedding.Vector;

Uri endpoint = new Uri(Environment.GetEnvironmentVariable("SEARCH_ENDPOINT"));
AzureKeyCredential credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("SEARCH_API_KEY"));
SearchClient searchClient = new SearchClient(endpoint, "indexName", credential);

SearchResults<Hotel> response = searchClient.Search<Hotel>(new SearchOptions
{
    VectorSearch = new()
    {
        Queries = { new VectorizedQuery(vector) { KNearestNeighborsCount = 3, Fields = { "DescriptionVector" }}}
    }
});
```