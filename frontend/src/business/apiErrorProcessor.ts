import { GraphQLResponse } from "../interfaces/api/graphQLResponse";


class ApiErrorProcessor {
  generateGraphQLError<T>(err?: any): GraphQLResponse<T> {
    const genericGraphQLError: GraphQLResponse<T> = {
      errors: [
        {
          message: "Woops, we ran into an issue handling your action! Maybe try again in a few minutes?"
        }
      ]
    };

    return genericGraphQLError;
  }

  extractErrorMessages<T>(resp: GraphQLResponse<T>): string[] {
    const errArr: string[] = [];

    if (!this.hasErrors(resp)) {
      return errArr;
    }

    return resp.errors!
      .filter(e => e.message && e.message.length > 0)
      .map(e => e.message) as string[];
  }

  hasErrors<T>(resp: GraphQLResponse<T>): boolean {
    return (resp.errors && resp.errors.length > 0) === true;
  }
}

export const apiErrorProcessor = new ApiErrorProcessor();
