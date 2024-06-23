using Microsoft.AspNetCore.Components;
using Markdig;

namespace SK.Ollama.Qwen2.Blazor.Data
{
    public record Answer
    {
        public string prompt { get; set; }
        public string model { get; set; }
        public string created_at { get; set; }
        public string response { get; set; }
        public MarkupString htmlData
        {
            get
            {
                return (MarkupString)Markdown.ToHtml(response);
            }
        }
        public bool done { get; set; }
    }
}
