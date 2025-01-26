import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface SetDrawer
{
    header: Header,
    body: 
    {
        words: {word: string, difficulty: number}[]
    }
}

export function BuildSetDrawer(words: {word: string, difficulty: number}[]): SetDrawer
{
    return {
        header: 
        {
            type: "setDrawer", 
            timestamp: DateTime.now()
        }, 
        body: 
        {
            words: words 
        }
    };
}