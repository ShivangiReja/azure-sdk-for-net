namespace Azure.Search.Documents
{
    public partial class DocumentsClient
    {
        protected DocumentsClient() { }
        public DocumentsClient(string endpoint, Azure.Core.TokenCredential credential, Azure.Search.Documents.DocumentsClientOptions options = null) { }
        public virtual Azure.Core.Pipeline.HttpPipeline Pipeline { get { throw null; } }
    }
    public partial class DocumentsClientOptions : Azure.Core.ClientOptions
    {
        public DocumentsClientOptions(Azure.Search.Documents.Generated.DocumentsClientOptions.ServiceVersion version = Azure.Search.Documents.Generated.DocumentsClientOptions.ServiceVersion.V1_0_0) { }
        public enum ServiceVersion
        {
            V1_0_0 = 1,
        }
    }
}