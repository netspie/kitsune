namespace WanikaniTest.Models;

[Serializable]
public class VocabularyItemDTO
{
    public int Id { get; set; }
    public string Object { get; set; }
    public string Url { get; set; }
    public DateTime Data_Updated_At { get; set; }
    public VocabularyItemDataDTO Data { get; set; }
    public override string ToString() => Data.Characters;
}

[Serializable]
public class VocabularyItemDataDTO
{
    public DateTime Created_At { get; set; }
    public int Level { get; set; }
    public string Slug { get; set; }
    public string Hidden_At { get; set; }
    public string Document_Url { get; set; }
    public string Characters { get; set; }
    public VocabularyItemMeaningDTO[] Meanings { get; set; }
    public VocabularyItemAuxilaryMeaningDTO[] Auxiliary_Meanings { get; set; }
    public VocabularyItemReadingsDTO[] Readings { get; set; }
    public string[] Parts_Of_Speech { get; set; }
    public int[] Component_Subject_Ids { get; set; }
    public string Meaning_Mnemonic { get; set; }
    public string Reading_Mnemonic { get; set; }
    public SentenceAndTranslationDTO[] Context_Sentences { get; set; }
    public int Lesson_Position { get; set; }
    public int Spaced_Repetition_System_Id { get; set; }

}

[Serializable]
public class VocabularyItemMeaningDTO
{
    public string Meaning { get; set; }
    public bool Primary { get; set; }
    public bool Accepted_Answer { get; set; }
}

[Serializable]
public class VocabularyItemAuxilaryMeaningDTO
{
    public string Type { get; set; }
    public string Meaning { get; set; }
}

[Serializable]
public class VocabularyItemReadingsDTO
{
    public bool Primary { get; set; }
    public string Reading { get; set; }
    public bool Accepted_Answer { get; set; }
}

[Serializable]
public class SentenceAndTranslationDTO
{
    public string En { get; set; }
    public string Ja { get; set; }
}