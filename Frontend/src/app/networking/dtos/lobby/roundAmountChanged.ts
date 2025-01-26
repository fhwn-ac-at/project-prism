import { DateTime } from "luxon";
import { Header } from "../shared/header";

export interface RoundAmountChanged
{
    header: Header,
    body:
    {
        rounds: number,
    }
}

export function BuildRoundAmountChanged(rounds: number): RoundAmountChanged
{
    return {
        header: 
        {
            type: "roundAmountChanged", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
            rounds: rounds
        }
    };
}