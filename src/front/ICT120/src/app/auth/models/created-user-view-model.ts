import { UserSummary } from "src/app/user/models/user-summary";
import { UserSession } from "./user-session";

export interface CreatedUserViewModel {
    user: UserSummary
    session: UserSession
}
