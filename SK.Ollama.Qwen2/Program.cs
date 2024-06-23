using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using SK.Ollama.Qwen2;
using Microsoft.Extensions.DependencyInjection;

var builder = Kernel.CreateBuilder();
builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", new OllamaChatCompletionService());
var kernel = builder.Build();

string prompt = "你是谁？";
var response = await kernel.InvokePromptAsync(prompt);
Console.WriteLine(response.GetValue<string>());