import styles from "./puzzle-keyboard.module.scss"
import { Text, theme } from "@nextui-org/react";
import { WordPuzzleLetter } from "../../interfaces/wordPuzzle/wordPuzzleLetter";

interface PuzzleKeyboardProps {
}

const PuzzleKeyboard = (props: PuzzleKeyboardProps) => {
  return (
    <div className={""}>
      <Text css={{
        color: '$puzzleLetter'
      }}>TEST</Text>
    </div>
  );
}

export default PuzzleKeyboard;