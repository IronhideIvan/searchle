
export interface GraphQLRequest {
  operationName: string;
  query: string;
  variables: any;
}