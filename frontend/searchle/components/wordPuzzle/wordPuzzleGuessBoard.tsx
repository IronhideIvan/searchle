import styles from "./word-puzzle.module.scss"
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import PuzzleKeyboard from "../keyboard/puzzleKeyboard";
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";
import React, { useState } from "react";
import WordPuzzleGuessWord from "./wordPuzzleGuessWord";
import { WordPuzzleBoard } from "../../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame } from "../../business/wordPuzzleGame";
import { Button, Grid, Text } from "@nextui-org/react";
import { doWordSearch } from "../../business/wordPuzzleSearch";
import { WordSearchResult } from "../../interfaces/api/wordSearchResult";
import WordSearchResults from "../dictionary/WordSearchResults";
import LoaderButton from "../common/LoaderButton";
import { convertToKeyboardKey } from "../../interfaces/keyboard/keyboardKeysConverter";
import { wordPuzzleValidator } from "../../business/WordPuzzleValidator";
import ResponsiveModal from "../common/ResponsiveModal";
import { apiErrorProcessor } from "../../business/apiErrorProcessor";
import ConfirmationButton from "../common/ConfirmationButton";

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
      if (validations.length === 0) {
        if (board.words.length > 10) {
          validations.push("No more than ten words are allowed at a time");
        }
        else {
          newBoard = wordPuzzleGame.addNewWord(board);
        }
      }
      setValidationMessages(validations);
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

      if (apiErrorProcessor.hasErrors(results)) {
        setValidationMessages(apiErrorProcessor.extractErrorMessages(results));
      }
      else if (!results.data
        || !results.data.wordSearch
        || results.data.wordSearch.length === 0) {
        setValidationMessages(["No Results Found"]);
      }
      else {
        setSearchResults(results.data!);
        setResultsVisible(true);
      }
    }
    finally {
      setTimeout(() => {
        setSearchInProgress(false);
      }, 1000);
    }
  }

  const closeModal = (): void => {
    setResultsVisible(false);
  }

  const clearBoard = (): void => {
    setBoard(wordPuzzleGame.createBoard(5));
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
            {
              validationMessages.map((m, index) => <li key={"val-" + index}><Text>{m}</Text></li>)
            }
          </ul>
        </div>
      ) : <></>}

      <div className={styles.wordPuzzleBtnContainer}>
        <LoaderButton
          color={"primary"}
          ghost={true}
          onPress={searchClicked}
          className={styles.wordPuzzleBtn}
          isLoading={searchInProgress}
        >
          Search
        </LoaderButton>
        <ConfirmationButton
          color={"error"}
          ghost
          className={styles.wordPuzzleBtn}
          onConfirmed={clearBoard}
          dialogConfirmText={"Clear"}
          dialogConfirmBtnColor={"error"}
          dialogCancelBtnColor={"default"}
          dialogContents={<Text>Are you sure you want to clear your search?</Text>}
        >
          Clear
        </ConfirmationButton>
      </div>

      <PuzzleKeyboard onKeyPressed={virtualKeyboardKeyPressed} />

      <ResponsiveModal
        closeButton
        aria-label="Popup with search results"
        open={resultsVisible}
        onClose={closeModal}
      >
        <WordSearchResults words={searchResults.wordSearch} />
      </ResponsiveModal>
    </div>
  )
}

export default WordPuzzleGuessBoard;