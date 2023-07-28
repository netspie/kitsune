"use client";
import React, { useEffect, useState } from "react";
import { v4 as uuid } from "uuid";

type NavbarProps = {
  groups: NavbarGroup[];
};

type NavbarGroup = {
  name: string;
  href?: string;
  items?: NavbarItem[];
};

type NavbarItem = {
  name: string;
  href: string;
};

export default function Navbar(props: NavbarProps) {
  const [alreadyInitialized, setAlreadyInitialized] = useState(false);

  useEffect(() => {
    const init = async () => {
      const { Dropdown, initTE } = await import("tw-elements");
      if (!alreadyInitialized) {
        initTE({   Dropdown });
        setAlreadyInitialized(true);
      }
    };
    init();
  }, []);

  return (
    <>
      <nav
        className="relative flex w-full items-center justify-start py-2 text-neutral-600 dark:text-neutral-300 lg:flex-wrap lg:justify-center"
        data-te-navbar-ref=""
      >
        <div className="px-6">
          <button
            className="border-0 bg-transparent py-3 text-xl leading-none transition-shadow duration-150 ease-in-out hover:text-neutral-700 focus:text-neutral-700 dark:hover:text-white dark:focus:text-white lg:hidden"
            type="button"
            data-te-collapse-init=""
            data-te-target="#navbarSupportedContentX"
            aria-controls="navbarSupportedContentX"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="[&>svg]:w-8">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
                className="h-8 w-8"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5"
                />
              </svg>
            </span>
          </button>
          <div
            className="!visible hidden flex-grow basis-[100%] items-center lg:!flex lg:basis-auto gap-4"
            id="navbarSupportedContentX"
            data-te-collapse-item=""
          >
            {props.groups.map((group) => (
              <ul
                className="mr-auto flex flex-row"
                data-te-navbar-nav-ref=""
                key={uuid()}
              >
                <li
                  className="static"
                  data-te-nav-item-ref=""
                  data-te-dropdown-ref=""
                >
                  {group.items && group.items.length > 0 ? (
                    <a
                      className="flex items-center whitespace-nowrap py-2 pr-2  transition duration-150 ease-in-out hover:text-neutral-700 focus:text-neutral-700 dark:hover:text-white dark:focus:text-white lg:px-2"
                      href="#"
                      data-te-ripple-init=""
                      data-te-ripple-color="light"
                      type="button"
                      id="dropdownMenuButtonX"
                      data-te-dropdown-toggle-ref=""
                      aria-expanded="false"
                      data-te-nav-link-ref=""
                    >
                      {group.name}
                      {group.items && (
                        <span className="ml-2 w-2" key={uuid()}>
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 20 20"
                            fill="currentColor"
                            className="h-5 w-5"
                          >
                            <path
                              fillRule="evenodd"
                              d="M5.23 7.21a.75.75 0 011.06.02L10 11.168l3.71-3.938a.75.75 0 111.08 1.04l-4.25 4.5a.75.75 0 01-1.08 0l-4.25-4.5a.75.75 0 01.02-1.06z"
                              clipRule="evenodd"
                            />
                          </svg>
                        </span>
                      )}
                    </a>
                  ) : (
                    <a
                      className="flex items-center whitespace-nowrap py-2 pr-2  transition duration-150 ease-in-out hover:text-neutral-700 focus:text-neutral-700 dark:hover:text-white dark:focus:text-white lg:px-2"
                      href={group.href}
                      data-te-ripple-init=""
                      data-te-ripple-color="light"
                      type="button"
                    >
                      {group.name}
                      {group.items && (
                        <span className="ml-2 w-2" key={uuid()}>
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 20 20"
                            fill="currentColor"
                            className="h-5 w-5"
                          >
                            <path
                              fillRule="evenodd"
                              d="M5.23 7.21a.75.75 0 011.06.02L10 11.168l3.71-3.938a.75.75 0 111.08 1.04l-4.25 4.5a.75.75 0 01-1.08 0l-4.25-4.5a.75.75 0 01.02-1.06z"
                              clipRule="evenodd"
                            />
                          </svg>
                        </span>
                      )}
                    </a>
                  )}
                  <div
                    className={`absolute top-full z-[1000] mt-0 hidden w-fit border-none bg-white bg-clip-padding text-neutral-600 shadow-lg dark:bg-neutral-700 dark:text-neutral-200 [&[data-te-dropdown-show]]:block ${
                      (!group.items || group.items.length === 0) && "invisible"
                    }`}
                    aria-labelledby="dropdownMenuButtonX"
                    data-te-dropdown-menu-ref=""
                  >
                    <div className="pt-2 lg:px-4 w-fit">
                      <div className="flex flex-col">
                        <div>
                          {group.items &&
                            group.items.map((item) => (
                              <a
                                href={item.href}
                                key={uuid()}
                                aria-current="true"
                                className="block w-full border-b border-neutral-200 py-2 transition duration-150 ease-in-out hover:bg-neutral-50 hover:text-neutral-700 dark:border-neutral-500 dark:hover:bg-neutral-800 dark:hover:text-white"
                              >
                                {item.name}
                              </a>
                            ))}
                        </div>
                      </div>
                    </div>
                  </div>
                </li>
              </ul>
            ))}
          </div>
        </div>
      </nav>
    </>
  );
}
