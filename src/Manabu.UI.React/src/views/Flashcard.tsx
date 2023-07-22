import { useEffect, useState } from 'react';

export type FlashcardItem = {
    id: string;
    name: string;
}

export type FlashcardProps = {
    classTop?: string;
    questions: FlashcardItem[];
    answers: FlashcardItem[];
    onPrevClick: () => void;
    onNextClick: () => void;
}

export function Flashcard(props: FlashcardProps) {
    const [answerShown, setAnswerShown] = useState(false);

    return (
        <>
        <div className={`flex flex-col items-center justify-evenly gap-5
            divide-y w-80 min-w-fit h-80 min-h-fit ${props.classTop}`}>
            <div className="flex flex-col items-center justify-evenly 
                h-32 min-h-fit">
                {/* Questions */}
                <div className="flex flex-col items-center">
                {
                    props.questions.map(question => (
                        <span key={question.id}>{question.name}</span>
                    ))
                }
                </div>
                {/* Answers */}
                {
                    answerShown && 
                    <div className="flex flex-col items-center">
                        {props.answers.map(answer => (
                            <span 
                                key={answer.id} 
                                className="text-xs">
                                {answer.name}
                            </span>
                        ))}
                    </div>
                }
            </div>
            <div className="flex justify-evenly py-4 min-w-full">
                <button onClick={() => {
                    props.onPrevClick();
                    setAnswerShown(false);
                    }}>Prev</button>
                <button onClick={() => setAnswerShown(true)}>Answer</button>
                <button onClick={() => {
                    props.onNextClick();
                    setAnswerShown(false);
                    }}>Next</button>
            </div>
        </div>
        </>
    )
}