import styles from "./word-puzzle.module.scss"
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import PuzzleKeyboard from "../keyboard/puzzleKeyboard";
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";
import { useState } from "react";
import WordPuzzleGuessWord from "./wordPuzzleGuessWord";
import { WordPuzzleBoard } from "../../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame } from "../../business/wordPuzzleGame";
import { Button, styled } from "@nextui-org/react";
import { doWordSearch } from "../../business/wordPuzzleSearch";

const WordPuzzleGuessWordContainer = styled('div');

const WordPuzzleGuessBoard = () => {
  const [board, setBoard] = useState<WordPuzzleBoard>(wordPuzzleGame.createBoard(5));

  const puzzleLetterTryChangeStatus = (letter: WordPuzzleLetter): void => {
    const newBoard = wordPuzzleGame.cycleLetterStatus(letter);
    setBoard(newBoard);
  }

  const keyboardKeyPressed = (keyboardKey: KeyboardKeys): void => {
    let newBoard: WordPuzzleBoard | null = null;
    if (keyboardKey === KeyboardKeys.Delete) {
      newBoard = wordPuzzleGame.backspace(board);
    }
    else if (keyboardKey === KeyboardKeys.Enter) {
      newBoard = wordPuzzleGame.addNewWord(board);
    }
    else {
      newBoard = wordPuzzleGame.addLetter(keyboardKey, board);
    }

    if (newBoard !== null) {
      setBoard(newBoard);
    }
  }

  const searchClicked = async (): Promise<void> => {
    await doWordSearch(board);
  }

  return (
    <div>
      <WordPuzzleGuessWordContainer className={styles.wordPuzzleWordContainer}>
        {
          board.words.map((w) => (
            <WordPuzzleGuessWord
              {...w}
              key={"word|" + w.index}
              onLetterClicked={puzzleLetterTryChangeStatus}
            />
          ))
        }
      </WordPuzzleGuessWordContainer>

      <Button color={"primary"} auto ghost={true} onPress={searchClicked}>Search</Button>

      <PuzzleKeyboard onKeyPressed={keyboardKeyPressed} />
    </div>
  )
}

export default WordPuzzleGuessBoard;