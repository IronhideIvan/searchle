import styles from "./word-puzzle.module.scss"
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import PuzzleKeyboard from "../keyboard/puzzleKeyboard";
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";
import { useState } from "react";
import WordPuzzleGuessWord from "./wordPuzzleGuessWord";
import { WordPuzzleBoard } from "../../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame } from "../../business/wordPuzzleGame";
import { Modal, styled } from "@nextui-org/react";
import { doWordSearch } from "../../business/wordPuzzleSearch";
import { WordSearchResult } from "../../interfaces/api/wordSearchResult";
import WordSearchResults from "../dictionary/WordSearchResults";
import LoaderButton from "../common/LoaderButton";

const WordPuzzleGuessWordContainer = styled('div');

const WordPuzzleGuessBoard = () => {
  const [board, setBoard] = useState<WordPuzzleBoard>(wordPuzzleGame.createBoard(5));
  const [resultsVisible, setResultsVisible] = useState<boolean>(false);
  const [searchResults, setSearchResults] = useState<WordSearchResult>({ wordSearch: [] });
  const [searchInProgress, setSearchInProgress] = useState<boolean>(false);

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
    <div className={styles.wordPuzzleBoardContainer}>
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

      <PuzzleKeyboard onKeyPressed={keyboardKeyPressed} />

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