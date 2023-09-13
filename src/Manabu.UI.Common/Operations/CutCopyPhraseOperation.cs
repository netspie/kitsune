using Manabu.UI.Common.Components;
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

    public Task Cut(string phraseId, string conversationId) =>
        _storage.Save(new CutPasteOperationData(phraseId, conversationId));

    public async Task Paste(string conversationId, Func<CutPasteOperationData, Task> action)
    {
        var data = await _storage.Get<CutPasteOperationData>();
        if (data is null)
            return;

        await _storage.Delete<CutPasteOperationData>();
        await action(data);
    }

    public async Task<bool> IsOngoing() => (await _storage.Get<CutPasteOperationData>()) is not null;
}

public class CutPasteOperationData
{
    public CutPasteOperationData(string phraseId, string currentConversationId)
    {
        CurrentConversationId = currentConversationId;
        PhraseId = phraseId;
    }

    public string CurrentConversationId { get; set; }
    public string PhraseId { get; set; }
}
