using Corelibs.Basic.Repository;
using Manabu.Entities.Conversations;
using Manabu.Entities.Flashcards;
using Manabu.Entities.Lessons;
using Manabu.Entities.Phrases;
using Manabu.Entities.Words;
using System.Reflection;

namespace Manabu.UseCases.Shared;

public static class RepositoryExtensions
{
    public static readonly Dictionary<ItemType, Type> ItemsPerTypes = new()
    {
        { ItemType.Conversation, typeof(Conversation) },
        { ItemType.Lesson, typeof(Lesson) },
        { ItemType.Phrase, typeof(Phrase) },
        { ItemType.Word, typeof(Word) },
    };

    public static IRepository? GetRepositoryOfItemType(this object instance, ItemType type) =>
        instance.GetAllRepositories().GetRepositoryOfItemType(type);

    public static IRepository? GetRepositoryOfItemType(this IRepository[] repositories, ItemType type)
    {
        foreach (var repository in repositories)
        {
            Type repositoryGenericType = repository.GetType().GetGenericArguments().FirstOrDefault();

            if (repositoryGenericType is not null)
                if (ItemsPerTypes.TryGetValue(type, out Type targetType) && targetType == repositoryGenericType)
                    return repository;
        }

        return null;
    }

    public static IRepository[] GetAllRepositories(this object instance)
    {
        var type = instance.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        var repositories = fields
            .Where(f => f.FieldType.IsGenericType &&
                        f.FieldType.GetGenericTypeDefinition() == typeof(IRepository<,>))
            .Select(f => (IRepository)f.GetValue(instance))
            .ToArray();

        return repositories;
    }
}
