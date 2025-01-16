import { EventVisitor } from "./EventVisitor";
import {Event} from "./Event"

export class PoppedEvent<T> implements Event
{
    public constructor(item: T | undefined)
    {
        this.PoppedItem = item;
    }

    Accept(visitor: EventVisitor): void 
    {
       visitor.Visit(this);
    }

    public PoppedItem: T | undefined;
}