﻿namespace Manabu.Entities.Shared;

public class LearningObjectType
{
    public string Value { get; }

    public LearningObjectType(string value) =>
        Value = value;

    public bool IsContainerItem() =>
        Value == LearningContainerType.Conversation.Value ||
        Value == LearningContainerType.Lesson.Value;

    public bool IsLearningItem() =>
        Value == LearningItemType.Phrase.Value ||
        Value == LearningItemType.WordMeaning.Value ||
        Value == LearningItemType.WordFormation.Value ||
        Value == LearningItemType.Kana.Value ||
        Value == LearningItemType.Kanji.Value;

    public bool IsProperty() =>
        Value == LearningPropertyType.Audio.Value ||
        Value == LearningPropertyType.Context.Value ||
        Value == LearningPropertyType.Original.Value ||
        Value == LearningPropertyType.Translation.Value;

    public override bool Equals(object obj)
    {
        if (obj is LearningObjectType other)
            return Value == other.Value;
        
        return false;
    }

    public override int GetHashCode() =>
        Value.GetHashCode();

    public static bool operator ==(LearningObjectType left, LearningObjectType right) =>
        left.Equals(right);

    public static bool operator !=(LearningObjectType left, LearningObjectType right) =>
        !(left == right);
}

public class LearningContainerType : LearningObjectType
{
    public static readonly LearningContainerType Conversation = new LearningContainerType("conversation");
    public static readonly LearningContainerType Lesson = new LearningContainerType("lesson");

    public LearningContainerType(string value) : base(value)
    {
    }
}

public class LearningPropertyType : LearningObjectType
{
    public static readonly LearningPropertyType Audio = new LearningPropertyType("audio");
    public static readonly LearningPropertyType Context = new LearningPropertyType("context");
    public static readonly LearningPropertyType Original = new LearningPropertyType("original");
    public static readonly LearningPropertyType Translation = new LearningPropertyType("translation");

    public LearningPropertyType(string value) : base(value)
    {
    }
}

public class LearningItemType : LearningObjectType
{
    public static readonly LearningItemType Phrase = new LearningItemType("phrase");
    public static readonly LearningItemType Word = new LearningItemType("word");
    public static readonly LearningItemType WordMeaning = new LearningItemType("word-meaning");
    public static readonly LearningItemType WordFormation = new LearningItemType("word-formation");
    public static readonly LearningItemType Radical = new LearningItemType("radical");
    public static readonly LearningItemType Kana = new LearningItemType("kana");
    public static readonly LearningItemType Kanji = new LearningItemType("kanji");

    public LearningItemType(string value) : base(value)
    {
    }
}