"use client";
import React, { useEffect } from "react";

export default function Navbar() {
  useEffect(() => {
    const init = async () => {
      const { Collapse, Dropdown, initTE } = await import("tw-elements");
      initTE({ Collapse, Dropdown });
    };
    init();
  }, []);

  return (
    <nav className="relative flex w-full flex-wrap items-center justify-between bg-[#FBFBFB] py-2 text-neutral-500  hover:text-neutral-700 focus:text-neutral-700 dark:bg-neutral-600 lg:py-4">
      <div className="flex w-full flex-wrap items-center justify-between px-3">
        {/* Left elements */}
        <div className="flex">
          <form className="hidden md:flex">
            <input
              type="search"
              className="relative m-0 block w-[250px] min-w-0 flex-auto rounded border border-solid border-neutral-300 bg-transparent bg-clip-padding px-3 text-base font-normal leading-[1.6] text-neutral-700 outline-none motion-reduce:transition-none transition duration-200 ease-in-out focus:z-[3] focus:border-primary focus:text-neutral-700 focus:shadow-[inset_0_0_0_1px_rgb(59,113,202)] focus:outline-none dark:border-neutral-500 dark:text-neutral-200 dark:placeholder:text-neutral-200 dark:focus:border-primary"
              placeholder="Search"
              aria-label="Search"
              aria-describedby="button-addon2"
            />
            {/*Search icon*/}
            <span
              className="input-group-text flex items-center whitespace-nowrap rounded px-3 text-center text-base font-normal text-neutral-700 dark:text-neutral-200"
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
          </form>
        </div>
        {/* Left elements */}
        {/* Right elements */}
        <ul
          className="list-style-none ml-auto flex flex-row pl-0 md:pl-4 justify-center"
          data-te-navbar-nav-ref="">
          <li
            className="flex flex-col justify-center px-2"
            data-te-nav-item-ref=""
          >
            <a
              href="#!"
              className="text-neutral-800 transition duration-150 ease-in-out hover:text-neutral-800 focus:text-neutral-800 active:text-neutral-900 dark:text-neutral-900 dark:hover:text-neutral-900 dark:focus:text-neutral-900 dark:active:text-neutral-900"
            >
              Dark link
            </a>
          </li>
          <li
            className="flex flex-col justify-center px-2"
            data-te-nav-item-ref=""
          >
            <a
              href="#!"
              className="text-neutral-800 transition duration-150 ease-in-out hover:text-neutral-800 focus:text-neutral-800 active:text-neutral-900 dark:text-neutral-900 dark:hover:text-neutral-900 dark:focus:text-neutral-900 dark:active:text-neutral-900"
            >
              Dark link
            </a>
          </li>
          <li
            className="flex flex-col justify-center px-2"
            data-te-nav-item-ref=""
          >
            <a
              href="#!"
              className="text-neutral-800 transition duration-150 ease-in-out hover:text-neutral-800 focus:text-neutral-800 active:text-neutral-900 dark:text-neutral-900 dark:hover:text-neutral-900 dark:focus:text-neutral-900 dark:active:text-neutral-900"
            >
              Dark link
            </a>
          </li>
          <li
            className="flex flex-col justify-center px-2"
            data-te-nav-item-ref=""
          >
            <a
              href="#!"
              className="text-neutral-800 transition duration-150 ease-in-out hover:text-neutral-800 focus:text-neutral-800 active:text-neutral-900 dark:text-neutral-900 dark:hover:text-neutral-900 dark:focus:text-neutral-900 dark:active:text-neutral-900"
            >
              Dark link
            </a>
          </li>
        </ul>
        {/* Right elements */}
      </div>
    </nav>
  );
}
