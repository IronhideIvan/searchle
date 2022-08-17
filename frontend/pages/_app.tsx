import { ChakraProvider } from "@chakra-ui/react"

const PortfolioApp = ({Component, pageProps}) => {
  return (
    <ChakraProvider>
      <Component {...pageProps} />
    </ChakraProvider>
  );
};

export default PortfolioApp;