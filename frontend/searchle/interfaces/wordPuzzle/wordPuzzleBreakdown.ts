export type WordPuzzleLetterMetadata = {
  letter: string;
  position: number;
}

export type WordPuzzleBreakdown = {
  includesLetters: string[];
  correctPositionLetters: WordPuzzleLetterMetadata[];
  incorrectPositionLetters: WordPuzzleLetterMetadata[];
  invalidLetters: string[];
  wordLength: number;
}