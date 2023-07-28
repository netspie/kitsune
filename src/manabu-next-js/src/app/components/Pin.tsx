"use client";
import React, { useState } from "react";

export type PinProps = {
  id?: string;
  name: string;
  active?: boolean;
  onToggle?: (active: boolean, name: string, id?: string) => void;
};

function Pin(props: PinProps) {
  const [active, setActive] = useState(props.active ?? false);

  return (
    <button
      className={`block rounded bg-gray-400 px-2 pb-2 pt-2 text-xs text-white uppercase leading-tight font-thin 
      ${active && "bg-primary-600"}`}
      key={props.id ?? props.name}
      role="button"
      onClick={() => {
        setActive(!active);
        props.onToggle && props?.onToggle(active, props.name, props.id);
      }}
    >
      {props.name}
    </button>
  );
}

export default Pin;
