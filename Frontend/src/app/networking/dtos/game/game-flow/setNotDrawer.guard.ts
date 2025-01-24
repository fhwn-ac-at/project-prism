/*
 * Generated type guards for "setNotDrawer.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { SetNotDrawer } from "./setNotDrawer";

export function isSetNotDrawer(obj: unknown): obj is SetNotDrawer {
    const typedObj = obj as SetNotDrawer
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        typedObj.header.type === "setNotDrawer"
    )
}
