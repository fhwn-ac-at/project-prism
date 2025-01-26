import { DateTime } from "luxon";
import { Header } from "../shared/header";

export interface GameStarted
{
    header: Header,
    body: 
    {    
    }
}

export function BuildGameStarted(): GameStarted
{
    return {
        header: 
        {
            type: "gameStarted", 
            timestamp: DateTime.now()
        }, 
        body: 
        {
        }
    };
}