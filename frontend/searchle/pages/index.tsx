import type { NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import styles from '../styles/Home.module.scss'
import { Button } from '@nextui-org/react';
import WordPuzzleGuessWord from '../components/wordPuzzle/wordPuzzleGuessWord';
import { WordPuzzleLetter, WordPuzzleLetterStatus } from '../interfaces/wordPuzzle/wordPuzzleLetter';
import PuzzleKeyboard from '../components/keyboard/puzzleKeyboard';
import { useState } from 'react';
import { WordPuzzleWord } from '../interfaces/wordPuzzle/wordPuzzleWord';
import { KeyboardKeys } from '../interfaces/keyboard/keyboardKeys';

const defaultWordPuzzleLetter: WordPuzzleLetter = {
  letter: "_",
  status: WordPuzzleLetterStatus.Unresolved,
  index: -1
}

const Home: NextPage = () => {
  const [words, setWords] = useState<WordPuzzleWord[]>([
    {
      letters: [
        { ...defaultWordPuzzleLetter, index: 0 },
        { ...defaultWordPuzzleLetter, index: 1 },
        { ...defaultWordPuzzleLetter, index: 2 },
        { ...defaultWordPuzzleLetter, index: 3 },
        { ...defaultWordPuzzleLetter, index: 4 }
      ],
      index: 0
    }
  ])

  const puzzleWordChanged = (word: WordPuzzleWord): void => {
    let newWords = words.splice(0, word.index).concat(
      [word],
      words.splice(word.index + 1));
    setWords(newWords);
  }

  const keyboardKeyPressed = (keyboardKey: KeyboardKeys): void => {
    if (words.length === 0) {
      return;
    }

    const currentWord = words[words.length - 1];
  }

  return (
    <div>
      <div>
        {
          words.map((w) => (
            <WordPuzzleGuessWord
              {...w}
              key={"word|" + w.index}
              onWordChanged={puzzleWordChanged}
            />
          ))
        }
      </div>

      <PuzzleKeyboard></PuzzleKeyboard>
    </div>
  )
}

export default Home
