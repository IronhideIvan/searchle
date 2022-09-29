import { Button, Modal, Navbar, Text } from "@nextui-org/react";
import { useState } from "react";
import WordPuzzleHelp from "../wordPuzzle/wordPuzzleHelp";
import ResponsiveModal from "./ResponsiveModal";

const SearchleNavbar = () => {
  const [openHelp, setOpenHelp] = useState<boolean>(false);

  const closeHelpModal = () => {
    setOpenHelp(false);
  }

  const openHelpModal = () => {
    setOpenHelp(true);
  }

  return (
    <Navbar
      isBordered
      isCompact
    // shouldHideOnScroll
    // variant={"sticky"}
    >
      <Navbar.Brand>
        <Text b>Searchle</Text>
      </Navbar.Brand>

      <Navbar.Content>
        <Navbar.Item id="navbar-help">
          <div>
            <Button
              auto
              flat
              onClick={openHelpModal}
            >
              Help
            </Button>
            <ResponsiveModal
              closeButton
              aria-label="Searchle help page popup"
              open={openHelp}
              onClose={closeHelpModal}
            >
              <WordPuzzleHelp />
            </ResponsiveModal>
          </div>
        </Navbar.Item>
      </Navbar.Content>
    </Navbar>
  );
}

export default SearchleNavbar;