import { searchWords } from "../apis/dictionaryApi";
import { GraphQLResponse } from "../interfaces/api/graphQLResponse";
import { WordSearchResult, WordSearchWord } from "../interfaces/api/wordSearchResult";
import { WordPuzzleBoard } from "../interfaces/wordPuzzle/wordPuzzleBoard";
import { wordPuzzleGame } from "./wordPuzzleGame";

export async function doWordSearch(board: WordPuzzleBoard): Promise<GraphQLResponse<WordSearchResult>> {
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

  if (puzzleBreakdown.instanceCounts.length > 0) {
    searchQuery = searchQuery
      .concat(" cnt:",
        ...puzzleBreakdown.instanceCounts.map((l) => `,${l.letter}|${l.isExact ? "=" : ">="}|${l.count}`));
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

  return await searchWords(searchQuery);
}
