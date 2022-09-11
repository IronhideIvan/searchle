export type WordPuzzleLetterMetadata = {
  letter: string;
  position: number;
}

export type WordPuzzleLetterInstanceCount = {
  letter: string;
  count: number;
  isExact: boolean;
}

export type WordPuzzleBreakdown = {
  includesLetters: string[];
  correctPositionLetters: WordPuzzleLetterMetadata[];
  incorrectPositionLetters: WordPuzzleLetterMetadata[];
  invalidLetters: string[];
  wordLength: number;
  instanceCounts: WordPuzzleLetterInstanceCount[];
}