/*
 * Generated type guards for "setDrawer.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { SetDrawer } from "./setDrawer";

export function isSetDrawer(obj: unknown): obj is SetDrawer {
    const typedObj = obj as SetDrawer
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        Array.isArray(typedObj["body"]["words"]) &&
        typedObj["body"]["words"].every((e: any) =>
            (e !== null &&
                typeof e === "object" ||
                typeof e === "function") &&
            typeof e["word"] === "string" &&
            typeof e["difficulty"] === "number"
        ) &&
        typedObj.header.type === "setDrawer"
    )
}
