import { Header } from "../../shared/header";

export interface NextRound
{
    header: Header,
    body: {}
}

export function BuildNextRound(): NextRound
{
    return {
        header: 
        {
            type: "nextRound", 
            timestamp: Date.now()
        }, 
        body: 
        {
        }
    };
}