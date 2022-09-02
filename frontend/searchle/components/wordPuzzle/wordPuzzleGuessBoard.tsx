import styles from "./word-puzzle.module.scss"
import { Container } from "@nextui-org/react";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import WordPuzzleGuessLetter from "./wordPuzzleGuessLetter";
import { WordPuzzleWord } from "../../interfaces/wordPuzzle/wordPuzzleWord";
import PuzzleKeyboard from "../keyboard/puzzleKeyboard";
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";
import { useState } from "react";
import WordPuzzleGuessWord from "./wordPuzzleGuessWord";
import { WordPuzzleBoard } from "../../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame, WordPuzzleGame } from "../../types/wordPuzzleGame";

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

  return (
    <div>
      <div>
        {
          board.words.map((w) => (
            <WordPuzzleGuessWord
              {...w}
              key={"word|" + w.index}
              onLetterClicked={puzzleLetterTryChangeStatus}
            />
          ))
        }
      </div>

      <PuzzleKeyboard onKeyPressed={keyboardKeyPressed} />
    </div>
  )
}

export default WordPuzzleGuessBoard;