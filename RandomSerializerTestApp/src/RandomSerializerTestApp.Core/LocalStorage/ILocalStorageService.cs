namespace RandomSerializerTestApp.Core.LocalStorage;

public interface ILocalStorageService
{
    Task<IEnumerable<T>> Get<T>(string fileNmae);
    Task Save<T>(string fileNmae, IEnumerable<T> source);
}
