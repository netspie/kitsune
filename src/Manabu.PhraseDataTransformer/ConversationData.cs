namespace LangGrinder.Infrastructure.Data;

public class ConversationsData
{
    public ConversationData[] conversations { get; set; }
}

public class ConversationData
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public string Date { get; set; }
    public string[] PhraseIds { get; set; }
}
