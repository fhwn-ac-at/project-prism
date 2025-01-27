import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface GuessClose
{
    header: Header,
    body: 
    {
        guess: string,
        distance: number
    }
}

export function BuildGuessClose(guess: string, distance: number)
{
    return {
        header: 
        {
            type: "guessClose", 
            timestamp: DateTime.now()
        }, 
        body: 
        {
            guess: guess,
            distance: distance
        }
    }
}