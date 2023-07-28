/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,ts,jsx,tsx,mdx}",
    "/node_modules/flowbite-react/**/*.js"
  ],
  purge: [
    "./src/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  variants: {
    extend: {
        display: ["group-hover"],
        visibility: ["group-hover"]
        
    },
  },
  theme: {
    extend: {
      width: {
        900: '900px'
      }
    }
  },
  plugins: [require("tw-elements/dist/plugin.cjs"), require('flowbite/plugin')]
}
