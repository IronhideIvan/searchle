import { createTheme } from "@nextui-org/react"
import variables from "./../styles/variables.module.scss"
import { SharedTheme } from "./sharedTheme";

export const DarkTheme = createTheme({
  ...SharedTheme,
  type: "dark",
  theme: {
    colors: {
      puzzleTileDefaultText: variables.darkPuzzleTileDefaultTextColor,
      puzzleTileCorrectText: variables.darkPuzzleTileCorrectTextColor,
      puzzleTileCloseText: variables.darkPuzzleTileCloseTextColor,
      puzzleTileWrongText: variables.darkPuzzleTileWrongTextColor,

      puzzleTileDefaultBackground: variables.darkPuzzleTileDefaultBackgroundColor,
      puzzleTileCorrectBackground: variables.darkPuzzleTileCorrectBackgroundColor,
      puzzleTileCloseBackground: variables.darkPuzzleTileCloseBackgroundColor,
      puzzleTileWrongBackground: variables.darkPuzzleTileWrongBackgroundColor,

      puzzleTileBorder: variables.darkPuzzleTileBorderColor,
      keyboardBackground: variables.darkKeyboardBackgroundColor,
      keyboardKeyBackground: variables.darkKeyboardKeyBackgroundColor,
      keyboardKeyTextColor: variables.darkKeyboardKeyTextColor
    },
    space: {},
    fonts: {}
  }
});