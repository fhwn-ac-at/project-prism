import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface NextRound
{
    header: Header,
    body: 
    {   
        word: string,
        round: number,
        score: object
    }
}

export function BuildNextRound(round: number, word: string, score: object): NextRound
{
    return {
        header: 
        {
            type: "nextRound", 
            timestamp: DateTime.now()
        }, 
        body: 
        {  
            round: round,
            score: score,
            word: word
        }
    };
}