import styles from "./word-puzzle-help.module.scss";
import { Container, Text } from "@nextui-org/react";
import { WordPuzzleLetterStatus } from "../../interfaces/wordPuzzle/wordPuzzleLetter";
import WordPuzzleGuessLetter from "./wordPuzzleGuessLetter";
import WordPuzzleGuessWord from "./wordPuzzleGuessWord";

interface Props {

}

const WordPuzzleHelp = (props: Props) => {
  return (
    <Container className={styles.puzzleHelpContainer}>
      <Text h2>
        What is Searchle?
      </Text>
      <Text>
        Searchle is a tool to help you solve those pesky words you just can't figure out when solving a
        word puzzle game like <Text b>Wordle</Text>.
      </Text>
      <Text h2>
        How does it work?
      </Text>
      <Text>
        It sort of functions like a dictionary, just with a very specific way of searching for words.
        To get started, type out a 5 letter word. For example, let's put in:
      </Text>
      <WordPuzzleGuessWord
        index={0}
        letters={[
          {
            letter: "D",
            index: 0,
            status: WordPuzzleLetterStatus.NotExists
          },
          {
            letter: "R",
            index: 1,
            status: WordPuzzleLetterStatus.NotExists
          },
          {
            letter: "I",
            index: 2,
            status: WordPuzzleLetterStatus.NotExists
          },
          {
            letter: "V",
            index: 3,
            status: WordPuzzleLetterStatus.NotExists
          },
          {
            letter: "E",
            index: 4,
            status: WordPuzzleLetterStatus.NotExists
          }
        ]}
      />
      <Text className={styles.topMargin}>
        If you perform a search now you will be given a list of words that don't contain any of the
        letters in the word. Why? Because the default status of a letter (which you can identify from the
        black background) means that this letter does not exist in the word. If we want to change the status,
        we can do so by clicking on a letter to cycle it between three possible statuses. Now, what are the
        statuses and what do they mean, you might be asking? Well, they are:
      </Text>
      <WordPuzzleGuessWord
        index={0}
        letters={[
          {
            letter: "D",
            index: 0,
            status: WordPuzzleLetterStatus.CorrectPosition
          },
          {
            letter: "R",
            index: 1,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "I",
            index: 2,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "V",
            index: 3,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "E",
            index: 4,
            status: WordPuzzleLetterStatus.Unresolved
          }
        ]}
      />
      <Text>
        <Text small>
          Green. This color means that this letter both exists in the word and is in the correct position.
          Using our example, it means that our word starts with the letter "D".
        </Text>
      </Text>
      <WordPuzzleGuessWord
        index={0}
        letters={[
          {
            letter: "D",
            index: 0,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "R",
            index: 1,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "I",
            index: 2,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "V",
            index: 3,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "E",
            index: 4,
            status: WordPuzzleLetterStatus.IncorrectPosition
          }
        ]}
      />
      <Text>
        <Text small>
          Yellow. This color means that the letter exists in the word, but it is not in the correct
          position. In our example it means that the letter "E" is in our word, but it's not the last
          letter of our word.
        </Text>
      </Text>
      <WordPuzzleGuessWord
        index={0}
        letters={[
          {
            letter: "D",
            index: 0,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "R",
            index: 1,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "I",
            index: 2,
            status: WordPuzzleLetterStatus.Unresolved
          },
          {
            letter: "V",
            index: 3,
            status: WordPuzzleLetterStatus.NotExists
          },
          {
            letter: "E",
            index: 4,
            status: WordPuzzleLetterStatus.Unresolved
          }
        ]}
      />
      <Text>
        <Text small>
          Black. This color means that the letter does not exist in the word. In our example, this
          means that the letter "V" is not in any part of our word.
        </Text>
      </Text>
      <Text>
        Once you've set the status for all of your letters you can go ahead and hit the search button
        to see what words this could possibly be.
      </Text>
      <Text b>
        OR
      </Text>
      <Text className={styles.topMargin}>
        You can hit the Enter key to add a new word to the search to narrow down our words even more. You can
        have up to 10 words in your search!
      </Text>
      <Text b size={"$2xl"}>
        Happy puzzling!
      </Text>
    </Container>
  );
}

export default WordPuzzleHelp;
