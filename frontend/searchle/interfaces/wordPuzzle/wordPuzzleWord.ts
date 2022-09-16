import { WordPuzzleBoard } from "./wordPuzzleBoard";
import { WordPuzzleLetter } from "./wordPuzzleLetter";

export interface WordPuzzleWord {
  letters: WordPuzzleLetter[];
  index: number;
  parent?: WordPuzzleBoard;
}