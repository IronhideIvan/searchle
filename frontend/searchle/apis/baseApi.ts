import { apiErrorProcessor } from "../business/apiErrorProcessor";
import { GraphQLResponse } from "../interfaces/api/graphQLResponse";

export const baseApi = {
  graphQLEndpoint: "/graphql",
  getSanitizedResponse<T>(resp: GraphQLResponse<T>): GraphQLResponse<T> {
    if (!resp || !resp.data) {
      return apiErrorProcessor.generateGraphQLError();
    }
    else {
      return resp;
    }
  }
}