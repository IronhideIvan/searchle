import styles from "./word-puzzle.module.scss"
import { Table } from "@nextui-org/react";
import { WordPuzzleLetter, WordPuzzleLetterStatus } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import WordPuzzleGuessLetter from "./wordPuzzleGuessLetter";
import { WordPuzzleWord } from "../../interfaces/wordPuzzle/wordPuzzleWord";
import { WordSearchWord } from "../../interfaces/api/wordSearchResult";

interface WordPuzzleSearchResultsProps {
  words: WordSearchWord[];
}

const WordPuzzleSearchResults = (props: WordPuzzleSearchResultsProps) => {
  const columns = [
    { name: "Word", uid: "word" }
  ];

  return (
    <Table aria-label="Table of search results">
      <Table.Header columns={columns}>
        {(c) => (
          <Table.Column
            key={c.uid}
            align={"start"}
          >
            {c.name}
          </Table.Column>
        )}
      </Table.Header>
      <Table.Body items={props.words}>
        {(w) => (
          <Table.Row key={w.id}>
            {(columnKey) => {
              return <Table.Cell>{w[columnKey as keyof typeof w]}</Table.Cell>;
            }}
          </Table.Row>
        )}
      </Table.Body>
    </Table>
  );
}

export default WordPuzzleSearchResults;