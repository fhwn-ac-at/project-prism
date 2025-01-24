export interface RelativePoint
{
    x: number,
    y: number,
}

export function IsRelativePoint(obj: any)
{
    return  typeof obj.x == "number" &&
            typeof obj.y === "number"
}

