"use client";
import React from "react";
import Pin from "./Pin";
import { PinProps } from "./Pin";

type PinListProps = {
  items: PinProps[];
  onToggle?: (active: boolean, name: string, id?: string) => void;
};

function PinList(props: PinListProps) {
  return (
    <>
      <div className="flex gap-2 flex-wrap">
        {props.items.map((item) => (
          <Pin id={item.id} name={item.name} onToggle={props.onToggle} />
        ))}
      </div>
    </>
  );
}

export default PinList;
