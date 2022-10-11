import styles from "./word-puzzle.module.scss"
import { Container } from "@nextui-org/react";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import WordPuzzleGuessLetter from "./wordPuzzleGuessLetter";
import { WordPuzzleWord } from "../../interfaces/wordPuzzle/wordPuzzleWord";

interface WordPuzzleGuessWordProps extends WordPuzzleWord {
  onLetterClicked?(letter: WordPuzzleLetter): void;
}

const WordPuzzleGuessWord = (props: WordPuzzleGuessWordProps) => {
  const onLetterClicked = (letter: WordPuzzleLetter): void => {
    if (props.onLetterClicked) {
      props.onLetterClicked(letter);
    }

  }

  return (
    <Container className={styles.puzzleWord}>
      {
        props.letters.map((l) => <WordPuzzleGuessLetter
          {...l}
          key={`${l.letter}|${l.index}`}
          onClick={onLetterClicked}
        />)
      }
    </Container>
  );
}

export default WordPuzzleGuessWord;