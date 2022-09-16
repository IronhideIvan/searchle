import { WordPuzzleBoard } from "../interfaces/wordPuzzle/wordPuzzleBoard";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../interfaces/wordPuzzle/wordPuzzleLetter";
import { WordPuzzleWord } from "../interfaces/wordPuzzle/wordPuzzleWord";
import { wordPuzzleGame } from "./wordPuzzleGame";

class WordPuzzleValidator {

  validateWord(word: WordPuzzleWord, ignoreStatus: boolean): string[] {
    const validationMessages: string[] = [];

    let hasBlankSpaces: boolean = false;
    let hasUnresolvedStatuses: boolean = false;

    // For each letter
    word.letters.forEach(l => {
      // Check that it is not a blank space
      if (l.letter === wordPuzzleGame.getDefaultLetter()) {
        hasBlankSpaces = true;
      }

      // Check that it has a status set (if we are not ignoring statuses)
      if (!ignoreStatus && l.status === WordPuzzleLetterStatus.Unresolved) {
        hasUnresolvedStatuses = true;
      }
    });

    if (hasBlankSpaces) {
      validationMessages.push("Do not leave blank spaces in words.");
    }

    if (hasUnresolvedStatuses) {
      validationMessages.push("Every letter must have a color set. Click on a letter to give it a color.");
    }

    return validationMessages;
  }

  validateBoard(board: WordPuzzleBoard, ignoreStatus: boolean): string[] {
    const validationMessages: string[] = [];

    let hasNoWords = false;

    if (!board.words || board.words.length === 0) {
      hasNoWords = true;
    }

    // Filter out all empty words
    const filteredWords = hasNoWords ? [] : board.words.filter((w) => {
      return w.letters.some(l => l.letter !== wordPuzzleGame.getDefaultLetter());
    });

    // Check that there is at least one word
    if (filteredWords.length === 0) {
      hasNoWords = true;
    }

    // Validate each word
    for (let i = 0; i < filteredWords.length; ++i) {
      const wordValidations = this.validateWord(filteredWords[i], ignoreStatus);
      if (wordValidations.length > 0) {
        validationMessages.push(...wordValidations);
        break;
      }
    }

    if (hasNoWords) {
      validationMessages.push("Enter at least 1 word.");
    }

    return validationMessages;
  }
}

export const wordPuzzleValidator = new WordPuzzleValidator();