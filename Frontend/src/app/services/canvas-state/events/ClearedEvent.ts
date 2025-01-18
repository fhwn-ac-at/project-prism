import { StrokesEvent } from "./StrokesEvent";
import { StrokesEventVisitor } from "./StrokesEventVisitor";

export class ClearedEvent implements StrokesEvent
{
    public Accept(visitor: StrokesEventVisitor): void 
    {
        visitor.Visit(this);
    }

}