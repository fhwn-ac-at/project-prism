import { EventVisitor } from "./EventVisitor";
import { Event } from "./Event";

export class ClearEvent implements Event
{
    Accept(visitor: EventVisitor): void 
    {
        visitor.Visit(this);
    }
}