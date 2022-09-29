import styles from "./puzzle-keyboard.module.scss"
import { styled, Text } from "@nextui-org/react";
import { KeyboardKeys } from "../../interfaces/keyboard/keyboardKeys";

interface PuzzleKeyboardKeyProps {
  keyboardKey: KeyboardKeys;
  display?: JSX.Element;
  onPress?(keyboardKey: KeyboardKeys): any;
}

const KeyboardButton = styled("button", {
  color: "$keyboardKeyTextColor",
  backgroundColor: "$keyboardKeyBackground"
})

const PuzzleKeyboardKey = (props: PuzzleKeyboardKeyProps) => {
  const onKeyPress: any = () => {
    // console.log("clicked: " + props.keyboardKey);
    if(props.onPress){
      props.onPress(props.keyboardKey);
    }
  }

  return (
    <KeyboardButton type="button" onClick={onKeyPress} className={styles.puzzleKey}>
      {props.display ? props.display : (
        <Text 
        className={styles.puzzleKeyLetter}
        css={{
          color: "$keyboardKeyTextColor"
        }}>{props.keyboardKey}</Text>
      )}
    </KeyboardButton>
  );
}

export default PuzzleKeyboardKey;