using Corelibs.Basic.Repository;
using Manabu.Entities.Content.Conversations;
using Manabu.Entities.Content.Lessons;
using Manabu.Entities.Content.Phrases;
using Manabu.Entities.Content.Words;
using Manabu.Entities.Flashcards;
using System.Reflection;

namespace Manabu.UseCases.Content.Shared;

public static class RepositoryExtensions
{
    public static readonly Dictionary<LearningItemType, Type> ItemsPerTypes = new()
    {
        { LearningItemType.Conversation, typeof(Conversation) },
        { LearningItemType.Lesson, typeof(Lesson) },
        { LearningItemType.Phrase, typeof(Phrase) },
        { LearningItemType.Word, typeof(Word) },
    };

    public static IRepository? GetRepositoryOfItemType(this object instance, LearningItemType type) =>
        instance.GetAllRepositories().GetRepositoryOfItemType(type);

    public static IRepository? GetRepositoryOfItemType(this IRepository[] repositories, LearningItemType type)
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
