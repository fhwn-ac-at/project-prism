import { Header } from "../../shared/header";

export interface ClosePath
{
    header: Header,
    body: 
    {    
    }
}

export function BuildClosePath(): ClosePath
{
    return {
        header: 
        {
            type: "closePath", 
            timestamp: Date.now()
        }, 
        body: 
        {
        }
    };
}