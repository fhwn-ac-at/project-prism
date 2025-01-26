import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface NextRound
{
    header: Header,
    body: 
    {   
        word: string,
        round: number,
        score: Map<string,number>
    }
}

export function BuildNextRound(round: number, word: string, score: Map<string, number>): NextRound
{
    return {
        header: 
        {
            type: "nextRound", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {  
            round: round,
            score: score,
            word: word
        }
    };
}