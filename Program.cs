using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;


// genel bir host (barındırıcı) oluştur.
var builder = Host.CreateApplicationBuilder(args);

// MCP istemcileriyle daha iyi entegrasyon için loglamayı yapılandır.
builder.Logging.AddConsole(consoleLogOptions =>
{
consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// MCP sunucusunu kaydet ve stdio (standart giriş/çıkış) taşımasını kullanacak şekilde yapılandır.
// Derlemede (assembly) tanımlı araçları tara.
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

// Host'u oluştur ve çalıştır. Bu MCP sunucusunu başlatır.
await builder.Build().RunAsync();

// MCP araçlarını barındıracak statik sınıf tanımla.
[McpServerToolType]
public static class EchoTool
{
    // Gelen mesajı istemciye geri döndüren bir araç sun.
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"Hello from C#: {message}";

    // Gelen mesajı ters çevirip geri döndüren bir araç sun.
    [McpServerTool, Description("Echoes in reverse the message sent by the client.")]
    public static string ReverseEcho(string message) => new string(message.Reverse().ToArray());
}