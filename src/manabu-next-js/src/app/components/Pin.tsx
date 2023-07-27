"use client";
import React, { useState } from "react";

export type PinProps = {
  id?: string;
  name: string;
};

function Pin(props: PinProps) {
  const [active, setActive] = useState(false);

  return (
    <button
      className={`block rounded bg-gray-400 px-2 pb-2 pt-2 text-xs text-white uppercase leading-tight font-thin
      ${active && "bg-primary-600"}`}
      key={props.id ?? props.name}
      role="button"
      data-te-nav-active={active}
      onClick={() => {
        setActive(!active);
        console.log(active);
      }}
    >
      {props.name}
    </button>
  );
}

export default Pin;
