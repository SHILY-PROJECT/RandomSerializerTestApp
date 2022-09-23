namespace RandomSerializerTestApp.Core.LocalStorage;

public class LocalStorageServiceSettings : ILocalStorageServiceSettings
{
    public string DirectoryNameOfFiles { get; init; } = string.Empty;
}