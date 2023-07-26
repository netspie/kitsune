import React from "react";
import { useEffect, useState, useRef } from "react";
import { v4 as uuid } from "uuid";
import Dropdown from 'tw-elements';

type DropdownProps = {
  selectedId: string;
  items: DropdownItem[];
};

type DropdownItem =
  | {
      id: string;
      name: string;
    }
  | string;

export default function DropdownX(props: DropdownProps) {
  const [dropdownId] = useState(uuid());
  const dropdownRef = useRef(null);
  
  useEffect(() => {
    const init = async () => {
      const { Dropdown, Ripple, initTE } = await import("tw-elements");
      initTE({ Dropdown, Ripple });
    };
    init();
    const myDropdown = document.getElementById(dropdownId);
    const myDropdownInstance = new Dropdown(myDropdown);
    myDropdownInstance.show();
  }, []);

  function GetSelectedItem(): string {
    const item = props.items.find((item) =>
      typeof item === "string"
        ? item === props.selectedId
        : item.id === props.selectedId
    );

    if (typeof item === "string") return item;
    else if (item) return item.name;

    return "";
  }

  return (
    <>
      <div className="flex justify-center">
        <div>
          <div className="relative" data-te-dropdown-ref>
            <button
              className="flex items-center whitespace-nowrap rounded bg-primary px-6 pb-2 pt-2.5 text-xs font-medium uppercase leading-normal text-white shadow-[0_4px_9px_-4px_#3b71ca] transition duration-150 ease-in-out hover:bg-primary-600 hover:shadow-[0_8px_9px_-4px_rgba(59,113,202,0.3),0_4px_18px_0_rgba(59,113,202,0.2)] focus:bg-primary-600 focus:shadow-[0_8px_9px_-4px_rgba(59,113,202,0.3),0_4px_18px_0_rgba(59,113,202,0.2)] focus:outline-none focus:ring-0 active:bg-primary-700 active:shadow-[0_8px_9px_-4px_rgba(59,113,202,0.3),0_4px_18px_0_rgba(59,113,202,0.2)] motion-reduce:transition-none"
              type="button"
              id={dropdownId}
              data-te-dropdown-toggle-ref={dropdownRef as Dropdown}
              aria-expanded="false"
              data-te-ripple-init=""
              data-te-ripple-color="light"
            >
              {GetSelectedItem()}
              <span className="ml-2 w-2">
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
            </button>
            <ul
              className="absolute z-[1000] float-left m-0 hidden min-w-max list-none overflow-hidden rounded-lg border-none bg-white bg-clip-padding text-left text-base shadow-lg dark:bg-neutral-700 [&[data-te-dropdown-show]]:block"
              aria-labelledby={dropdownId}
              data-te-dropdown-menu-ref
            >
              {props.items.map((item) => (
                <li>
                  <a
                    className="block w-full whitespace-nowrap bg-transparent px-4 py-2 text-sm font-normal text-neutral-700 hover:bg-neutral-100 active:text-neutral-800 active:no-underline disabled:pointer-events-none disabled:bg-transparent disabled:text-neutral-400 dark:text-neutral-200 dark:hover:bg-neutral-600"
                    href="#"
                    data-te-dropdown-item-ref={uuid()}
                  >
                    {typeof item === "string" ? item : item.name}
                  </a>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </>
  );
}
