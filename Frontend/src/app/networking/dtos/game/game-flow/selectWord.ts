import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface SelectWord
{
    header: Header,
    body: {
        word: string
    }
}

export function BuildSelectWord(word: string): SelectWord
{
    return {
        header: 
        {
            type: "selectWord", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
            word: word 
        }
    };
}