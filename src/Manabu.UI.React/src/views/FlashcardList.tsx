import { useState, useEffect, useRef } from 'react';
import { Flashcard, FlashcardItem } from './Flashcard';
import { shuffle, range } from 'lodash'

type FlashcardListProps = {
    flashcards: FlashcardProp[];
}

type FlashcardProp = {
    questions: FlashcardItem[];
    answers: FlashcardItem[];
}

export function FlashcardList(props: FlashcardListProps) {
    const flashcardRef = useRef(null);
    const [index, setIndex] = useState(0);
    const [indexes, setIndexes] = useState<Array<number>>(
        shuffle(range(props.flashcards.length))
    );

    function ChangeIndex(dir: number) {
        var sign = Math.sign(dir);
        var nextIndex = index + sign;
        if (nextIndex < 0) 
            nextIndex = props.flashcards.length - 1;
        else
        if (nextIndex >= props.flashcards.length) 
            nextIndex = 0;

        setIndex(nextIndex);
        
    }

    return (
        <>
        {
            <Flashcard
                classTop="fixed left-80" 
                questions={props.flashcards[indexes[index]].questions}
                answers={props.flashcards[indexes[index]].answers}
                onPrevClick={() => ChangeIndex(-1)}
                onNextClick={() => ChangeIndex(1)} />
        }
        </>
    )
}
