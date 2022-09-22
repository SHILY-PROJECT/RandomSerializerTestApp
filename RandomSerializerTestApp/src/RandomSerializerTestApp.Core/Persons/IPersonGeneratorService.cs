namespace RandomSerializerTestApp.Core.Persons;

public interface IPersonGeneratorService
{
    Task<IEnumerable<Person>> GenerateAsync(int numberOfPersonsToGenerate);
}