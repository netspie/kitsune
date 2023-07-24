import Image from 'next/image'
import { DatePicker, Hero } from './components'
import dynamic from 'next/dynamic'

const DynamicComponent = dynamic(() => import("./components/DatePicker"), {
  ssr: false
});

export default function Home() {
  return (
    <main className="overflow-hidden">
      <Hero />
      <DatePicker />
    </main>
  )
}
