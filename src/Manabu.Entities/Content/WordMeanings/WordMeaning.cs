﻿using Corelibs.Basic.DDD;
using Manabu.Entities.Shared;

namespace Manabu.Entities.Content.WordMeanings;

public class WordMeaning : Entity<WordMeaningId>, IAggregateRoot<WordMeaningId>
{
    public const string DefaultCollectionName = "wordMeanings";

    public string Original { get; private set; }
    public List<string> Translations { get; private set; }
    public string HiraganaWriting { get; private set; }
    public string PitchAccent { get; private set; }
    public bool? KanjiWritingPreffered { get; private set; }

    public WordMeaning(string name)
    {

    }

    public WordMeaning(
        WordMeaningId id,
        uint version,
        string original,
        List<string> translations) : base(id, version)
    {
        Original = original;
        Translations = translations;
    }
}

public class WordMeaningId : LearningObjectId { public WordMeaningId(string value) : base(value) {} }