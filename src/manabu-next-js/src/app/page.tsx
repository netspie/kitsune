import Image from "next/image";
import { DatePicker, Hero, Grid, Navbar } from "./components";
import dynamic from "next/dynamic";

const DynamicComponent = dynamic(() => import("./components/DatePicker"), {
  ssr: false,
});

export default function Home() {
  return (
    <>
      <main className="relative px-8 flex flex-col justify-center mb-12 mt-64">
        <h2 className="mb-6 text-6xl font-bold text-center">
          Manabu
        </h2>
        <p className="mb-16 text-center text-neutral-500 dark:text-neutral-300 uppercase">
          Ultimate solution to learn Japanese!
        </p>
      </main>
    </>
  );
}
