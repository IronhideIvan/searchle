import { Table } from "@nextui-org/react";
import { WordSearchWord } from "../../interfaces/api/wordSearchResult";

interface WordSearchResultsProps {
  words: WordSearchWord[];
}

const WordSearchResults = (props: WordSearchResultsProps) => {
  const columns = [
    { name: "Word", uid: "word" }
  ];

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
              return <Table.Cell>{w[columnKey as keyof typeof w]}</Table.Cell>;
            }}
          </Table.Row>
        )}
      </Table.Body>
    </Table>
  );
}

export default WordSearchResults;