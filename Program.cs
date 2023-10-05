using Azure;
using Azure.AI.OpenAI;
using DemoOpenAI.Services;
using Microsoft.Extensions.Configuration;


namespace DemoOpenAI
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                //Initialize confguration from appsettings.json
                var azureConfig = new AzureConfiguration();

                //Initialize OpenAI client with Ednpoint and Key
                var aoaiClient = new OpenAIClient(new Uri(azureConfig.AOAIEndpoint), new AzureKeyCredential(azureConfig.AOAIKey));

                //Initialize AzureOpenAIChat service with OpenAI client
                var azureOpenAIChat = new AzureOpenAIChat(aoaiClient);

                string userSelectionOption = string.Empty;
                string userPromptInput = string.Empty;

                //Provide options to proceed
                while (true)
                {
                    Console.WriteLine("Please select the option (type exit at any time to change option):");
                    Console.WriteLine("1. AI Assistant");
                    Console.WriteLine("2. AI Assistant with Streaming");
                    Console.WriteLine("3. AI Assistant with own data - eCommerce - GPT 3.5");
                    Console.WriteLine("4. AI Assistant with own data - eCommerce - GPT 3.5 (including Search result messages)");
                    Console.WriteLine("5. AI Assistant with own data - eCommerce - GPT 4 - Keyword Search");
                    Console.WriteLine("6. AI Assistant with own data - eCommerce - GPT 4 - Semantic Search");
                    Console.WriteLine("7. AI Assistant with own data - eCommerce Support - GPT 4 - Keyword Search");
                    Console.WriteLine("8. AI Assistant with own data - eCommerce Support - GPT 4 - Semantic Search");
                    Console.WriteLine("9. AI Assistant with own data - eCommerce Support - GPT 4 - Vector Embeddings");

                    userSelectionOption = Console.ReadLine();


                    switch (userSelectionOption)
                    {
                        case "1":
                            //Initialize chat completions with initial system prompt
                            azureOpenAIChat.InitializeChatColmpetions(PromptData.SystemPrompt_AIAssistant);
                            Console.WriteLine("I am an AI Assistant, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt
                                    List<ChatMessage> chatCompletionMessages = await azureOpenAIChat.GetChatCompletion(userPromptInput, azureConfig.AOAIDeploymentId);
                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "2":
                            //Initialize chat completions with initial system prompt
                            azureOpenAIChat.InitializeChatColmpetions(PromptData.SystemPrompt_AIAssistant);
                            Console.WriteLine("I am an AI Assistant with response streaming enabled, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt
                                    List<ChatMessage> chatCompletionMessagesStream = await azureOpenAIChat.GetChatCompletionStream(userPromptInput, azureConfig.AOAIDeploymentId);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "3":
                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexProducts,
                                true,20, AzureCognitiveSearchQueryType.Simple, "ProductID|ProductName|Description|CatalogDescription|ProductNumber|ProductModelName|StandardCost|Color|ListPrice|Size|Weight|Category",
                                "ProductName", "ProductName", "ProductName", azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4);
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt
                                    List<ChatMessage> chatCompletionMessagesOwnedData = await azureOpenAIChat.GetChatCompletionOwnData(userPromptInput, azureConfig.AOAIDeploymentId, false);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "4":
                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexProducts,
                                true,20, AzureCognitiveSearchQueryType.Simple, "ProductID|ProductName|Description|CatalogDescription|ProductNumber|ProductModelName|StandardCost|Color|ListPrice|Size|Weight|Category",
                                "ProductName", "ProductName", "ProductName", azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4);
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt
                                    List<ChatMessage> chatCompletionMessagesOwnedDataWithSearchResults = await azureOpenAIChat.GetChatCompletionOwnData(userPromptInput, azureConfig.AOAIDeploymentId, true);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "5":

                            aoaiClient = new OpenAIClient(new Uri(azureConfig.AOAIEndpointGPT4), new AzureKeyCredential(azureConfig.AOAIKeyGPT4));
                            azureOpenAIChat = new AzureOpenAIChat(aoaiClient);
                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexProducts,
                                true, 20, AzureCognitiveSearchQueryType.Simple, "ProductID|ProductName|Description|CatalogDescription|ProductNumber|ProductModelName|StandardCost|Color|ListPrice|Size|Weight|Category",
                                "ProductName", "ProductName", "ProductName", azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4);
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt and search results from Azure Cognitive Search with own data
                                    List<ChatMessage> chatCompletionMessagesOwnedDataWithSearchResults = await azureOpenAIChat.GetChatCompletionStream(userPromptInput, azureConfig.AOAIDeploymentIdGPT4);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "6":
                            //Initialize OpenAI client with Ednpoint and Key for GPT 4
                            aoaiClient = new OpenAIClient(new Uri(azureConfig.AOAIEndpointGPT4), new AzureKeyCredential(azureConfig.AOAIKeyGPT4));
                            azureOpenAIChat = new AzureOpenAIChat(aoaiClient);

                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexProducts,
                                true,20, AzureCognitiveSearchQueryType.Semantic, "ProductID|ProductName|Description|CatalogDescription|ProductNumber|ProductModelName|StandardCost|Color|ListPrice|Size|Weight|Category",
                                "ProductName", "ProductName", "ProductName",azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4, azureConfig.SearchSemanticConfiguration);
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt and search results from Azure Cognitive Search with own data
                                    List<ChatMessage> chatCompletionMessagesOwnedDataWithSearchResults = await azureOpenAIChat.GetChatCompletionStream(userPromptInput, azureConfig.AOAIDeploymentIdGPT4);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "7":
                            //Initialize OpenAI client with Ednpoint and Key for GPT 4
                            aoaiClient = new OpenAIClient(new Uri(azureConfig.AOAIEndpointGPT4), new AzureKeyCredential(azureConfig.AOAIKeyGPT4));
                            azureOpenAIChat = new AzureOpenAIChat(aoaiClient);

                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant_Support, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexSupport,
                                true,20, AzureCognitiveSearchQueryType.Semantic, "content|translated_text",
                                "metadata_title", "metadata_title", "metadata_title",azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4); 
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                //Get user input
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt and search results from Azure Cognitive Search with own data
                                    List<ChatMessage> chatCompletionMessagesOwnedDataWithSearchResults = await azureOpenAIChat.GetChatCompletionStream(userPromptInput, azureConfig.AOAIDeploymentIdGPT4);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "8":
                            //Initialize OpenAI client with Ednpoint and Key for GPT 4
                            aoaiClient = new OpenAIClient(new Uri(azureConfig.AOAIEndpointGPT4), new AzureKeyCredential(azureConfig.AOAIKeyGPT4));
                            azureOpenAIChat = new AzureOpenAIChat(aoaiClient);

                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant_Support, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexSupport,
                                true, 20, AzureCognitiveSearchQueryType.Semantic, "content",
                                "metadata_title", "metadata_title", "metadata_title", azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4, azureConfig.SearchSemanticConfiguration);
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt and search results from Azure Cognitive Search with own data
                                    List<ChatMessage> chatCompletionMessagesOwnedDataWithSearchResults = await azureOpenAIChat.GetChatCompletionStream(userPromptInput, azureConfig.AOAIDeploymentIdGPT4);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "9":
                            //Initialize OpenAI client with Ednpoint and Key for GPT 4
                            aoaiClient = new OpenAIClient(new Uri(azureConfig.AOAIEndpointGPT4), new AzureKeyCredential(azureConfig.AOAIKeyGPT4));
                            azureOpenAIChat = new AzureOpenAIChat(aoaiClient);

                            //Initialize chat completions with initial system prompt and Azure Cognitive Search with own data. Define also the search fields and Search query type
                            azureOpenAIChat.InitializeChatCompletionsOwnedData(PromptData.SystemPrompt_EcommerceAssistant_Support, azureConfig.SearchEndpoint, azureConfig.SearchKey, azureConfig.SearchIndexSupportVector,
                                true, 20, AzureCognitiveSearchQueryType.Vector, "content", "filepath", "title", "url", azureConfig.AOAIEmbeddingsEndpointGPT4, azureConfig.AOAIKeyGPT4);
                            Console.WriteLine("I am your e-Commerce assistant and my answers will be based your own data source, you can ask me anything (type exit at any time to change option).");
                            while (true)
                            {
                                Console.WriteLine();
                                userPromptInput = Console.ReadLine();

                                if (string.Compare(userPromptInput, "exit", true) != 0)
                                {
                                    //Get chat completion messages from Azure OpenAI based on user prompt and search results from Azure Cognitive Search with own data
                                    List<ChatMessage> chatCompletionMessagesOwnedDataWithSearchResults = await azureOpenAIChat.GetChatCompletionStream(userPromptInput, azureConfig.AOAIDeploymentIdGPT4);

                                    Console.WriteLine();
                                    Console.WriteLine("Do you have any other question? You can type exit at any time to change option");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}