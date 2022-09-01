export enum WordPuzzleLetterStatus {
  Unresolved,
  CorrectPosition,
  IncorrectPosition,
  NotExists
}

export interface WordPuzzleLetter {
  letter: string;
  status: WordPuzzleLetterStatus;
  index: number;
}