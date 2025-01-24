import { Header } from "../../shared/header";

export interface DrawingSizeChanged
{
    header: Header,
    body: 
    {
        size: number
    }
}

export function BuildDrawingSizeChanged(size: number): DrawingSizeChanged
{
    return {
        header: 
        {
            type: "drawingSizeChanged", 
            timestamp: Date.now()
        }, 
        body: 
        {
            size: size
        }
    };
}