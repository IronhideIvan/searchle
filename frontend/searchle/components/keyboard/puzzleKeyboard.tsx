import styles from "./puzzle-keyboard.module.scss"
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";
import PuzzleKeyboardKey from "./puzzleKeyboardKey";
import { styled } from "@nextui-org/react";

interface PuzzleKeyboardProps {
  onKeyPressed?(keyboardKey: KeyboardKeys): any;
}

const keyListRow1: KeyboardKeys[] = [
  KeyboardKeys.Q,
  KeyboardKeys.W,
  KeyboardKeys.E,
  KeyboardKeys.R,
  KeyboardKeys.T,
  KeyboardKeys.Y,
  KeyboardKeys.U,
  KeyboardKeys.I,
  KeyboardKeys.O,
  KeyboardKeys.P,
];

const keyListRow2: KeyboardKeys[] = [
  KeyboardKeys.A,
  KeyboardKeys.S,
  KeyboardKeys.D,
  KeyboardKeys.F,
  KeyboardKeys.G,
  KeyboardKeys.H,
  KeyboardKeys.J,
  KeyboardKeys.K,
  KeyboardKeys.L,
];

const keyListRow3: KeyboardKeys[] = [
  KeyboardKeys.Z,
  KeyboardKeys.X,
  KeyboardKeys.C,
  KeyboardKeys.V,
  KeyboardKeys.B,
  KeyboardKeys.N,
  KeyboardKeys.M,
];

const KeyboardElement = styled('div', {
  backgroundColor: "$keyboardBackground"
});

const PuzzleKeyboard = (props: PuzzleKeyboardProps) => {
  const onKeyPressed = (keyboardKey: KeyboardKeys): void => {
    if(props.onKeyPressed){
      props.onKeyPressed(keyboardKey);
    }
  }

  const buildKeyboardKeys = (keys: KeyboardKeys[]): JSX.Element[] => {
    return keys.map((k) => (
      <PuzzleKeyboardKey key={k} keyboardKey={k} onPress={onKeyPressed} />
    ));
  }

  return (
    <KeyboardElement className={styles.puzzleKeyboardContainer}>
      <div className={styles.puzzleKeyboardRow}>
        {buildKeyboardKeys(keyListRow1)}
      </div>
      <div className={styles.puzzleKeyboardRow}>
        {buildKeyboardKeys(keyListRow2)}
      </div>
      <div className={styles.puzzleKeyboardRow}>
        {buildKeyboardKeys(keyListRow3)}
      </div>
    </KeyboardElement>
  );
}

export default PuzzleKeyboard;