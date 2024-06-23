namespace SK.Ollama.Qwen2.Blazor.Data
{
    public record Question
    {
        public string model { get; set; } = "qwen2";
        public string prompt { get; set; } = string.Empty;
        public bool stream { get; set; } = false;
    }
}
