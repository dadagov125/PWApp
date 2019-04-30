import { IPaginationResponse } from "./pagination.response";
import { UserResponse } from "./user.response";

export class UsersListResponse implements IPaginationResponse<UserResponse> {
  TotalCount: number;
  list: UserResponse[];
  skipped?: number;
  taken?: number;

}
