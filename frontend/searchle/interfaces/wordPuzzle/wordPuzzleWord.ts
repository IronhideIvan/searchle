import { WordPuzzleLetter } from "./wordPuzzleLetter";

export interface WordPuzzleWord {
  letters: WordPuzzleLetter[];
  index: number;
}