import { searchWords } from "../apis/dictionaryApi";
import { WordSearchWord } from "../interfaces/api/wordSearchResult";
import { WordPuzzleBoard } from "../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame } from "./wordPuzzleGame";

export async function doWordSearch(board: WordPuzzleBoard): Promise<WordSearchWord[]> {
  const puzzleBreakdown = wordPuzzleGame.getPuzzleBreakdown(board);
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
