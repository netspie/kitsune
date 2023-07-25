import React from "react";

const stats = [
  { id: 1, name: "Transactions every 24 hours", value: "44 million" },
  { id: 2, name: "Assets under holding", value: "$119 trillion" },
  { id: 3, name: "New users annually", value: "46,000" },
];

export default function Grid4x2() {
  return (
    <>
      {/* Container for demo purpose */}
      <div className="container mx-auto md:px-6" style={{ marginTop: "4rem" }}>
        {/* Section: Design Block */}
        <section className="mb-36">
          <div className="flex justify-center mb-12">
            <div className="max-w-[700px] text-center">
              <p className="mb-6 font-bold uppercase text-primary dark:text-primary-400">
                Features
              </p>
              <h2 className="mb-6 text-3xl font-bold">Why is it so great?</h2>
              <p className="mb-16 text-neutral-500 dark:text-neutral-300">
                Nunc tincidunt vulputate elit. Mauris varius purus malesuada
                neque iaculis malesuada. Aenean gravida magna orci, non
                efficitur est porta id. Donec magna diam.
              </p>
            </div>
          </div>
          <div className="grid gap-x-6 md:grid-cols-2 lg:grid-cols-4 xl:gap-x-12">
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Support 24/7</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Pellentesque mollis, metus nec fringilla aliquam
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Tracking</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Magna lacus iaculis elit, quis pharetra varius.
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Reporting</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Pellentesque varius ex vel consequat quis.
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Analytics</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Vestibulum gravida iaculis nisl, vel lobortis eros.
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Huge community</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Praesent vulputate lacus bibendum augue .
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Easy to use</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Sed mauris ex, imperdiet sit amet nisl ac, ultrices.
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Frequent updates</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Aenean lectus ex, placerat id tellus in eros.
                  </p>
                </div>
              </div>
            </div>
            <div className="mb-12">
              <div className="flex">
                <div className="ml-2 grow">
                  <p className="mb-1 font-bold">Responsive</p>
                  <p className="text-neutral-500 dark:text-neutral-300">
                    Donec consequat orci quis volutpat imperdiet.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </section>
        {/* Section: Design Block */}
      </div>
      {/* Container for demo purpose */}
    </>
  );
}
