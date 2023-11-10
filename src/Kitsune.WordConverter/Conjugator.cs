using Manabu.Entities.Content.WordLexemes;
using Manabu.Entities.Content.Words;

namespace Kitsune.WordConverter;

public static class Conjugator
{
    private static Dictionary<char, string> GodanEnding_A_ConvertionChars = new()
    {
        { 'る', "ら" }, // なる
        { 'す', "さ" }, // 話す
        { 'む', "ま" }, // 飲む
        { 'ぶ', "ば" }, // 遊ぶ
        { 'ぬ', "な" }, // 死ぬ
        { 'く', "か" }, // きく
        { 'う', "わ" }, // 使う
        { 'つ', "た" }, // 待つ
    };

    private static Dictionary<char, string> GodanEnding_I_ConvertionChars = new()
    {
        { 'る', "り" },
        { 'す', "し" },
        { 'む', "み" },
        { 'ぶ', "び" },
        { 'ぬ', "に" },
        { 'く', "き" },
        { 'う', "い" },
        { 'つ', "ち" },
    };

    private static Dictionary<char, string> GodanEnding_Ta_ConvertionChars = new()
    {
        { 'る', "った" },
        { 'す', "した" },
        { 'む', "んだ" },
        { 'ぶ', "んだ" },
        { 'ぬ', "んだ" },
        { 'く', "いた" },
        { 'う', "った" },
        { 'つ', "った" },
    };

    private static Dictionary<char, string> GodanEnding_Te_ConvertionChars = new()
    {
        { 'る', "って" },
        { 'す', "して" },
        { 'む', "んで" },
        { 'ぶ', "んで" },
        { 'ぬ', "んで" },
        { 'く', "いて" },
        { 'う', "って" },
        { 'つ', "って" },
    };

    private static Dictionary<char, string> GodanEnding_E_ConvertionChars = new()
    {
        { 'る', "れ" },
        { 'す', "せ" },
        { 'む', "め" },
        { 'ぶ', "べ" },
        { 'ぬ', "ね" },
        { 'く', "け" },
        { 'う', "え" },
        { 'つ', "て" },
    };

    private static Dictionary<char, string> GodanEnding_O_ConvertionChars = new()
    {
        { 'る', "ろう" },
        { 'す', "そう" },
        { 'む', "もう" },
        { 'ぶ', "ぼう" },
        { 'ぬ', "のう" },
        { 'く', "こう" },
        { 'う', "おう" },
        { 'つ', "とう" },
    };

    public static List<WordInflectionPair> ConjugateIAdjective(string adjective)
    {
        var list = new List<WordInflectionPair>();

        list.Add(new(InflectionType.Present,
            Informal: new(
                Positive: new(adjective),
                Negative: new(adjective.TrimEnd('い') + "くない"))));

        list.Add(new(InflectionType.Past,
            Informal: new(
                Positive: new(adjective.TrimEnd('い') + "かった"),
                Negative: new(adjective.TrimEnd('い') + "くなかった"))));

        return list;
    }

    public static List<WordInflectionPair> ConjugateVerb(string verb, VerbConjugationType verbType)
    {
        if (verbType == VerbConjugationType.Godan)
            return ConjugateGodanVerb(verb);
        else if (verbType == VerbConjugationType.Ichidan)
            return ConjugateIchidanVerb(verb);

        return null;
    }

