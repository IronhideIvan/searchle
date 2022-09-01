import styles from "./word-puzzle.module.scss"
import { Container } from "@nextui-org/react";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import WordPuzzleGuessLetter from "./wordPuzzleGuessLetter";
import { WordPuzzleWord } from "../../interfaces/wordPuzzle/wordPuzzleWord";

interface WordPuzzleGuessWordProps extends WordPuzzleWord {
  onWordChanged?(word: WordPuzzleWord): void;
}

const WordPuzzleGuessWord = (props: WordPuzzleGuessWordProps) => {
  const onLetterClicked = (letter: WordPuzzleLetter): void => {
    if (!props.onWordChanged) {
      return;
    }

    const { status } = letter;
    let newStatus = status;

    if (status === WordPuzzleLetterStatus.Unresolved) {
      newStatus = WordPuzzleLetterStatus.CorrectPosition;
    }
    else if (status === WordPuzzleLetterStatus.CorrectPosition) {
      newStatus = WordPuzzleLetterStatus.IncorrectPosition;
    }
    else if (status === WordPuzzleLetterStatus.IncorrectPosition) {
      newStatus = WordPuzzleLetterStatus.NotExists;
    }
    else if (status === WordPuzzleLetterStatus.NotExists) {
      newStatus = WordPuzzleLetterStatus.Unresolved;
    }

    const newLetters: WordPuzzleLetter[] =
      props.letters.slice(0, letter.index).concat(
        [{
          ...letter,
          status: newStatus
        }],
        props.letters.slice(letter.index + 1)
      );

    const newWord: WordPuzzleWord = {
      ...props,
      letters: newLetters
    }

    props.onWordChanged(newWord);
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