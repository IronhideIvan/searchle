import { createTheme } from "@nextui-org/react"
import variables from "./../styles/variables.module.scss"
import { SharedTheme } from "./sharedTheme";

export const DarkTheme = createTheme({
  ...SharedTheme,
  type: "dark",
  theme: {
    colors: {
      // main
      background: "$gray50",
      foreground: "$gray900",

      // custom
      puzzleTileDefaultText: "$foreground",
      puzzleTileCorrectText: "$black",
      puzzleTileCloseText: "$black",
      puzzleTileWrongText: "$foreground",

      puzzleTileDefaultBackground: "none",
      puzzleTileCorrectBackground: "$success",
      puzzleTileCloseBackground: "$warning",
      puzzleTileWrongBackground: "$black",

      puzzleTileBorder: "rgb(222, 240, 255, .3)",
      keyboardBackground: "none",
      keyboardKeyBackground: "$gray500",
      keyboardKeyTextColor: "$gray900",

      colorScheme: "dark"

    },
    space: {},
    fonts: {}
  }
});