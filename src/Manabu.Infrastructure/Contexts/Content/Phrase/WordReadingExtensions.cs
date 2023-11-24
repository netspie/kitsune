namespace Manabu.Infrastructure.Contexts.Content.Phrases;

class Item(string kanji)
{
    public string Key { get; set; } = kanji;
    public string Kana { get; set; }
}

public enum WritingType
{
    Default,
    Hiragana,
    Katakana,
}

public static class WordReadingExtensions
{
    private const int HiraganaStart = 0x3040;
    private const int HiraganaEnd = 0x309f;

    private const int KatakanaStart = 0x30a0;
    private const int KatakanaEnd = 0x30ff;

    public static string Get(
        this string reading, 
        string originalForm, 
        string conjugatedForm,
        WritingType writingType = WritingType.Default)
    {
        // originalForm - 上が上る
        // conjugatedForm - 上が上らない
        // reading - あいがある

        var list = new List<Item>();
        while (originalForm.Length > 0)
        {
            if (IsKanji(originalForm[0]))
            {
                var s = new string(originalForm.TakeWhile(IsKanji).ToArray());
                list.Add(new Item(s));
                originalForm = new string(originalForm.Skip(s.Length).ToArray());
            }
            else
            {
                var s = new string(originalForm.TakeWhile(IsHiragana).ToArray());
                list.Add(new Item(s)
                {
                    Kana = s
                });
                originalForm = new string(originalForm.Skip(s.Length).ToArray());
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsKanji(list[i].Key[0]))
            {
                reading = new string(reading.Skip(list[i].Kana.Length).ToArray());
                continue;
            }

            if (i == list.Count - 1)
            {
                list[i].Kana = new string(reading.ToArray());
                reading = "";
                break;
            }

            var nextValueStart = list[i + 1].Key[0];
            list[i].Kana = new string(reading.TakeWhile(c => c != nextValueStart).ToArray());
            reading = new string(reading.Skip(list[i].Kana.Length).ToArray());
        }

        string result = "";

        for (int i = 0; i < list.Count; i++)
        {
            if (conjugatedForm.StartsWith(list[i].Key))
            {
                result += list[i].Kana;
                conjugatedForm = new string(conjugatedForm.Skip(list[i].Key.Length).ToArray());
            }
            else
            {
                result += conjugatedForm;
                break;
            }
        }

        return result;

        static bool IsKanji(char c) => c < HiraganaStart || c > HiraganaEnd;
        static bool IsHiragana(char c) => !IsKanji(c);
    }
}
