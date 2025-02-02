import { DateTime } from "luxon";
import { Header } from "../../shared/header";
import { HexColor } from "../../shared/hexColor";

export interface BackgroundColor
{   
    header: Header,
    body: {
        color: HexColor
    }
}

export function BuildBackgroundColor(color: string): BackgroundColor
{
    return {
        header: 
        {
            type: "backgroundColor", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
            color: 
            {
                hexString: color
            }
        }
    };
}