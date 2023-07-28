"use client"
import { Breadcrumbs, PinList } from "@/src/app/components";
import React from "react";



function Conversation() {
  return (
    <div className="my-32 lg:w-900 md:w-10/12 sm:w-10/12 max-sm:w-11/12 mx-auto">
      <div className="flex flex-col justify-center">
        <Breadcrumbs
          items={[
            { name: "Media", href: "/media" },
            { name: "Conversations", href: "/conversations" },
          ]}
        />
        <div className="mt-10 mb-2">
          <PinList
            items={[
              { name: "All" },
              { name: "Names" },
              { name: "Japanese" },
              { name: "English" },
              { name: "Words" },
            ]}
            onToggle={(active:boolean, name:string, id?: string) => {
              console.log("")
            }}
          />
        </div>
        <div className="max-w-[700px] text-center">
          <p className="mb-6 font-bold text-primary dark:text-primary-400 text-left">
            Yuta Aoki - Vocabulary - 2020年6月1 日Level 4 試験どうだった
          </p>
        </div>
      </div>
      <div className="flex flex-col gap-8">
        <div className="flex flex-col">
          <h4 className="font-bold">Description</h4>
          <span className="mt-2 ml-4">
            College friends (Yoko and Kazuki) talk about an exam.
          </span>
        </div>
        <div className="flex flex-col ">
          <h4 className="font-bold">Dialogue</h4>
          <div className="flex flex-col pt-4 ml-3">
            <div className="flex group hover:bg-gray-100">
              <span className="px-1">やっと試験終わったね。</span>
              <div className="px-1 w-fit invisible group-hover:visible">
                (洋子)
              </div>
            </div>
            <div className="flex group hover:bg-gray-100">
              <span className="px-1 text-gray-500">
                Finally finished the exam, huh?
              </span>
              <div className="px-1 w-fit invisible group-hover:visible">
                (Yoko)
              </div>
            </div>
          </div>
          <div className="flex flex-col pt-4 ml-3">
            <div className="flex group hover:bg-gray-100">
              <span className="px-1">試験どうだった？</span>
              <div className="px-1 w-fit invisible group-hover:visible">
                (和樹)
              </div>
            </div>
            <div className="flex group hover:bg-gray-100">
              <span className="px-1 text-gray-500">
                Yeah. How was the exam?
              </span>
              <div className="px-1 w-fit invisible group-hover:visible">
                (Kazuki)
              </div>
            </div>
          </div>
          <div className="flex flex-col pt-4 ml-3">
            <div className="flex group hover:bg-gray-100">
              <span className="px-1">たぶん大丈夫だと思う。</span>
              <div className="px-1 w-fit invisible group-hover:visible">
                (洋子)
              </div>
            </div>
            <div className="flex group hover:bg-gray-100">
              <span className="px-1 text-gray-500">I think I did okay.</span>
              <div className="px-1 w-fit invisible group-hover:visible">
                (Yoko)
              </div>
            </div>
          </div>
        </div>
        <div className="flex flex-col ">
          <h4 className="font-bold">Words</h4>
          <div className="flex flex-col py-4 ml-3">
            <span className="hover:bg-gray-100 px-1">
              試験 - exam, test, trial
            </span>
            <span className="hover:bg-gray-100 px-1">
              試験する - to examine
            </span>
          </div>
        </div>
        <audio className="" controls>
          <source
            src="../../../audio/2020年6月1日 Level 4 試験どうだった？.mp3"
            type="audio/mpeg"
          />
          Your browser does not support the audio element.
        </audio>
      </div>
    </div>
  );
}

export default Conversation;
