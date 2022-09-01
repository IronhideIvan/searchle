import styles from "./word-puzzle.module.scss"
import { styled, Text } from "@nextui-org/react";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import { useState } from "react";

interface WordPuzzleGuessLetterProps extends WordPuzzleLetter {
  onClick?(puzzleLetter: WordPuzzleLetter): void;
}

const WordPuzzleGuessLetterTile = styled('div');

const WordPuzzleGuessLetter = (props: WordPuzzleGuessLetterProps) => {

  const onClicked = () => {
    // console.log("puzzle letter clicked: " + props.letter);
    if (props.onClick) {
      props.onClick(props);
    }
  }

  const tileBgColor =
    props.status === WordPuzzleLetterStatus.Unresolved ? "$puzzleTileDefaultBackground"
      : props.status === WordPuzzleLetterStatus.CorrectPosition ? "$puzzleTileCorrectBackground"
        : props.status === WordPuzzleLetterStatus.IncorrectPosition ? "$puzzleTileCloseBackground"
          : "$puzzleTileWrongBackground";

  const tileTextColor =
    props.status === WordPuzzleLetterStatus.Unresolved ? "$puzzleTileDefaultText"
      : props.status === WordPuzzleLetterStatus.CorrectPosition ? "$puzzleTileCorrectText"
        : props.status === WordPuzzleLetterStatus.IncorrectPosition ? "$puzzleTileCloseText"
          : "$puzzleTileWrongText";

  return (
    <WordPuzzleGuessLetterTile
      css={{ backgroundColor: tileBgColor }}
      className={styles.puzzleLetterBox}
      onClick={onClicked}>
      <Text
        className={styles.puzzleLetter}
        css={{ color: tileTextColor }}>{props.letter}</Text>
    </WordPuzzleGuessLetterTile>
  );
}

export default WordPuzzleGuessLetter;