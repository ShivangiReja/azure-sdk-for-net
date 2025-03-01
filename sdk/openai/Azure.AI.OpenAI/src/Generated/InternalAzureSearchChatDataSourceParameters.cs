// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.AI.OpenAI.Chat
{
    /// <summary> The AzureSearchChatDataSourceParameters. </summary>
    internal partial class InternalAzureSearchChatDataSourceParameters
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
        internal IDictionary<string, BinaryData> SerializedAdditionalRawData { get; set; }
        /// <summary> Initializes a new instance of <see cref="InternalAzureSearchChatDataSourceParameters"/>. </summary>
        /// <param name="endpoint"> The absolute endpoint path for the Azure Search resource to use. </param>
        /// <param name="indexName"> The name of the index to use, as specified in the Azure Search resource. </param>
        /// <param name="authentication">
        /// The authentication mechanism to use with Azure Search.
        /// Please note <see cref="DataSourceAuthentication"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes..
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="endpoint"/>, <paramref name="indexName"/> or <paramref name="authentication"/> is null. </exception>
        public InternalAzureSearchChatDataSourceParameters(Uri endpoint, string indexName, DataSourceAuthentication authentication)
        {
            Argument.AssertNotNull(endpoint, nameof(endpoint));
            Argument.AssertNotNull(indexName, nameof(indexName));
            Argument.AssertNotNull(authentication, nameof(authentication));

            _internalIncludeContexts = new ChangeTrackingList<string>();
            Endpoint = endpoint;
            IndexName = indexName;
            Authentication = authentication;
        }

        /// <summary> Initializes a new instance of <see cref="InternalAzureSearchChatDataSourceParameters"/>. </summary>
        /// <param name="topNDocuments"> The configured number of documents to feature in the query. </param>
        /// <param name="inScope"> Whether queries should be restricted to use of the indexed data. </param>
        /// <param name="strictness">
        /// The configured strictness of the search relevance filtering.
        /// Higher strictness will increase precision but lower recall of the answer.
        /// </param>
        /// <param name="maxSearchQueries">
        /// The maximum number of rewritten queries that should be sent to the search provider for a single user message.
        /// By default, the system will make an automatic determination.
        /// </param>
        /// <param name="allowPartialResult">
        /// If set to true, the system will allow partial search results to be used and the request will fail if all
        /// partial queries fail. If not specified or specified as false, the request will fail if any search query fails.
        /// </param>
        /// <param name="internalIncludeContexts">
        /// The output context properties to include on the response.
        /// By default, citations and intent will be requested.
        /// </param>
        /// <param name="endpoint"> The absolute endpoint path for the Azure Search resource to use. </param>
        /// <param name="indexName"> The name of the index to use, as specified in the Azure Search resource. </param>
        /// <param name="authentication">
        /// The authentication mechanism to use with Azure Search.
        /// Please note <see cref="DataSourceAuthentication"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes..
        /// </param>
        /// <param name="fieldMappings"> The field mappings to use with the Azure Search resource. </param>
        /// <param name="queryType"> The query type for the Azure Search resource to use. </param>
        /// <param name="semanticConfiguration"> Additional semantic configuration for the query. </param>
        /// <param name="filter"> A filter to apply to the search. </param>
        /// <param name="vectorizationSource">
        /// The vectorization source to use with Azure Search.
        /// Supported sources for Azure Search include endpoint, deployment name, and integrated.
        /// Please note <see cref="DataSourceVectorizer"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes..
        /// </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal InternalAzureSearchChatDataSourceParameters(int? topNDocuments, bool? inScope, int? strictness, int? maxSearchQueries, bool? allowPartialResult, IList<string> internalIncludeContexts, Uri endpoint, string indexName, DataSourceAuthentication authentication, DataSourceFieldMappings fieldMappings, DataSourceQueryType? queryType, string semanticConfiguration, string filter, DataSourceVectorizer vectorizationSource, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            TopNDocuments = topNDocuments;
            InScope = inScope;
            Strictness = strictness;
            MaxSearchQueries = maxSearchQueries;
            AllowPartialResult = allowPartialResult;
            _internalIncludeContexts = internalIncludeContexts;
            Endpoint = endpoint;
            IndexName = indexName;
            Authentication = authentication;
            FieldMappings = fieldMappings;
            QueryType = queryType;
            SemanticConfiguration = semanticConfiguration;
            Filter = filter;
            VectorizationSource = vectorizationSource;
            SerializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> Initializes a new instance of <see cref="InternalAzureSearchChatDataSourceParameters"/> for deserialization. </summary>
        internal InternalAzureSearchChatDataSourceParameters()
        {
        }

        /// <summary> The configured number of documents to feature in the query. </summary>
        public int? TopNDocuments { get; set; }
        /// <summary> Whether queries should be restricted to use of the indexed data. </summary>
        public bool? InScope { get; set; }
        /// <summary>
        /// The configured strictness of the search relevance filtering.
        /// Higher strictness will increase precision but lower recall of the answer.
        /// </summary>
        public int? Strictness { get; set; }
        /// <summary>
        /// The maximum number of rewritten queries that should be sent to the search provider for a single user message.
        /// By default, the system will make an automatic determination.
        /// </summary>
        public int? MaxSearchQueries { get; set; }
        /// <summary>
        /// If set to true, the system will allow partial search results to be used and the request will fail if all
        /// partial queries fail. If not specified or specified as false, the request will fail if any search query fails.
        /// </summary>
        public bool? AllowPartialResult { get; set; }
        /// <summary> The absolute endpoint path for the Azure Search resource to use. </summary>
        internal Uri Endpoint { get; set; }
        /// <summary> The name of the index to use, as specified in the Azure Search resource. </summary>
        internal string IndexName { get; set; }
        /// <summary> Additional semantic configuration for the query. </summary>
        public string SemanticConfiguration { get; set; }
        /// <summary> A filter to apply to the search. </summary>
        public string Filter { get; set; }
    }
}
