import styles from "./word-puzzle.module.scss"
import { Text } from "@nextui-org/react";
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";

interface WordPuzzleGuessLetterProps extends WordPuzzleLetter {
}

const WordPuzzleGuessLetter = (props: WordPuzzleGuessLetterProps) => {
  return (
    <div className={styles.puzzleLetterBox}>
      <Text className={styles.puzzleLetter}>{props.letter}</Text>
    </div>
  );
}

export default WordPuzzleGuessLetter;