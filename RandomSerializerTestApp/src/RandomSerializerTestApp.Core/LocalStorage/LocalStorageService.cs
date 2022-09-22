using System.Text.Json;

namespace RandomSerializerTestApp.Core.LocalStorage;

public class LocalStorageService : ILocalStorageService
{
    private readonly ILocalStorageSettings _localStorageSettings;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public LocalStorageService(ILocalStorageSettings localStorageSettings)
	{
        _localStorageSettings = localStorageSettings;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<IEnumerable<T>> Get<T>(string fileNmae)
    {
        this.IfNameIsInvalidThrowAnException(fileNmae);
        var fullFileName = this.CombineFullFileName(fileNmae);

        if (!File.Exists(fullFileName))
            throw new FileNotFoundException(fullFileName);

        using var fileStream = File.OpenRead(fullFileName);
        var source = JsonSerializer.Deserialize<IEnumerable<T>>(fileStream, _jsonSerializerOptions);
        await fileStream.DisposeAsync();

        return source ?? Enumerable.Empty<T>();
    }

    public async Task Save<T>(string fileNmae, IEnumerable<T> source)
    {
        this.IfNameIsInvalidThrowAnException(fileNmae);

        using var fileStream = File.Create(this.CombineFullFileName(fileNmae));
        await JsonSerializer.SerializeAsync(fileStream, source, _jsonSerializerOptions);
        await fileStream.DisposeAsync();
    }

    private string CombineFullFileName(string fileNmae) =>
        Path.Combine(_localStorageSettings.DirectoryNameOfFiles, fileNmae);

    private void IfNameIsInvalidThrowAnException(string fileNmae)
    {
        if (string.IsNullOrWhiteSpace(fileNmae))
            throw new ArgumentException(null, nameof(fileNmae));
    }

}