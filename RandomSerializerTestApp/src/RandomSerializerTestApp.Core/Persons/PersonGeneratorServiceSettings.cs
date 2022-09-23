namespace RandomSerializerTestApp.Core.Persons;

public class PersonGeneratorServiceSettings : IPersonGeneratorServiceSettings
{
    public string DataLocalizationLanguage { get; init; } = "ru";
}
