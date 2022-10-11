import styles from "./WordSearchResults.module.scss";
import { Table, Text } from "@nextui-org/react";
import { Key } from "react";
import { WordSearchWord } from "../../interfaces/api/wordSearchResult";

interface WordSearchResultsProps {
  words: WordSearchWord[];
}

const WordSearchResults = (props: WordSearchResultsProps) => {
  const columns = [
    { name: "Word", uid: "word" }
  ];

  const getCellContents = (word: WordSearchWord, columnKey: Key): JSX.Element => {
    const wordKey = columnKey as keyof typeof word;

    if (columnKey === "word") {
      return (
        <Text className={styles.wordSearchWord}>{word[wordKey]}</Text>
      );
    }

    return (
      <Text>{word[wordKey]}</Text>
    );
  }

  return (
    <Table
      headerLined
      aria-label="Table of search results"
      bordered
      shadow={false}
    >
      <Table.Header columns={columns}>
        {(c) => (
          <Table.Column
            key={c.uid}
            align={"center"}
          >
            {c.name}
          </Table.Column>
        )}
      </Table.Header>
      <Table.Body items={props.words}>
        {(w) => (
          <Table.Row key={w.id}>
            {(columnKey) => {
              return <Table.Cell>{getCellContents(w, columnKey)}</Table.Cell>;
            }}
          </Table.Row>
        )}
      </Table.Body>
    </Table>
  );
}

export default WordSearchResults;