import React from "react";

type GridProps = {
  items: GridItem[];
};

type GridItem = {
  name: string;
  description: string;
};

export default function Grid(props: GridProps) {
  return (
    <>
      {/* Container for demo purpose */}
      <div className="container mx-auto md:px-6" style={{ marginTop: "4rem" }}>
        {/* Section: Design Block */}
        <section className="mb-36">
          <div className="flex justify-center mb-12">
            <div className="max-w-[700px] text-center">
              <p className="mb-6 font-bold uppercase text-primary dark:text-primary-400">
                Vocabulary
              </p>
              </div>
          </div>
          <div className="grid gap-x-6 md:grid-cols-2 lg:grid-cols-4 xl:gap-x-12">
            {props.items.map(item => (
              <div className="mb-12 p-2 hover:bg-gray-100">
                <div className="flex">
                  <div className="ml-2 grow">
                    <p className="mb-1 font-bold">{item.name}</p>
                    <p className="text-neutral-500 dark:text-neutral-300">
                      {item.description}
                    </p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </section>
        {/* Section: Design Block */}
      </div>
      {/* Container for demo purpose */}
    </>
  );
}
