import type { NextPage } from 'next'
import WordPuzzleGuessBoard from '../components/wordPuzzle/wordPuzzleGuessBoard';
import PageRoot from "../components/common/PageRoot";
import SearchleNavbar from '../components/common/SearchleNavbar';

const Home: NextPage = () => {

  return (
    <PageRoot>
      <header>
        <SearchleNavbar />
      </header>
      <WordPuzzleGuessBoard />
    </PageRoot>
  )
}

export default Home
