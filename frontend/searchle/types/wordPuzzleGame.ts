import { WordPuzzleBoard } from "../interfaces/wordPuzzle/wordPuzzleBoard";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../interfaces/wordPuzzle/wordPuzzleLetter";
import { WordPuzzleWord } from "../interfaces/wordPuzzle/wordPuzzleWord";

const deepCopy = (board: WordPuzzleBoard): WordPuzzleBoard => {
  const newBoard: WordPuzzleBoard = {
    wordLength: board.wordLength,
    words: []
  };

  board.words.forEach(w => {
    const newWord: WordPuzzleWord = {
      parent: newBoard,
      letters: [],
      index: w.index
    }

    w.letters.forEach(l => {
      const newLetter: WordPuzzleLetter = {
        ...l,
        parent: newWord
      }
      newWord.letters.push(newLetter);
    });

    newBoard.words.push(newWord);
  });

  return newBoard;
}

export class WordPuzzleGame {
  readonly defaultLetter: string = "_";

  createBoard(wordLength: number): WordPuzzleBoard {
    const board = {
      words: [],
      wordLength: wordLength
    }

    return this.addNewWord(board);
  }

  getNextLetterStatus(status: WordPuzzleLetterStatus): WordPuzzleLetterStatus {
    let newStatus = status;

    if (status === WordPuzzleLetterStatus.Unresolved) {
      newStatus = WordPuzzleLetterStatus.CorrectPosition;
    }
    else if (status === WordPuzzleLetterStatus.CorrectPosition) {
      newStatus = WordPuzzleLetterStatus.IncorrectPosition;
    }
    else if (status === WordPuzzleLetterStatus.IncorrectPosition) {
      newStatus = WordPuzzleLetterStatus.NotExists;
    }
    else if (status === WordPuzzleLetterStatus.NotExists) {
      newStatus = WordPuzzleLetterStatus.Unresolved;
    }

    return newStatus;
  }

  cycleLetterStatus(letter: WordPuzzleLetter): WordPuzzleBoard {
    let newStatus = this.getNextLetterStatus(letter.status);

    const word = letter.parent;

    const boardCopy = deepCopy(word.parent);
    const wordCopy = boardCopy.words[word.index];
    const oldLetterCopy = wordCopy.letters[letter.index];

    const newLetter = {
        ...oldLetterCopy,
        status: newStatus
      };

    wordCopy.letters[newLetter.index] = newLetter;

    return boardCopy;
  }

  addNewWord(board: WordPuzzleBoard): WordPuzzleBoard{
    const boardCopy = deepCopy(board);

    const word: WordPuzzleWord = {
      letters: [],
      index: boardCopy.words.length,
      parent: boardCopy
    };

    word.letters = Array.from({length: boardCopy.wordLength}, (v, i): WordPuzzleLetter => {
        return {
          letter: this.defaultLetter,
          index: i,
          status: WordPuzzleLetterStatus.Unresolved,
          parent: word
        }
      });

    boardCopy.words.push(word);

    return boardCopy;
  }

  addLetter(character: string, board: WordPuzzleBoard): WordPuzzleBoard | null {
    if(board.words.length === 0){
      return null;
    }

    const word = board.words[board.words.length - 1];
    const nextBlankIndex = word.letters.map((l) => l.letter).indexOf(this.defaultLetter);
    if(nextBlankIndex < 0){
      return null;
    }

    const boardCopy = deepCopy(board);
    const wordCopy = boardCopy.words[word.index];
    const oldLetter = wordCopy.letters[nextBlankIndex];

    wordCopy.letters[nextBlankIndex] = {
      letter: character,
      index: oldLetter.index,
      parent: wordCopy,
      status: oldLetter.status
    };


    return boardCopy;
  }

  backspace(board: WordPuzzleBoard): WordPuzzleBoard | null {
    if(board.words.length === 0){
      return null;
    }

    const word = board.words[board.words.length - 1];
    let letterIndex = -1;
    for(let i = 0; i < word.letters.length; ++i){
      if(word.letters[i].letter === this.defaultLetter){
        letterIndex = i;
        break;
      }
    }

    if(letterIndex === 0){
      // don't do anything if we are on the first word.
      if(board.words.length === 1){
        return null;
      }

      // if we don't have anything else then
      // delete the latest word.
      const boardCopy = deepCopy(board);
      boardCopy.words.pop();
      
      return boardCopy;
    }

    if(letterIndex === -1){
      letterIndex = word.letters.length;
    }

    const boardCopy = deepCopy(board);
    const oldWord = boardCopy.words[word.index];
    const oldLetter = oldWord.letters[letterIndex - 1];
    oldWord.letters[letterIndex - 1] = {
      ...oldLetter,
      letter: this.defaultLetter
    };

    return boardCopy;
  }
}

export const wordPuzzleGame = new WordPuzzleGame();
