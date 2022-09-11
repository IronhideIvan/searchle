import styles from "./word-puzzle.module.scss"
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import PuzzleKeyboard from "../keyboard/puzzleKeyboard";
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";
import React, { useState } from "react";
import WordPuzzleGuessWord from "./wordPuzzleGuessWord";
import { WordPuzzleBoard } from "../../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame } from "../../business/wordPuzzleGame";
import { Modal, Text } from "@nextui-org/react";
import { doWordSearch } from "../../business/wordPuzzleSearch";
import { WordSearchResult } from "../../interfaces/api/wordSearchResult";
import WordSearchResults from "../dictionary/WordSearchResults";
import LoaderButton from "../common/LoaderButton";
import { convertToKeyboardKey } from "../../interfaces/keyboard/keyboardKeysConverter";
import { wordPuzzleValidator } from "../../business/WordPuzzleValidator";

const WordPuzzleGuessBoard = () => {
  const [board, setBoard] = useState<WordPuzzleBoard>(wordPuzzleGame.createBoard(5));
  const [resultsVisible, setResultsVisible] = useState<boolean>(false);
  const [searchResults, setSearchResults] = useState<WordSearchResult>({ wordSearch: [] });
  const [searchInProgress, setSearchInProgress] = useState<boolean>(false);
  const [validationMessages, setValidationMessages] = useState<string[]>([]);

  const puzzleLetterTryChangeStatus = (letter: WordPuzzleLetter): void => {
    const newBoard = wordPuzzleGame.cycleLetterStatus(letter);
    setBoard(newBoard);
  }

  const virtualKeyboardKeyPressed = (keyboardKey: KeyboardKeys): void => {
    let newBoard: WordPuzzleBoard | null = null;
    if (keyboardKey === KeyboardKeys.Delete) {
      newBoard = wordPuzzleGame.backspace(board);
    }
    else if (keyboardKey === KeyboardKeys.Enter) {
      const latestWord = board.words[board.words.length - 1];
      const validations = wordPuzzleValidator.validateWord(latestWord, true);
      setValidationMessages(validations);
      if (validations.length > 0) {
        return;
      }
      else {
        newBoard = wordPuzzleGame.addNewWord(board);
      }
    }
    else {
      newBoard = wordPuzzleGame.addLetter(keyboardKey, board);
    }

    if (newBoard !== null) {
      // If we delete a word from the board, then reset the validatiom
      // messages.
      if (board.words.length > newBoard.words.length) {
        setValidationMessages([]);
      }
      setBoard(newBoard);
    }
  }

  const physicalKeyboardKeyPressed = (event: React.KeyboardEvent<HTMLDivElement>): void => {
    const virtualKey = convertToKeyboardKey(event.code);
    if (virtualKey !== null) {
      virtualKeyboardKeyPressed(virtualKey);
    }
  }

  const searchClicked = async (): Promise<void> => {
    const validations = wordPuzzleValidator.validateBoard(board, false);
    setValidationMessages(validations);
    if (validations.length > 0) {
      return;
    }

    try {
      setSearchInProgress(true);
      const results = await doWordSearch(board);
      setSearchResults(results);
      setResultsVisible(true);
    }
    finally {
      setSearchInProgress(false);
    }
  }

  const closeModal = (): void => {
    setResultsVisible(false);
  }

  return (
    <div className={styles.wordPuzzleBoardContainer}
      onKeyUp={physicalKeyboardKeyPressed}
      tabIndex={-1}
    >
      <div className={styles.wordPuzzleWordContainer}>
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

      {validationMessages.length > 0 ? (
        <div className={styles.wordPuzzleValidationContainer}>
          <ul>
            {validationMessages.map(m => <li><Text>{m}</Text></li>)}
          </ul>
        </div>
      ) : <></>}

      <div className={styles.wordPuzzleBtnContainer}>
        <LoaderButton
          color={"primary"}
          ghost={true}
          onPress={searchClicked}
          className={styles.wordPuzzleSearchBtn}
          isLoading={searchInProgress}
        >
          Search
        </LoaderButton>
      </div>

      <PuzzleKeyboard onKeyPressed={virtualKeyboardKeyPressed} />

      <Modal
        closeButton
        fullScreen
        aria-label="Popup with search results"
        open={resultsVisible}
        onClose={closeModal}
      >
        <div className={styles.searchResultsContainer}>
          <WordSearchResults words={searchResults.wordSearch} />
        </div>
      </Modal>
    </div>
  )
}

export default WordPuzzleGuessBoard;