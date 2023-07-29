import React from "react";
import { Breadcrumbs, Input, PinList } from "@/src/app/components";

function PhraseEdit() {
  return (
    <div className="my-32 lg:w-900 md:w-10/12 sm:w-10/12 max-sm:w-11/12 mx-auto">
      <div className="flex flex-col justify-center">
        <Breadcrumbs
          items={[
            { name: "Media", href: "/media" },
            { name: "Phrases", href: "/phrases" },
          ]}
        />
      </div>
      <div className="flex flex-col gap-8">
        <div className="flex flex-col pt-4">
          <h4 className="font-bold mb-3">Phrase</h4>
          <div className="flex flex-col">
            <h5 className="mb-3 text-gray-500">Japanese</h5>
            <div className="flex gap-2">
              <Input value="やっと試験終わったね。" label="Phrase" />
              <Input value="洋子" label="Name" />
            </div>
          </div>
          <div className="flex flex-col">
            <h5 className="mb-3 text-gray-500">English</h5>
            <div className="flex gap-2">
              <Input value="Finally finished the exam, huh? " label="Phrase" />
              <Input value="Yoko" label="Name" />
            </div>
          </div>
        </div>
        <div className="flex flex-col pt-4">
          <h4 className="font-bold mb-3">Words</h4>
          <div className="flex flex-col">
            <div className="flex flex-col gap-2">
              <Input value="やっと試験終わったね。" label="Phrase" />
              <Input value="洋子" label="Name" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default PhraseEdit;
