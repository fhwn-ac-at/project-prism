import { Event } from "./Event";
import { EventVisitor } from "./EventVisitor";

export class SetEvent<T> implements Event
{
    public constructor(newItems: T[])
    {
        this.NewItems = newItems;
    }

    public NewItems: T[];

    public Accept(visitor: EventVisitor): void 
    {
        visitor.Visit(this);
    }
}