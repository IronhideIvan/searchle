import { Box, Flex, SimpleGrid, Stack, Image, Text, useColorModeValue, Heading } from '@chakra-ui/react'
import Link from 'next/link'
import React from 'react'
import Layout from '../components/Layout'

const IndexPage = () => (
  <Layout title="Home | Next.js + TypeScript Example">
    <Flex w="100%" h="90vh" 
      justifyContent="center"
      alignItems="center"
      backgroundImage={"url('19449741.jpg')"}
      backgroundPosition="center"
      backgroundRepeat={"no-repeat"}
      bgSize={"100%"}>
        <Heading>Arthur Lisek-Koper</Heading>
    </Flex>
    <Flex w="100%" h="90vh" 
      bgGradient="linear(to-br, cyan.200, blue.500)" 
      justifyContent="center"
      alignItems="center">

      <SimpleGrid 
        columns={{base: 1, md: 2}} 
        spacing={4}
        bg={"white"}
        justifyContent="center"
        alignItems="center"
        >
        
        <Stack spacing={4}>
          <Heading>Who's this guy?</Heading>
          <Text fontSize={"lg"}>
            Some jerkoff just trying to put together a website..
          </Text>
        </Stack>
        <Flex maxW={"150px"}>
          <Image 
            borderRadius={"full"}
            boxSize={"150px"}
            rounded={"md"}
            alt={"feature image"}
            src={"https://images.unsplash.com/photo-1552058544-f2b08422138a?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=1299&q=80"}
            objectFit={"cover"}
          />
        </Flex>
      </SimpleGrid>
    </Flex>
    <a href="https://www.freepik.com/vectors/background">Background vector created by starline - www.freepik.com</a>
  </Layout>
)

export default IndexPage
