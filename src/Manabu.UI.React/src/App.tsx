import { useEffect, useState } from 'react'
import { FlashcardList } from './views/FlashcardList'
import { Something } from './views/Something'
import { MainMenu } from './views/MainMenu'
import  DropdownX from './views/DropdownX'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div className='flex flex-col items-start space-y-1 gap-4 p-5'>
      <DropdownX
          key="2"
          selectedId="Anime"
          items={[
            "Anime",
            "Drama",
            "TV Show",
            "Manga",
            "Newspaper",
            "Conversation",
          ]}
        />
        <MainMenu />
        <Something />
        <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
          Button
        </button>
        <FlashcardList
          flashcards={
            [
              {
                questions: [
                  { id: '1', name: "What's the deal?" },
                ],
                answers: [
                  { id: '1', name: 'Not your business, dude!' },
                ],
              },
              {
                questions: [
                  { id: '2', name: 'Who is the first president of the United States?' },
                ],
                answers: [
                  { id: '2', name: 'George Washington' },
                ],
              },
              {
                questions: [
                  { id: '3', name: 'What is the capital of France?' },
                ],
                answers: [
                  { id: '3', name: 'Paris' },
                ],
              },
              {
                questions: [
                  { id: '4', name: 'In which year did the Titanic sink?' },
                ],
                answers: [
                  { id: '4', name: '1912' },
                ],
              },
              {
                questions: [
                  { id: '5', name: 'Who wrote the play "Romeo and Juliet"?' },
                ],
                answers: [
                  { id: '5', name: 'William Shakespeare' },
                ],
              },
              {
                questions: [
                  { id: '6', name: 'What is the chemical symbol for water?' },
                ],
                answers: [
                  { id: '6', name: 'H2O' },
                ],
              },
              {
                questions: [
                  { id: '7', name: 'What is the tallest mountain on Earth?' },
                ],
                answers: [
                  { id: '7', name: 'Mount Everest' },
                ],
              },
              {
                questions: [
                  { id: '8', name: 'What is the capital of Japan?' },
                ],
                answers: [
                  { id: '8', name: 'Tokyo' },
                ],
              },
              {
                questions: [
                  { id: '9', name: 'Who painted the Mona Lisa?' },
                ],
                answers: [
                  { id: '9', name: 'Leonardo da Vinci' },
                ],
              },
              {
                questions: [
                  { id: '10', name: 'What is the symbol for the element oxygen?' },
                ],
                answers: [
                  { id: '10', name: 'O' },
                ],
              },
            ]
          } />
      </div>
    </>
  )
}

export default App
