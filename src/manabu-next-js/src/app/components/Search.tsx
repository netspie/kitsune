"use client";

import React, { useEffect, useState } from "react";
import { Collapse, Dropdown, DropdownLabeled, PinList } from ".";

function Search() {
  const [filtersActive, setFiltersActive] = useState(false);
  const [alreadyInitialized, setAlreadyInitialized] = useState(false);
  useEffect(() => {
    const init = async () => {
      const { Collapse, Ripple, initTE } = await import("tw-elements");
      if (!alreadyInitialized) {
        initTE({  Collapse, Ripple, Dropdown });
        setAlreadyInitialized(true);
      }
    };
    init();
  }, []);

  return (
    <div className="flex flex-col mb-3 p-2 bg-gray-100 rounded-lg">
      <div className="relative flex w-full flex-wrap items-center gap-2 h-8">
        {/*Search icon*/}
        <button className="flex flex-col justify-center w-8 items-center rounded bg-gray-200 py-1.5 hover:bg-gray-300 h-full">
          <span
            className="input-group-text flex items-center whitespace-nowrap rounded py-1.5 text-center text-base font-normal text-neutral-800 dark:text-neutral-200 "
            id="basic-addon2"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 20 20"
              fill="currentColor"
              className="h-5 w-5"
            >
              <path
                fillRule="evenodd"
                d="M9 3.5a5.5 5.5 0 100 11 5.5 5.5 0 000-11zM2 9a7 7 0 1112.452 4.391l3.328 3.329a.75.75 0 11-1.06 1.06l-3.329-3.328A7 7 0 012 9z"
                clipRule="evenodd"
              />
            </svg>
          </span>
        </button>
        <input
          type="search"
          className="h-full relative m-0 block w-[1px] min-w-0 flex-auto rounded border border-solid border-neutral-300 bg-transparent bg-clip-padding px-3 text-base font-normal leading-[1.6] text-neutral-700 outline-none transition duration-200 ease-in-out focus:z-[3] focus:border-primary focus:text-neutral-700 focus:shadow-[inset_0_0_0_1px_rgb(59,113,202)] focus:outline-none dark:border-neutral-600 dark:text-neutral-200 dark:placeholder:text-neutral-200 dark:focus:border-primary bg-white"
          placeholder="Search"
          aria-label="Search"
          aria-describedby="button-addon2"
        />
        {/* Settings Button */}
        <button
          className="flex flex-col justify-center items-center rounded bg-gray-200 px-2 pb-1 pt-1 hover:bg-gray-300 h-full"
          data-te-collapse-init=""
          data-te-ripple-init=""
          data-te-ripple-color="light"
          data-te-target="#settings"
          aria-expanded="false"
          aria-controls="settings"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={1.5}
            stroke="currentColor"
            className="w-6 h-6"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M10.5 6h9.75M10.5 6a1.5 1.5 0 11-3 0m3 0a1.5 1.5 0 10-3 0M3.75 6H7.5m3 12h9.75m-9.75 0a1.5 1.5 0 01-3 0m3 0a1.5 1.5 0 00-3 0m-3.75 0H7.5m9-6h3.75m-3.75 0a1.5 1.5 0 01-3 0m3 0a1.5 1.5 0 00-3 0m-9.75 0h9.75"
            />
          </svg>
        </button>
      </div>
      {/* Settings */}
      <div className="!visible hidden" id="settings" data-te-collapse-item="">
        <div className="flex gap-2 rounded-lg py-2 dark:bg-neutral-700 dark:text-neutral-50">
          {/* Filters Button*/}
          <button
            className="flex flex-col justify-center items-center rounded bg-gray-200 px-2 pb-1 pt-1 hover:bg-gray-300 h-full"
            data-te-collapse-init=""
            data-te-ripple-init=""
            data-te-ripple-color="light"
            data-te-target="#filters"
            aria-expanded="false"
            aria-controls="filters"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth={1.5}
              stroke="currentColor"
              className="w-6 h-6"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M10.5 6h9.75M10.5 6a1.5 1.5 0 11-3 0m3 0a1.5 1.5 0 10-3 0M3.75 6H7.5m3 12h9.75m-9.75 0a1.5 1.5 0 01-3 0m3 0a1.5 1.5 0 00-3 0m-3.75 0H7.5m9-6h3.75m-3.75 0a1.5 1.5 0 01-3 0m3 0a1.5 1.5 0 00-3 0m-9.75 0h9.75"
              />
            </svg>
          </button>
          <DropdownLabeled
            label="Sort By"
            dropdownKey="afsdasfaf"
            selectedId="Author"
            items={["Creation", "Author", "Item Name"]}
            onItemClick={(selected: string) => {
              console.log(selected);
            }}
          />
        </div>
        <div className="!visible hidden" id="filters" data-te-collapse-item="">
          <PinList
            items={[
              { name: "Animed" },
              { name: "Mangav" },
              { name: "TVs" },
              { name: "Dramad" },
              { name: "Conversationf" },
              { name: "Interviewd" },
              { name: "Xd" },
            ]}
          />
        </div>
      </div>
    </div>
  );
}

export default Search;
