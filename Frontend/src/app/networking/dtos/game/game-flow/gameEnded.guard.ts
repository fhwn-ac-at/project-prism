/*
 * Generated type guards for "gameEnded.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { GameEnded } from "./gameEnded";

export function isGameEnded(obj: unknown): obj is GameEnded 
{
    const typedObj = obj as GameEnded

    if (!
            (   
                typedObj !== null &&
                typeof typedObj === "object" ||
                typeof typedObj === "function"
            )
        )
    {
        return false;
    }

    if (!isHeader(typedObj["header"]) as boolean)
    {
        return false;
    } 

    if 
    (!
        (
            typeof typedObj.body === "object" ||
            typeof typedObj.body === "function"
        )
    )
    {
        return false;
    }

    if
    (! 
        (
            typeof typedObj.body.score === "object" &&
            typeof typedObj.body.word === "string"
        )
    )
    {
        return false;
    }

    if (typedObj.header.type != "gameEnded")
    {
        return false;
    }

    return true;
}
