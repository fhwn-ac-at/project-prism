import { Header } from "./header";
import { isHeader } from "./header.guard";

export function hasHeaderAndBody(obj: unknown) : obj is {header: Header, body: any}
{
    const typedObj: any = obj as any;

    return  (
                (
                    typedObj !== null &&
                    typeof typedObj === "object" ||
                    typeof typedObj === "function"
                ) &&
                isHeader(typedObj["header"]) as boolean &&
                (
                    typedObj["body"] !== null &&
                    typeof typedObj["body"] === "object" ||
                    typeof typedObj["body"] === "function"
                )
            )
}