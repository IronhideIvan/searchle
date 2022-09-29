import { GraphQLError } from "./graphQLError"

export interface GraphQLResponse<T> {
  data?: T
  errors?: GraphQLError[]
};