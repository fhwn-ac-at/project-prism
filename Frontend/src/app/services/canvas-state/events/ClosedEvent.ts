import { StrokeVM } from "../StrokeVM";
import { StrokesEventVisitor } from "./StrokesEventVisitor";

export class ClosedEvent
{
    public constructor(public stroke: StrokeVM)
    {}

    public Accept(visitor: StrokesEventVisitor): void 
    {
        visitor.Visit(this);
    }
}