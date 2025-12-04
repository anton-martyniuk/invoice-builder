export type Paged<T> = {
  items: T[];
  offset: number;
  limit: number;
  total: number;
};
