import styles from "./word-puzzle.module.scss"
import { Container, Text } from "@nextui-org/react";
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import WordPuzzleGuessLetter from "./wordPuzzleGuessLetter";

interface WordPuzzleGuessWordProps {
  letters: WordPuzzleLetter[];
}

const WordPuzzleGuessWord = (props: WordPuzzleGuessWordProps) => {
  return (
    <Container className={styles.puzzleWord}>
      {
        props.letters.map((l) => <WordPuzzleGuessLetter {...l} key={`${l.letter}|${l.position}`} />)
      }
    </Container>
  );
}

export default WordPuzzleGuessWord;