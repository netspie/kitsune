﻿using Corelibs.Basic.DDD;
using Manabu.Entities.Content.Courses;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.Authors;

public class Author : Entity<AuthorId>, IAggregateRoot<AuthorId>
{
    public const string DefaultCollectionName = "authors";

    public string Name { get; private set; }
    public List<CourseId> Courses { get; private set; }

    public Author(string name)
    {
        Name = name;
    }

    public Author(
        AuthorId id,
        uint version,
        string name,
        List<CourseId> courses) : base(id, version)
    {
        Name = name;
        Courses = courses;
    }
}

public class AuthorId : EntityId { public AuthorId(string value) : base(value) {} }
