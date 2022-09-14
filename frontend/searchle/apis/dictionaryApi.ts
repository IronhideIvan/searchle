import axios from 'axios';
import { apiErrorProcessor } from '../business/apiErrorProcessor';
import { GraphQLRequest } from '../interfaces/api/graphQLRequest';
import { GraphQLResponse } from '../interfaces/api/graphQLResponse';
import { WordSearchResult } from '../interfaces/api/wordSearchResult';
import { baseApi } from './baseApi';

export async function searchWords(queryString: string): Promise<GraphQLResponse<WordSearchResult>> {
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

  try {
    const { data } = await axios.post<GraphQLResponse<WordSearchResult>>(
      baseApi.graphQLEndpoint,
      graphQLQuery,
      {
        headers: {
          "Content-Type": "application/json"
        }
      }
    );

    return baseApi.getSanitizedResponse(data);
  }
  catch (err) {
    return apiErrorProcessor.generateGraphQLError(err);
  }
}