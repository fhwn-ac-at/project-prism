import {DateTime} from "luxon";

export interface Header
{
    type: string,
    timestamp: DateTime
}