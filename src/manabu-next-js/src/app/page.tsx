import Image from "next/image";
import { DatePicker, Hero, Grid4x2, Navbar } from "./components";
import dynamic from "next/dynamic";

const DynamicComponent = dynamic(() => import("./components/DatePicker"), {
  ssr: false,
});

export default function Home() {
  return (
    <>
      <main className="relative px-8">
        <Grid4x2 />
      </main>
    </>
  );
}
