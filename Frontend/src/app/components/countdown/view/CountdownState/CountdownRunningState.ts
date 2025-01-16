import { CountdownState } from "./CountdownState";

export class CountdownRunningState implements CountdownState
{
    public constructor(public TimeLeft: number)
    {
    }
}