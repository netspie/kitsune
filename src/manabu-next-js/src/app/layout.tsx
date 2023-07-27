import type { Metadata } from "next";
import { Andika } from "next/font/google";
import "../../styles/globals.css";
import "tw-elements/dist/css/tw-elements.min.css";
import { MainMenu, Navbar } from "./components";

const roboto = Andika({ weight: "400", subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Create Next App",
  description: "Generated by create next app",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={`${roboto.className} body-font font-poppins`}>
        <header className="relative w-full">
          <Navbar
            groups={[
              {
                name: "Home"
              },
              {
                name: "Learn"
              },
              {
                name: "Materials",
                items: [
                  { name: "Courses", href: "" },
                  { name: "Kana", href: "" },
                  { name: "Kanji", href: "" },
                  { name: "Vocabulary", href: "" },
                  { name: "Frameworks", href: "" },
                  { name: "Themes", href: "" },
                  { name: "Media", href: "media" },
                  { name: "Keigo", href: "" },
                  { name: "Linguistics", href: "" },
                  { name: "Dialects", href: "" },
                  { name: "Jargons", href: "" },
                ],
              },
              {
                name: "Practice",
                items: [
                  { name: "Review", href: "" },
                  { name: "Spaced Repetition", href: "" },
                  { name: "History", href: "" },
                ],
              },
              {
                name: "Account",
                items: [
                  { name: "Profile", href: "" },
                  { name: "Settings", href: "" },
                  { name: "Sign In", href: "" },
                ],
              },
              {
                name: "Help",
                items: [
                  { name: "About", href: "" },
                  { name: "Contact", href: "" },
                ],
              }
            ]}
          />
        </header>
        {/* <MainMenu /> */}
        {children}
      </body>
    </html>
  );
}
