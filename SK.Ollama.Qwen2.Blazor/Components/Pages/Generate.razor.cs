using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;

namespace SK.Ollama.Qwen2.Blazor.Components.Pages
{
    public partial class Generate
    {
        private const string OllamaIP = "10.147.17.66";
        private readonly List<Data.Answer> AnswerList = [];
        private string ask = string.Empty;
        private bool loading = true;
        private readonly Kernel? kernel;

        [Inject]
        private IJSRuntime? JsRuntime { get; set; }

        public Generate()
        {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", new OllamaChatCompletionService(OllamaIP));
            kernel = builder.Build();

            try
            {
                Ping myPing = new();
                PingReply reply = myPing.Send(OllamaIP, 1000);
                if (reply != null)
                {
                    if (reply.Status == IPStatus.Success)
                        loading = false;
                }
            }
            catch
            {
                Debug.WriteLine("An error occurred while pinging an IP");
            }
        }

        private async Task ProcessEnterKey(KeyboardEventArgs eventArgs)
        {
            if (eventArgs.Key == "Enter")        // fire on enter 
                await SendAsync(ask);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // 每次html页面渲染完, 就调用prism去渲染code标签
            await JsRuntime!.InvokeVoidAsync("Prism.highlightAll");
        }

        private async Task SendAsync(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Data.Answer Answer = new()
                {
                    done = false,
                    prompt = message
                };
                loading = true;

                if (kernel != null)
                {
                    var response = await kernel.InvokePromptAsync(Answer.prompt);

                    if (response != null)
                    {
                        Answer.response = response.GetValue<string>()!;
                        Answer.done = true;
                        AnswerList.Add(Answer);
                        loading = false;
                    }
                }

                ask = string.Empty;
            }
        }
    }
}
