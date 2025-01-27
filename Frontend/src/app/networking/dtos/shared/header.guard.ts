/*
 * Generated type guards for "header.ts".
 * WARNING: Do not manually change this file.
 */
import { DateTime } from "luxon";
import { Header } from "./header";

export function isHeader(obj: unknown): obj is Header {
    const typedObj = obj as Header

    const isHeaderType = (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        typeof typedObj["type"] === "string" &&
        typeof typedObj["timestamp"] === "string" 
    );

    if (isHeaderType) {
        const timestamp = DateTime.fromISO(typedObj.timestamp);
        return timestamp.isValid && timestamp.diffNow().toMillis() >= -5000;
    }

    return false;
}
