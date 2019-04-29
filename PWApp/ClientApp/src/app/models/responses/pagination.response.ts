export interface IPaginationResponse<T> {

  list: T[];

  TotalCount: number

  skipped?: number

  taken?: number

}
