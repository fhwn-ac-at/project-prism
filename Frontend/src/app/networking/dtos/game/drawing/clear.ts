import { Header } from "../../shared/header";

export interface Clear
{
    header: Header,
    body: 
    {    
    }
}

export function BuildClear(): Clear
{
    return {
        header: 
        {
            type: "clear", 
            timestamp: Date.now()
        }, 
        body: 
        {
        }
    };
}