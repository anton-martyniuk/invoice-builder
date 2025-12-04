/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Söhne', 'Inter', 'system-ui', 'sans-serif'],
      },
      colors: {
        blue: {
          primary: '#4f46e5',
          50: '#f5f7ff',
          100: '#ecf0ff',
          200: '#d9e1ff',
          300: '#b6c4ff',
          400: '#8494ff',
          500: '#6366f1',
          600: '#4f46e5',
          700: '#4338ca',
          800: '#3730a3',
          900: '#312e81',
        },
        chatbg: {
          light: '#ffffff',
          dark: '#f9fafb',
        }
      },
      boxShadow: {
        'chat-sm': '0 2px 5px -1px rgba(79, 70, 229, 0.1), 0 1px 3px -1px rgba(0, 0, 0, 0.05)',
        'chat-md': '0 4px 6px -1px rgba(79, 70, 229, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
        'chat-lg': '0 10px 15px -3px rgba(79, 70, 229, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)',
      }
    },
  },
  plugins: [],
}