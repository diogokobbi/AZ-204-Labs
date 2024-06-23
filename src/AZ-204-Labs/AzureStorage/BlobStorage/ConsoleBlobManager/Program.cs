using ConsoleBlobManager.AzureServices;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false)
                .Build();

Console.Write("Connection to azure storage");
for (int i = 0; i < 3; i++)
{
    Console.Write(".");
    Thread.Sleep(1000);
}
Console.WriteLine();

var _blobStorageService = new BlobStorageService(config);
var accountName = await _blobStorageService.GetAccountNameAsync();
await Console.Out.WriteLineAsync($"Connected to Azure Storage Account: " + accountName);

Console.WriteLine();

var containers = await _blobStorageService.GetContainersNamesAsync();
var selectedContainer = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Choose your container:")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more containers)[/]")
        .AddChoices(containers.ToArray())
    );

AnsiConsole.WriteLine($"Working with container: {selectedContainer}");

Console.ReadLine();