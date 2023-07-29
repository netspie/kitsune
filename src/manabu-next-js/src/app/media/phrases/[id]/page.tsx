"use client";
import { Breadcrumbs, PinList } from "@/src/app/components";
import React from "react";

function Phrase() {
  return (
    <>
      <div className="my-32 lg:w-900 md:w-10/12 sm:w-10/12 max-sm:w-11/12 mx-auto">
        <div className="flex flex-col justify-center">
          <Breadcrumbs
            items={[
              { name: "Media", href: "/media" },
              { name: "Phrases", href: "/phrases" },
            ]}
          />
          <div className="flex flex-col gap-2 mt-10 mb-2">
            <PinList items={[{ name: "Edit" }, { name: "Add To Practice" }]} />
            <PinList
              items={[
                { name: "All" },
                { name: "Names" },
                { name: "Japanese" },
                { name: "Furigana" },
                { name: "English" },
                { name: "Words" },
              ]}
              onToggle={(active: boolean, name: string, id?: string) => {
                console.log("");
              }}
            />
          </div>
        </div>
        <div className="flex flex-col gap-8">
          <div className="flex flex-col pt-4">
            <h4 className="font-bold mb-3">Phrase</h4>
            <div className="flex hover:bg-gray-100">
              <span className="px-0">やっと試験終わったね。</span>
              <div className="px-0 w-fit">(洋子)</div>
            </div>
            <div className="flex hover:bg-gray-100">
              <span className="px-0 text-gray-500">
                Finally finished the exam, huh?
              </span>
              <div className="px-0 w-fit">&nbsp;(Yoko)</div>
            </div>
          </div>
          <div className="flex flex-col">
            <h4 className="font-bold">Context</h4>
            <span className="mt-2 ml-0">
              College friends (Yoko and Kazuki) talk about an exam.
            </span>
          </div>
          <div className="flex flex-col">
            <h4 className="font-bold">Words</h4>
            <div className="flex flex-col py-4 ml-0">
              <span className="hover:bg-gray-100 px-0">
                試験 - exam, test, trial
              </span>
              <span className="hover:bg-gray-100 px-0">
                試験する - to examine
              </span>
            </div>
          </div>
          <div className="flex flex-col">
            <h4 className="font-bold">Kanji</h4>
            <div className="flex flex-col py-4 ml-0">
              <span className="hover:bg-gray-100 px-0">試 - something</span>
              <span className="hover:bg-gray-100 px-0">
                験 - something else
              </span>
            </div>
          </div>
          <div className="flex flex-col">
            <h4 className="font-bold mb-4">Audio</h4>
            <audio className="" controls>
              <source
                src="../../../audio/2020年6月1日 Level 4 試験どうだった？.mp3"
                type="audio/mpeg"
              />
              Your browser does not support the audio element.
            </audio>
          </div>
          <div className="max-w-[700px] text-center">
            <h4 className="font-bold mb-3 text-left">Referenced By</h4>
            <p className="p-0 text-primary dark:text-primary-400 text-left">
              Yuta Aoki - Vocabulary - 2020年6月1 日Level 4 試験どうだった
            </p>
            <p className="p-0  text-primary dark:text-primary-400 text-left">
              Yuta Aoki - Vocabulary - 2020年6月1 日Level 4 試験どうだった
            </p>
            <p className="p-0 text-primary dark:text-primary-400 text-left">
              Yuta Aoki - Vocabulary - 2020年6月1 日Level 4 試験どうだった
            </p>
          </div>
        </div>
      </div>
    </>
  );
}

export default Phrase;
