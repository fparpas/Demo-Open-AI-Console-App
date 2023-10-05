using Azure;
using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoOpenAI.Services
{
    // This class is the service that connects to the Azure OpenAI API
    internal class AzureOpenAIChat
    {
        OpenAIClient _client = null;

        public AzureOpenAIChat(OpenAIClient client)
        {
            _client = client;
        }

        public ChatCompletionsOptions ChatCompletions { get; set; }

        // This method initializes the chat completions with the initial system prompt that will set the behavior of the chat
        public void InitializeChatColmpetions(string message)
        {
            ChatCompletions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.System, message)
                },
                MaxTokens = 1000
            };
        }

        // This method initializes the chat completions with the initial system prompt that will set the behavior of the chat
        // It configures the Azure Cognitive Search extension that enables the search of documents in an search index of our own data
        public void InitializeChatCompletionsOwnedData(string message, string searchEndpoint, string searchKey, string searchIndex, bool includeOnlyOwnData, int documentCount, AzureCognitiveSearchQueryType queryType, string contentFields, string filePathField, string titleField, string urlField, string embeddingEndpoint, string embeddingKey, string semanticConfigurationName = "")
        {
            ChatCompletions = new ChatCompletionsOptions()
            {
                //Initialize initial system prompt
                Messages =
                {
                    new ChatMessage(ChatRole.System, message)
                },
                //Set the Azure Search extension options
                AzureExtensionsOptions = new AzureChatExtensionsOptions()
                {
                    Extensions =
                    {
                        new AzureCognitiveSearchChatExtensionConfiguration()
                        {
                            SearchEndpoint = new Uri(searchEndpoint),
                            SearchKey = new AzureKeyCredential(searchKey),
                            ShouldRestrictResultScope = includeOnlyOwnData,
                            QueryType =  queryType,
                            SemanticConfiguration = semanticConfigurationName,
                            DocumentCount=documentCount,
                            EmbeddingEndpoint = new Uri(embeddingEndpoint),
                            EmbeddingKey = new AzureKeyCredential(embeddingKey),
                            FieldMappingOptions = new AzureCognitiveSearchIndexFieldMappingOptions()
                            {
                                ContentFieldSeparator = contentFields,
                                FilepathFieldName = filePathField,
                                TitleFieldName = titleField,
                                UrlFieldName = urlField,                                                          
                            },
                            IndexName = searchIndex
                        },
                    }
                }
            };
        }
        // Call Open AI service to get the chat completion
        public async Task<List<ChatMessage>> GetChatCompletion(string question, string deploymentId)
        {
            //Add the user question to the chat completions
            ChatCompletions.Messages.Add(new ChatMessage(ChatRole.User, question));

            //Call the Open AI service
            var response = await _client.GetChatCompletionsAsync(deploymentOrModelName: deploymentId, ChatCompletions);

            var responseMessage = response.Value.Choices[0].Message;

            Console.WriteLine(responseMessage.Content);

            //Add the response to the chat completions
            ChatCompletions.Messages.Add(responseMessage);

            return ChatCompletions.Messages.ToList();

        }
        // Call Open AI service to get the chat completion with stream functionality enabled
        public async Task<List<ChatMessage>> GetChatCompletionStream(string question, string deploymentId)
        {
            //Add the user question to the chat completions
            ChatCompletions.Messages.Add(new ChatMessage(ChatRole.User, question));

            //Call the Open AI service
            var response = await _client.GetChatCompletionsStreamingAsync(deploymentOrModelName: deploymentId, ChatCompletions);
            
            //Get the streaming chat completions
            using StreamingChatCompletions streamingChatCompletions = response.Value;

            string resultMessage = string.Empty;

            //Get the streaming chat choices and output them on the console
            await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
            {
                await foreach (ChatMessage message in choice.GetMessageStreaming())
                {
                    resultMessage += message.Content;
                    Console.Write(message.Content);
                }
                Console.WriteLine();
            }

            //Add the response to the chat completions
            ChatCompletions.Messages.Add(new ChatMessage(ChatRole.System, resultMessage));

            return ChatCompletions.Messages.ToList();

        }
        // Call Open AI service to get the chat completion with Search extension enabled that will search in the Azure Cognitive Search index of our own data
        public async Task<List<ChatMessage>> GetChatCompletionOwnData(string question, string deploymentId, bool showSearchExtensionMessages)
        {
            //Add the user question to the chat completions
            ChatCompletions.Messages.Add(new ChatMessage(ChatRole.User, question));

            //Call the Open AI service
            var response = await _client.GetChatCompletionsAsync(deploymentOrModelName: deploymentId, ChatCompletions);

            //Get the response message
            ChatMessage responseMessage = response.Value.Choices[0].Message;

            //Add the response to the chat completions
            ChatCompletions.Messages.Add(responseMessage);

            Console.WriteLine(responseMessage.Content);

            //Output the search extension messages
            if (showSearchExtensionMessages)
            {
                foreach (ChatMessage contextMessage in responseMessage.AzureExtensionsContext.Messages)
                {
                    string contextContent = contextMessage.Content;
                    try
                    {
                        var contextMessageJson = JsonDocument.Parse(contextMessage.Content);
                        contextContent = JsonSerializer.Serialize(contextMessageJson, new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                        });
                    }
                    catch (JsonException)
                    { }
                    Console.WriteLine($"{contextMessage.Role}: {contextContent}");
                }
            }
            return ChatCompletions.Messages.ToList();
        }
    }
}
