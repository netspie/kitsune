namespace Manabu.UI.Common.Storage;

public interface IStorage
{
    Task Save<T>(T @object);
    Task<T> Get<T>();
    Task Delete<T>();
}
