import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface GameEnded
{
    header: Header,
    body: 
    {    
    }
}

export function BuildGameEnded(): GameEnded
{
    return {
        header: 
        {
            type: "gameEnded", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
        }
    };
}