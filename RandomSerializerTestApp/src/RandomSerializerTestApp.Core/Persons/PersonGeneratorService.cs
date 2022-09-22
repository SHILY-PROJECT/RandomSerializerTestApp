using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Extras;

namespace RandomSerializerTestApp.Core.Persons;

public class PersonGeneratorService : IPersonGeneratorService
{
    private const string Locale = "ru";

    private readonly Random _rnd = new();

    public async Task<IEnumerable<Person>> GenerateAsync(int numberOfPersonsToGenerate) =>
        await this.Generate(numberOfPersonsToGenerate);

    private async Task<IEnumerable<Person>> Generate(int numberOfPersonsToGenerate)
    {
        var childFaker = new Faker<Child>(Locale)
            .RuleFor(c => c.Id, f => f.IndexFaker)
            .RuleFor(c => c.Gender, f => f.PickRandom<Gender>())
            .RuleFor(c => c.FirstName, (f, c) => f.Name.FirstName((Name.Gender)c.Gender))
            .RuleFor(c => c.LastName, (f, c) => f.Name.LastName((Name.Gender)c.Gender));
        
        var personFaker = new Faker<Person>(Locale)
            .RuleFor(p => p.Id, f => f.IndexFaker)
            .RuleFor(p => p.TransportId, f => Guid.NewGuid())
            .RuleFor(p => p.CreditCardNumbers, f => Enumerable.Range(0, f.Random.Int(0, 3).OrDefault(f, f.Random.Float(0.15f, 0.30f))).Select(x => f.Finance.Random.ReplaceNumbers("4##############").AppendCheckDigit()).ToArray())
            .RuleFor(p => p.Age, f => this.GetRandomAgeUsingProbabilityPercentage())
            .RuleFor(p => p.BirthDate, (f, p) => new DateTimeOffset(this.ChangeYearByAge(f, p)).ToUnixTimeSeconds())
            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
            .RuleFor(p => p.SequenceId, f => f.IndexGlobal)
            .RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName((Name.Gender)p.Gender))
            .RuleFor(p => p.LastName, (f, p) => f.Name.LastName((Name.Gender)p.Gender))
            .RuleFor(p => p.Phones, f => Enumerable.Range(0, f.Random.Int(1, 2)).Select(x => f.Phone.PhoneNumber()).ToArray())
            .RuleFor(p => p.Salary, f => Math.Round(f.Random.Double(20000, 100000), 2, MidpointRounding.ToEven))
            .RuleFor(p => p.IsMarred, f => _rnd.NextDouble() <= 0.50)
            .RuleFor(p => p.Children, (f, p) => childFaker.GenerateBetween(0, p.IsMarred && p.Salary > 45000 ? 5 : 2).ToArray());
        
        var persons = personFaker.Generate(numberOfPersonsToGenerate);
        await Task.WhenAll(persons.Select(p => Task.Run(() => this.FixChildDateOfBirth(p))));

        return persons;
    }

    private DateTime ChangeYearByAge(Faker faker, Person person) => DateTime.Now
        .AddYears(-person.Age)
        .AddMonths(DateTime.Now.Month - faker.Person.DateOfBirth.Month)
        .AddDays(DateTime.Now.Day - faker.Person.DateOfBirth.Day);

    private int GetRandomAgeUsingProbabilityPercentage() => _rnd.NextDouble() switch
    {
        (< 0.20) => _rnd.Next(80, 100),
        (< 0.45) => _rnd.Next(60, 80),
        (< 0.55) => _rnd.Next(40, 60),
        (< 0.95) => _rnd.Next(18, 40),
        _ => _rnd.Next(18, 100)
    };

    private void FixChildDateOfBirth(Person person)
    {
        if (!person.Children.Any()) return;

        const int adulthoodAge = 18;
        const int ageAtWhichPeopleUsuallyStopHavingChildren = 45;

        var ageFromBeginningOfAdulthood = person.Age >= ageAtWhichPeopleUsuallyStopHavingChildren ?
            (ageAtWhichPeopleUsuallyStopHavingChildren - adulthoodAge) : (person.Age - adulthoodAge);
        
        var dateOfAdult = DateTimeOffset.FromUnixTimeSeconds(person.BirthDate).AddYears(adulthoodAge);
        var dateFromBeginningOfAdulthood = DateTimeOffset.FromUnixTimeSeconds(person.BirthDate).AddYears(ageFromBeginningOfAdulthood);

        var fromDate = TimeSpan.FromSeconds(dateOfAdult.ToUnixTimeSeconds());
        var toDate = TimeSpan.FromSeconds(dateFromBeginningOfAdulthood.ToUnixTimeSeconds());

        var сhildСounter = 0;
        Array.ForEach(person.Children, c =>
        {
            c.Id = сhildСounter++;
            c.BirthDate = (long)Math.Round(TimeSpan.FromMinutes(_rnd.NextDouble() * (toDate.TotalMinutes - fromDate.TotalMinutes) + fromDate.TotalMinutes).TotalSeconds, 0, MidpointRounding.ToEven);
        });
    }

}