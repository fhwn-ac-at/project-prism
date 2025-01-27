import { Header } from "./header";
import { isHeader } from "./header.guard";

export function hasHeader(obj: unknown) : obj is {header: Header}
{
    const typedObj: any = obj as any;

    if (!(
            typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function"
        ))
    {
        return false;
    }

    if(!isHeader(typedObj["header"]))
    {
        return false;
    }

    return true;
}