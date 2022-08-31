import type { NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import styles from '../styles/Home.module.scss'
import { Button } from '@nextui-org/react';
import WordPuzzleGuessWord from '../components/wordPuzzle/wordPuzzleGuessWord';
import { WordPuzzleLetterStatus } from '../interfaces/wordPuzzle/wordPuzzleLetter';

const Home: NextPage = () => {
  return (
      <WordPuzzleGuessWord letters={[
        {
          letter: "A",
          status: WordPuzzleLetterStatus.NotExists,
          position: 0
        },
        {
          letter: "B",
          status: WordPuzzleLetterStatus.NotExists,
          position: 1
        },
        {
          letter: "C",
          status: WordPuzzleLetterStatus.NotExists,
          position: 2
        },
        {
          letter: "D",
          status: WordPuzzleLetterStatus.NotExists,
          position: 3
        },
        {
          letter: "E",
          status: WordPuzzleLetterStatus.NotExists,
          position: 4
        }
      ]} />
  )
}

export default Home
