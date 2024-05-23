```csharp
public class Embedding<T> 
{
    public ReadOnlyMemory<T> Vector { get; }
    //Other Embedding properties		
}
```

```csharp
public class EmbeddingClient<T> 
{
    public EmbeddingClient<T> GetEmbeddingClient<T>();
    //Other Embedding properties		
}

or 

public class EmbeddingClient<T> 
{
    public EmbeddingClient<float> GetFloatEmbeddingClient<T>();
    public EmbeddingClient<short> GetShortEmbeddingClient<T>();
    //Other Embedding properties		
}
```

public class EmbeddingClient<T> 
{
    public EmbeddingClient<T> GetEmbeddingClient<T>();
    //Other Embedding properties		
}

### Example
```csharp
private readonly EmbeddingClient<float> _embeddings = …;
private readonly List<(ReadOnlyMemory<float> Embedding, string Text)> _vectorDB = …;
…
ReadOnlyMemory<float> questionEmbedding = _embeddings.GetEmbedding(question);
var context = _vectorDB.OrderByDescending(e => TensorPrimitives.CosineSimilarity(questionEmbedding, e.Embedding))
                       .Take(3)
                       .Select(e => e.Text);
AddToPrompt(context);
```