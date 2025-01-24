import { Header } from "./header";
import { User } from "./User";

export interface UserJoined
{
    header: Header,
    body: 
    {
        user: User
    }
}

export function BuildUserUserJoined(user: User): UserJoined
{
    return {
        header: 
        {
            type: "userJoined", 
            timestamp: Date.now()
        }, 
        body: 
        {
            user: user
        }
    };
}