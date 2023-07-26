import React from "react";

type BreadcrumbsProps = {
  items: BreadcrumbItem[];
};

type BreadcrumbItem = {
  name: string;
  href: string;
};

function Breadcrumbs(props: BreadcrumbsProps) {
  return (
    <nav className="w-full rounded-md py-3 dark:bg-neutral-600">
      <ol className="list-reset flex">
        {props.items.map((item, index: number) => (
          <>
            <li>
              <a
                href={item.href}
                className="text-primary transition duration-150 ease-in-out hover:text-primary-600 focus:text-primary-600 active:text-primary-700 dark:text-primary-400 dark:hover:text-primary-500 dark:focus:text-primary-500 dark:active:text-primary-600"
              >
                {item.name}
              </a>
            </li>
            {index < props.items.length - 1 && <span>&nbsp;/&nbsp;</span>}
          </>
        ))}
      </ol>
    </nav>
  );
}

export default Breadcrumbs;
