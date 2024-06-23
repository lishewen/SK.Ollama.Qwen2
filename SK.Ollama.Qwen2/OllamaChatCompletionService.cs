using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Ollama.Qwen2
{
    public class OllamaChatCompletionService(string ip) : IChatCompletionService
    {
        public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

        public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var ollama = new OllamaApiClient($"http://{ip}:11434", "qwen2");

            var chat = new Chat(ollama, _ => { });

            var lastMessage = chatHistory.LastOrDefault();

            string question = lastMessage?.Content!;
            var chatResponse = "";
            var history = (await chat.Send(question, CancellationToken.None)).ToArray();

            var last = history.Last();
            chatResponse = last.Content;

            return [new(AuthorRole.Assistant, chatResponse)];
        }


        public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
