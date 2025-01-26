import { DateTime } from "luxon";
import { Header } from "../../shared/header";
import { User } from "../../shared/User";

export interface UserScore
{
    header: Header,
    body: {
        score: number,
        user: User
    }
}

export function BuildUserScore(score: number, user: User): UserScore
{
    return {
        header: 
        {
            type: "userScore", 
            timestamp: DateTime.now()
        }, 
        body: 
        {
            score: score,
            user: user
        }
    };
}