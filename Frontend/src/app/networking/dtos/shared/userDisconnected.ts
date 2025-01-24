import { Header } from "./header";
import { User } from "./User";

export interface UserDisconnected
{
    header: Header,
    body: 
    {
        user: User
    }
}

export function BuildUserDisconnected(user: User): UserDisconnected
{
    return {
        header: 
        {
            type: "userDisconnected", 
            timestamp: Date.now()
        }, 
        body: 
        {
            user: user
        }
    };
}