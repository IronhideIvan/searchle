import type { NextPage } from 'next'
import WordPuzzleGuessBoard from '../components/wordPuzzle/wordPuzzleGuessBoard';
import PageRoot from "../components/common/PageRoot";

const Home: NextPage = () => {

  return (
    <PageRoot>
      <WordPuzzleGuessBoard />
    </PageRoot>
  )
}

export default Home
