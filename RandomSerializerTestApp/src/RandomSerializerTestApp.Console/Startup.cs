using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomSerializerTestApp.Core.Configuration;
using RandomSerializerTestApp.Core.LocalStorage;
using RandomSerializerTestApp.Core.Persons;

namespace RandomSerializerTestApp.Core;

internal class Startup
{
    private const string PersonsFileName = "Persons.json";

    private readonly ILocalStorageService _localStorageService;
    private readonly IPersonGeneratorService _personGenerator;
    private readonly ILocalStorageServiceSettings _localStorageServiceSettings;

    public Startup(
        ILocalStorageService localStorageService,
        IPersonGeneratorService personGeneratorService,
        ILocalStorageServiceSettings localStorageServiceSettings)
    {
        _localStorageService = localStorageService;
        _personGenerator = personGeneratorService;
        _localStorageServiceSettings = localStorageServiceSettings;
    }

    public static async Task Main()
    {
        try
        {
            await CreateBuilder().Build().Services.GetRequiredService<Startup>().Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.ReadKey();
    }
    
    public async Task Start()
    {
        var fullFileNameWithPersons = Path.Combine(_localStorageServiceSettings.DirectoryNameOfFiles, PersonsFileName);

        // Generation persons
        Console.WriteLine("The process of generating realistic data may take some time, please wait...");
        var persons = await _personGenerator.GenerateAsync(10000);
        Console.WriteLine($"Number of persons generated: {persons.Count()}");

        // Saving persons to file
        await _localStorageService.Save(PersonsFileName, persons);
        Console.WriteLine($"File with persons saved: {fullFileNameWithPersons}");

        // Getting persons from file
        persons = await _localStorageService.Get<Person>(PersonsFileName);
        Console.WriteLine($"Persons received from the file: {fullFileNameWithPersons} | Count: {persons.Count()}");

        // Persons credit card count
        var cardCount = persons.SelectMany(p => p.CreditCardNumbers).Count();
        Console.WriteLine($"Persons credit card count: {cardCount}");

        // Average age of child
        var children = persons.SelectMany(p => p.Children).ToArray();
        var averageAgeOfChild = this.GetAgeOfChildInYears(children.Average(c => c.BirthDate));
        Console.WriteLine($"Average age of the child: ~{averageAgeOfChild} y.o.");
    }

    private static IHostBuilder CreateBuilder() => Host
        .CreateDefaultBuilder()
        .ConfigureServices(services => services.AddRandomSerializerTestApp());

    private long GetAgeOfChildInYears(double value)
    {
        var daysPerYear = 365;
        var currentDateInSeconds = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        return (long)TimeSpan.FromSeconds(currentDateInSeconds - value).TotalDays / daysPerYear;
    }

}