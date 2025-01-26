import { DateTime } from "luxon";
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
            timestamp: DateTime.now()
        }, 
        body: 
        {
            size: size
        }
    };
}