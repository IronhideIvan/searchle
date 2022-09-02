import { WordPuzzleWord as WordPuzzleWord } from "./wordPuzzleWord";

export interface WordPuzzleBoard {
  words: WordPuzzleWord[];
  wordLength: number;
}