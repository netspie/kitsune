"use client";
import React from "react";

type CardProps = {
  name: string;
  type: string;
  creation: string;
  author: string;
};

type LinkItem = {
  text: string;
  href: string;
};

function Card(props: CardProps) {
  return (
    <div className="text-left w-full h-fit">
      <div className="items-start w-full">
        <div className="items-start w-full">
          <div className="flex items-start">
            <div className="w-full">
              <div className="relative w-full items-start z-[1] block rounded-lg bg-[hsla(0,0%,100%,0.55)] px-6 py-6 shadow-[0_2px_15px_-3px_rgba(0,0,0,0.23),0_10px_20px_-2px_rgba(0,0,0,0.04)] backdrop-blur-[25px] dark:bg-[hsla(0,0%,5%,0.7)] dark:shadow-black/20 ">
                <div className="flex ">
                  <h2 className="flex-grow mb-2 text-3xl font-bold text-primary dark:text-primary-400">
                    {props.name}
                  </h2>
                  <p className="top-12 right-12 font-semibold">{props.type}</p>
                </div>
                <p className="font-thin text-secondary-800"> {props.creation}</p>
                <p className="font-thin text-secondary-500">
                 {props.author}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Card;
