// See https://aka.ms/new-console-template for more information
using DoclogixTask;
using CommandLine;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using DoclogixTask.Interface;


var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(ILogsCollection), typeof(LogsCollection))
                .AddTransient(typeof(IQParser), typeof(QParser))
                .AddTransient(typeof(IFileImporter), typeof(FileImporter))
                .AddTransient(typeof(ISearcher), typeof(Searcher))
                .AddTransient(typeof(IExpressionBuilder), typeof(ExpressionBuilder))
                .AddSingleton<Application>(new Application())
                .BuildServiceProvider();

Application app = serviceProvider.GetRequiredService<Application>();
app.Start(serviceProvider);

public class Application
{
    public Application() 
    {
    }

    public void Start(IServiceProvider serviceProvider)
    {
        string[] line = ["--help"];
        while (true)
        {
            Parser.Default.ParseArguments<SearchOptions, ImportOptions>(line)
            .MapResult((SearchOptions searchOpt) => Search(searchOpt), (ImportOptions addOptions) => ImportFile(addOptions), errs => 1);
            line = Console.ReadLine().SplitArgs(false);
        }

        int Search(SearchOptions searchOption)
        {
            var searchResult = serviceProvider.GetRequiredService<ISearcher>().Search(searchOption.Query);
            if (searchResult == null)
            {
                return 0;
            }

            Console.WriteLine(JsonConvert.SerializeObject(searchResult, Formatting.Indented));

            return 0;
        }

        int ImportFile(ImportOptions importOptions)
        {
            var importer = serviceProvider.GetRequiredService<IFileImporter>();
            importer.ImportFile(importOptions.Path, importOptions.Severity);
            Console.WriteLine("File imported successfully");
            return 0;
        }
    }
}



