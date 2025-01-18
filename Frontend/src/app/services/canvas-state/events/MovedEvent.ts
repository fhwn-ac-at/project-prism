import { StrokeVM } from "../StrokeVM";
import { StrokesEventVisitor } from "./StrokesEventVisitor";

export class MovedEvent
{
    public constructor(public stroke: StrokeVM)
    {}

    public Accept(visitor: StrokesEventVisitor): void 
    {
        visitor.Visit(this);
    }
}