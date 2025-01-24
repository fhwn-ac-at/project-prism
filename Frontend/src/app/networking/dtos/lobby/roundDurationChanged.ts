import { Header } from "../shared/header";

export interface RoundDurationChanged
{
    header: Header,
    body:
    {
        duration: number,
    }
}

export function BuildRoundDurationChanged(duration: number): RoundDurationChanged
{
    return {
        header: 
        {
            type: "roundDurationChanged", 
            timestamp: Date.now()
        }, 
        body: 
        {
            duration: duration
        }
    };
}