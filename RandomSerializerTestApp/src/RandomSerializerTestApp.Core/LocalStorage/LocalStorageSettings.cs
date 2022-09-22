namespace RandomSerializerTestApp.Core.LocalStorage;

public class LocalStorageSettings : ILocalStorageSettings
{
    public string DirectoryNameOfFiles { get; init; } = string.Empty;
}