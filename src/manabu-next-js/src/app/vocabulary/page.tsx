import React from "react";
import { Grid } from "../components";

function Vocabulary() {
  return (
    <>
      <div className="flex justify-center mb-12 mt-32">
        <div className="max-w-[700px] text-center">
          <p className="mb-6 font-bold uppercase text-primary dark:text-primary-400">
            Vocabulary by Linguistics
          </p>
        </div>
      </div>
      <Grid
        items={[
          {
            name: "Particles",
            description:
              "助詞 - Words that do not have lexical meaning, but provide grammatical information.",
          },
          {
            name: "Pronouns",
            description:
              "代名詞 - Words used instead of nouns to avoid repetition or for emphasis.",
          },
          {
            name: "Adjectives",
            description: "形容詞 - Words that describe or modify nouns.",
          },
          {
            name: "Verbs",
            description:
              "動詞 - Words that express actions, occurrences, or states of being.",
          },
          {
            name: "Nouns",
            description:
              "名詞 - Words that represent people, places, things, or ideas.",
          },
          {
            name: "Conjunctions",
            description:
              "接続詞 - Words that connect words, phrases, clauses, or sentences.",
          },
          {
            name: "Auxiliary Verbs",
            description:
              "助動詞 - Words that accompany main verbs to express tense, mood, etc.",
          },
          {
            name: "Numerals",
            description: "数字 - Words that represent numbers.",
          },
          {
            name: "Prepositions",
            description:
              "前置詞 - Words that indicate relationships between other words.",
          },
          {
            name: "Classifiers",
            description: "類別詞 - Words used to categorize or count nouns.",
          },
          {
            name: "Adverbs",
            description:
              "副詞 - Words that modify verbs, adjectives, or other adverbs.",
          },
          {
            name: "Interjections",
            description:
              "相槌 - Words or phrases used to express emotions or reactions.",
          },
          {
            name: "Names",
            description:
              "名前 - Words that identify specific people, places, or things.",
          },
        ]}
      />
    </>
  );
}

export default Vocabulary;
