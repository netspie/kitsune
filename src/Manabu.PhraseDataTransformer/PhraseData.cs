namespace LangGrinder.Infrastructure.Data
{
    public class PhrasesData
    {
        public PhraseData[] Phrases { get; set; }
    }

    public class PhraseData
    {
        public string Id { get; set; }
        public string OriginalPhrase { get; set; }
        public string TranslatedPhrase { get; set; }
        public GrammarData[] Grammars { get; set; }
    }
}
