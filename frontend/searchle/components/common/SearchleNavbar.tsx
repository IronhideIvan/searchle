import { Button, Navbar, Text } from "@nextui-org/react";

const SearchleNavbar = () => {
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
        <Navbar.Item>
          <Button auto flat>Help</Button>
        </Navbar.Item>
      </Navbar.Content>
    </Navbar>
  );
}

export default SearchleNavbar;