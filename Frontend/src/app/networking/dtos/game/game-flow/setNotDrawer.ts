import { DateTime } from "luxon";
import { Header } from "../../shared/header";

export interface SetNotDrawer
{
    header: Header,
    body: 
    {
    }
}

export function BuildSetNotDrawer(): SetNotDrawer
{
    return {
        header: 
        {
            type: "setNotDrawer", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
        }
    };
}