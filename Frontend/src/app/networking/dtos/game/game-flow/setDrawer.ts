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
            timestamp: Date.now()
        }, 
        body: 
        {
            words: words 
        }
    };
}