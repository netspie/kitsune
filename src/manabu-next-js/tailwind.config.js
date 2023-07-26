/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,ts,jsx,tsx,mdx}",
    "/node_modules/flowbite-react/**/*.js"
  ],
  purge: [
    "./src/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  mode: "jit",
  variants: {
    extend: {
        display: ["group-hover"],
        visibility: ["group-hover"]
    },
  },
  theme: {
    extend: {},
  },
  plugins: [require("tw-elements/dist/plugin.cjs"), require('flowbite/plugin')]
}