    private static List<WordInflectionPair> ConjugateGodanVerb(string verb)
    {
        var list = new List<WordInflectionPair>();

        list.Add(new(InflectionType.Present,
            Informal: new(
                Positive: new(verb),
                Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "ない")),
            Formal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "ます"),
                Negative: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "ません"))));

        list.Add(new(InflectionType.Past,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_Ta_ConvertionChars)),
                Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "なかった")),
            Formal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "ました"),
                Negative: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "ませんでした"))));

        list.Add(new(InflectionType.Te,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars)),
                Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "なくて"))));

        list.Add(new(InflectionType.Progressive,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いる"),
                Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いない")),
            Formal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "います"),
                Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いません"))));

        list.Add(new(InflectionType.ProgressiveColloquial,
           Informal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "る"),
               Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "ない")),
            Formal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "ます"),
               Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "ません"))));

        list.Add(new(InflectionType.ProgressivePast,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いた"),
                Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いなかった")),
            Formal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いました"),
                Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "いませんでした"))));

        list.Add(new(InflectionType.ProgressivePastColloquial,
           Informal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "た"),
               Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "なかった")),
           Formal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "ました"),
               Negative: new(verb.ToGodanEnd(GodanEnding_Te_ConvertionChars) + "ませんでした"))));

        list.Add(new(InflectionType.Potential,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_E_ConvertionChars) + "る"),
                Negative: new(verb.ToGodanEnd(GodanEnding_E_ConvertionChars) + "ない"))));

        list.Add(new(InflectionType.Passive,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "れる"),
                Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "れない"))));

        list.Add(new(InflectionType.Causative,
            Informal: new(
                Positive: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "せる"),
                Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "せない"))));

        list.Add(new(InflectionType.CausativePassive,
           Informal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "せられる"),
               Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "せられない"))));

        list.Add(new(InflectionType.Violitional,
           Informal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_O_ConvertionChars)),
               Negative: new(verb + "まい"))));

        list.Add(new(InflectionType.ConditionalReba,
          Informal: new(
              Positive: new(verb.ToGodanEnd(GodanEnding_E_ConvertionChars) + "ば"),
              Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "なければ"))));

        list.Add(new(InflectionType.ConditionalTara,
          Informal: new(
              Positive: new(verb.ToGodanEnd(GodanEnding_Ta_ConvertionChars) + "ら"),
              Negative: new(verb.ToGodanEnd(GodanEnding_A_ConvertionChars) + "なかったら"))));

        list.Add(new(InflectionType.Imperative,
           Informal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_E_ConvertionChars) + "せられる"),
               Negative: new(verb + "な"))));

        list.Add(new(InflectionType.Desire,
           Informal: new(
               Positive: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "たい"),
               Negative: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "たくない"))));

        list.Add(new(InflectionType.DesirePast,
          Informal: new(
              Positive: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "たかった"),
              Negative: new(verb.ToGodanEnd(GodanEnding_I_ConvertionChars) + "たくなかった"))));

        return list;
    }

    private static string ToGodanEnd(this string verb, IDictionary<char, string> dict)
    {
        var ending = verb.Last();
        if (!dict.TryGetValue(ending, out var convertedEnding))
            return null;

        return $"{verb.TrimEnd(ending)}{convertedEnding}";
    }

    private static List<WordInflectionPair> ConjugateIchidanVerb(string verb)
    {
        var list = new List<WordInflectionPair>();

        list.Add(new(InflectionType.Present,
            Informal: new(
                Positive: new(verb),
                Negative: new(verb.TrimEnd('る') + "ない")),
            Formal: new(
                Positive: new(verb.TrimEnd('る') + "ます"),
                Negative: new(verb.TrimEnd('る') + "ません"))));

        list.Add(new(InflectionType.Past,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "た"),
                Negative: new(verb.TrimEnd('る') + "なかった")),
            Formal: new(
                Positive: new(verb.TrimEnd('る') + "ました"),
                Negative: new(verb.TrimEnd('る') + "ませんでした"))));

        list.Add(new(InflectionType.Te,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "て"),
                Negative: new(verb.TrimEnd('る') + "なくて"))));

        list.Add(new(InflectionType.Progressive,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "ている"),
                Negative: new(verb.TrimEnd('る') + "ていない")),
            Formal: new(
                Positive: new(verb.TrimEnd('る') + "ています"),
                Negative: new(verb.TrimEnd('る') + "ていません"))));

        list.Add(new(InflectionType.ProgressiveColloquial,
           Informal: new(
               Positive: new(verb.TrimEnd('る') + "てる"),
               Negative: new(verb.TrimEnd('る') + "てない")),
            Formal: new(
               Positive: new(verb.TrimEnd('る') + "てます"),
               Negative: new(verb.TrimEnd('る') + "てません"))));

        list.Add(new(InflectionType.ProgressivePast,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "ていた"),
                Negative: new(verb.TrimEnd('る') + "ていなかった")),
            Formal: new(
                Positive: new(verb.TrimEnd('る') + "ていました"),
                Negative: new(verb.TrimEnd('る') + "ていませんでした"))));

        list.Add(new(InflectionType.ProgressivePastColloquial,
           Informal: new(
               Positive: new(verb.TrimEnd('る') + "てた"),
               Negative: new(verb.TrimEnd('る') + "てなかった")),
           Formal: new(
               Positive: new(verb.TrimEnd('る') + "てました"),
               Negative: new(verb.TrimEnd('る') + "てませんでした"))));

        list.Add(new(InflectionType.Potential,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "られる"),
                Negative: new(verb.TrimEnd('る') + "られない"))));

        list.Add(new(InflectionType.Passive,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "られる"),
                Negative: new(verb.TrimEnd('る') + "られない"))));

        list.Add(new(InflectionType.Causative,
            Informal: new(
                Positive: new(verb.TrimEnd('る') + "させる"),
                Negative: new(verb.TrimEnd('る') + "させない"))));

        list.Add(new(InflectionType.CausativePassive,
           Informal: new(
               Positive: new(verb.TrimEnd('る') + "させられる"),
               Negative: new(verb.TrimEnd('る') + "させられない"))));

        list.Add(new(InflectionType.Violitional,
           Informal: new(
               Positive: new(verb.TrimEnd('る') + "よう"),
               Negative: new(verb + "まい"))));

        list.Add(new(InflectionType.ConditionalReba,
          Informal: new(
              Positive: new(verb.TrimEnd('る') + "れば"),
              Negative: new(verb.TrimEnd('る') + "なければ"))));

        list.Add(new(InflectionType.ConditionalReba,
          Informal: new(
              Positive: new(verb.TrimEnd('る') + "たら"),
              Negative: new(verb.TrimEnd('る') + "なかったら"))));

        list.Add(new(InflectionType.Imperative,
           Informal: new(
               Positive: new(verb.TrimEnd('る') + "ろ"),
               Negative: new(verb.TrimEnd('る') + "るな"))));

        list.Add(new(InflectionType.Desire,
           Informal: new(
               Positive: new(verb.TrimEnd('る') + "たい"),
               Negative: new(verb.TrimEnd('る') + "たくない"))));

        list.Add(new(InflectionType.DesirePast,
          Informal: new(
              Positive: new(verb.TrimEnd('る') + "たかった"),
              Negative: new(verb.TrimEnd('る') + "たくなかった"))));

        return list;
    }
}
