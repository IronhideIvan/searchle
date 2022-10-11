export interface GraphQLErrorExtensions {
  code?: string;
  message?: string;
}

export interface GraphQLErrorLocation {
  line: string;
  column: string;
}

export interface GraphQLError {
  message?: string;
  locations?: GraphQLErrorLocation[];
  path?: string[];
  extensions?: GraphQLErrorExtensions;
}