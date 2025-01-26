import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface SearchedWord
{
    header: Header,
    body: {
        word: string
    }
}

export function BuildSearchedWord(word: string): SearchedWord
{
    return {
        header: 
        {
            type: "searchedWord", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
            word: word 
        }
    };
}