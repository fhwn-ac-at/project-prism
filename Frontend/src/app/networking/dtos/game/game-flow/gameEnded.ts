import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface GameEnded
{
    header: Header,
    body: 
    {
        word: string,
        score: object    
    }
}

export function BuildGameEnded(word: string, score: object): GameEnded
{
    return {
        header: 
        {
            type: "gameEnded", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
            word: word,
            score: score
        }
    };
}