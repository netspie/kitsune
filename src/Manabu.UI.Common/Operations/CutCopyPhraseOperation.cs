using Manabu.UI.Common.Storage;

namespace Manabu.UI.Common.Operations;

public class CutCopyPhraseOperation
{
    private readonly IStorage _storage;

    public CutCopyPhraseOperation(
        IStorage storage)
    {
        _storage = storage;
    }

    public async Task Cut(string phraseId, string conversationId)
    {
        await _storage.Save(new CutPasteOperationData(phraseId, conversationId));
    }

    public async Task Cut(string[] phraseIds, string conversationId)
    {
        await _storage.Save(new CutPasteOperationData(phraseIds, conversationId));
    }
    public async Task<bool> Paste(Func<CutPasteOperationData, Task<bool>> action)
    {
        var data = await _storage.Get<CutPasteOperationData>();
        if (data is null)
            return false;

        await _storage.Delete<CutPasteOperationData>();
        var result = await action(data);

        return result;
    }

    public async Task<bool> IsOngoing() => (await _storage.Get<CutPasteOperationData>()) is not null;
}

public class CutPasteOperationData
{
    public CutPasteOperationData() {}
    public CutPasteOperationData(string phraseId, string currentConversationId)
    {
        CurrentConversationId = currentConversationId;
        PhraseIds = new[] { phraseId };
    }

    public CutPasteOperationData(string[] phraseId, string currentConversationId)
    {
        CurrentConversationId = currentConversationId;
        PhraseIds = phraseId;
    }

    public string CurrentConversationId { get; set; }
    public string[] PhraseIds { get; set; }
}
