import { searchWords } from "../apis/dictionaryApi";
import { WordSearchWord } from "../interfaces/api/wordSearchResult";
import { WordPuzzleBoard } from "../interfaces/wordPuzzle/wordPuzzleBoard";

export async function doWordSearch(board: WordPuzzleBoard): Promise<WordSearchWord[]> {
  return await (await searchWords("l:5 r:50 sp:y in:ae ex:rtdvipl pos:e|4,a|2 dif:a|0,o|0")).wordSearch;
}