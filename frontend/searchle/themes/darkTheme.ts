import { createTheme } from "@nextui-org/react"
import variables from "./../styles/variables.module.scss"
import { SharedTheme } from "./sharedTheme";

export const DarkTheme = createTheme({
  ...SharedTheme,
  type: "dark",
  theme: {
    colors: {
      puzzleLetter: variables.darkPuzzleLetterColor,
      puzzleTileBorder: variables.darkPuzzleTileBorderColor
    },
    space: {},
    fonts: {}
  }
});