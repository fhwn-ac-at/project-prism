import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface Undo
{
    header: Header,
    body: 
    {    
    }
}

export function BuildUndo(): Undo
{
    return {
        header: 
        {
            type: "undo", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
        }
    };
}