"use client";
import React, { ReactElement, useEffect } from "react";

type CollapseXProps = {
  id: string;
  TriggerContent?: ReactElement;
  ActualContent?: ReactElement;
  onActiveChanged?: (active: boolean) => void;
};

function CollapseX(props: CollapseXProps) {
  useEffect(() => {
    const init = async () => {
      const { Collapse, Dropdown, Ripple, initTE } = await import("tw-elements");
      initTE({ Collapse, Dropdown, Ripple });

      const collapse = document.getElementById(props.id);
      if (!collapse) return;
      console.log(`addEventListener to ${typeof collapse}`);
      collapse.addEventListener("show.te.collapse", () => {
        console.log("show");
        props.onActiveChanged && props.onActiveChanged(true);
      });
      collapse.addEventListener("hide.te.collapse", () => {
        console.log("collapse");
        props.onActiveChanged && props.onActiveChanged(false);
      });
    };
    init();
  }, []);

  return (
    <>
      <button
        className="flex flex-col justify-center items-center rounded bg-gray-200 px-2 pb-1 pt-1 hover:bg-gray-300 w-12 h-full"
        data-te-collapse-init=""
        data-te-ripple-init=""
        data-te-ripple-color="light"
        data-te-target={`#${props.id}`}
        aria-expanded="false"
        aria-controls={props.id}
      >
        {props.TriggerContent ?? "Trigger"}
      </button>
      <div
        className="!visible hidden"
        id={props.id}
        data-te-collapse-item=""
      >
        <div className="block rounded-lg py-2 px-1 dark:bg-neutral-700 dark:text-neutral-50">
          {props.ActualContent ?? "Content"}
        </div>
      </div>
    </>
  );
}

export default CollapseX;
