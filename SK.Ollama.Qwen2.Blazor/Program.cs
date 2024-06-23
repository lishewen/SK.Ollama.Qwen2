using Microsoft.FluentUI.AspNetCore.Components;
using SK.Ollama.Qwen2.Blazor.Components;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Loopback, 5001);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
