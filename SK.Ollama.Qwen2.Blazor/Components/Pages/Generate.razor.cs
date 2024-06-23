using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SK.Ollama.Qwen2.Blazor.Components.Pages
{
    public partial class Generate
    {
        private readonly List<Data.Answer> AnswerList = [];

        private string ask = string.Empty;

        private bool loading = false;
        private readonly Kernel? kernel;

        [Inject]
        private IJSRuntime? JsRuntime { get; set; }

        public Generate()
        {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", new OllamaChatCompletionService());
            kernel = builder.Build();
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
