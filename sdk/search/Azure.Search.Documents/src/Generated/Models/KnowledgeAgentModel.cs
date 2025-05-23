// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Search.Documents.Indexes.Models
{
    /// <summary>
    /// Specifies the connection parameters for the model to use for query planning.
    /// Please note <see cref="KnowledgeAgentModel"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes.
    /// The available derived classes include <see cref="KnowledgeAgentAzureOpenAIModel"/>.
    /// </summary>
    public abstract partial class KnowledgeAgentModel
    {
        /// <summary> Initializes a new instance of <see cref="KnowledgeAgentModel"/>. </summary>
        protected KnowledgeAgentModel()
        {
        }

        /// <summary> Initializes a new instance of <see cref="KnowledgeAgentModel"/>. </summary>
        /// <param name="kind"> The type of AI model. </param>
        internal KnowledgeAgentModel(KnowledgeAgentModelKind kind)
        {
            Kind = kind;
        }

        /// <summary> The type of AI model. </summary>
        internal KnowledgeAgentModelKind Kind { get; set; }
    }
}
