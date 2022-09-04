import axios from 'axios';
import { GraphQLRequest } from '../interfaces/api/graphQLRequest';
import { GraphQLResponse } from '../interfaces/api/graphQLResponse';
import { WordSearchResult } from '../interfaces/api/wordSearchResult';
import { baseApi } from './baseApi';

export async function searchWords(queryString: string): Promise<WordSearchResult> {
  const graphQLQuery: GraphQLRequest = {
    operationName: "WordSearch",
    query: `query WordSearch($q: String!) {
  wordSearch(queryString: $q){
    id,
    word
  }
}`,
    variables: {
      q: queryString
    }
  }

  const { data, status } = await axios.post<GraphQLResponse<WordSearchResult>>(
    baseApi.graphQLEndpoint,
    graphQLQuery,
    {
      headers: {
        "Content-Type": "application/json"
      }
    }
  );

  return data.data;

}