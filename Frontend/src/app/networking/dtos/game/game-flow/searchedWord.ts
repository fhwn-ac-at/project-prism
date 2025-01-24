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
            timestamp: Date.now()
        }, 
        body: 
        {
            word: word 
        }
    };
}