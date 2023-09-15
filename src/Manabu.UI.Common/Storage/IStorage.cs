namespace Manabu.UI.Common.Storage;

public interface IStorage
{
    Task Save<T>(T @object);
    Task Save(object @object, Type type);
    Task<T> Get<T>();
    Task<object> Get(Type type);
    Task Delete<T>();
}
