"use client";
import React, { useState } from "react";
import Pin from "./Pin";
import { PinProps } from "./Pin";

type PinListProps = {
  items: PinProps[];
};

function PinList(props: PinListProps) {
  return (
    <>
      <div className="flex gap-2 flex-wrap">
        {props.items.map((item) => (
          <Pin id={item.id} name={item.name} />
        ))}
      </div>
    </>
  );
}

export default PinList;
