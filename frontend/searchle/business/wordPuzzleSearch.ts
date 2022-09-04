import { searchWords } from "../apis/dictionaryApi";
import { WordSearchWord } from "../interfaces/api/wordSearchResult";
import { WordPuzzleBoard } from "../interfaces/wordPuzzle/wordPuzzleBoard";
import { WordPuzzleLetterStatus } from "../interfaces/wordPuzzle/wordPuzzleLetter";

type WordPuzzleLetterMetadata = {
  letter: string;
  position: number;
}

type WordPuzzleBreakdown = {
  includesLetters: string[];
  correctPositionLetters: WordPuzzleLetterMetadata[];
  incorrectPositionLetters: WordPuzzleLetterMetadata[];
  invalidLetters: string[];
  wordLength: number;
}

export async function doWordSearch(board: WordPuzzleBoard): Promise<WordSearchWord[]> {
  const puzzleBreakdown = getPuzzleBreakdown(board);
  let searchQuery = `l:${puzzleBreakdown.wordLength} r:50 sp:y`;

  if (puzzleBreakdown.correctPositionLetters.length > 0) {
    searchQuery = searchQuery
      .concat(" pos:",
        ...puzzleBreakdown.correctPositionLetters.map((l) => `,${l.letter}|${l.position}`));
  }

  if (puzzleBreakdown.incorrectPositionLetters.length > 0) {
    searchQuery = searchQuery
      .concat(" dif:",
        ...puzzleBreakdown.incorrectPositionLetters.map((l) => `,${l.letter}|${l.position}`));
  }

  if (puzzleBreakdown.invalidLetters.length > 0) {
    searchQuery = searchQuery
      .concat(" ex:",
        ...puzzleBreakdown.invalidLetters);
  }

  if (puzzleBreakdown.includesLetters.length > 0) {
    searchQuery = searchQuery
      .concat(" in:",
        ...puzzleBreakdown.includesLetters);
  }

  console.log("searchQuery: " + searchQuery);

  return await (await searchWords(searchQuery)).wordSearch;
}

function getPuzzleBreakdown(board: WordPuzzleBoard): WordPuzzleBreakdown {
  const puzzleBreakdown: WordPuzzleBreakdown = {
    includesLetters: [],
    correctPositionLetters: [],
    incorrectPositionLetters: [],
    invalidLetters: [],
    wordLength: board.wordLength
  };

  if (board.words.length === 0 || board.wordLength < 1) {
    return puzzleBreakdown;
  }

  board.words.forEach((w) => {
    if (w.letters.length === 0) {
      return;
    }

    w.letters.forEach((l) => {
      // If letter is empty space or in an unresolved state, then just ignore it.
      if (l.letter === "_" || l.status === WordPuzzleLetterStatus.Unresolved) {
        return;
      }
      // If the letter doesn't exist in the word, then add it to the
      // appropriate array if it isn't already added.
      else if (l.status === WordPuzzleLetterStatus.NotExists) {
        let existsIndex = puzzleBreakdown.invalidLetters.indexOf(l.letter);
        if (existsIndex < 0) {
          puzzleBreakdown.invalidLetters.push(l.letter);
        }
      }
      // Otherwise the letter belongs to the word, we just need to check
      // if the letter is included in the appropriate array at the 
      // specified position of the word.
      else {
        const arr = l.status === WordPuzzleLetterStatus.CorrectPosition
          ? puzzleBreakdown.correctPositionLetters
          : puzzleBreakdown.incorrectPositionLetters;

        let isFound: boolean = false;
        for (let i = 0; i < arr.length; ++i) {
          const arrWord = arr[i];
          if (arrWord.letter === l.letter && arrWord.position === l.index) {
            isFound = true;
            break;
          }
        }
        if (!isFound) {
          arr.push({
            letter: l.letter,
            position: l.index
          });
        }

        // add it to the includes
        const existsIndex = puzzleBreakdown.includesLetters.indexOf(l.letter);
        if (existsIndex < 0) {
          puzzleBreakdown.includesLetters.push(l.letter);
        }
      }
    });
  });

  return puzzleBreakdown;
}