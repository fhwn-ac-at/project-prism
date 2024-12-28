import { EventVisitor } from "./EventVisitor";
import {Event} from "./Event"

export class PushedEvent<T> implements Event
{
    public constructor(item: T)
    {
        this.PushedItem = item;
    }

    public PushedItem: T;

    Accept(visitor: EventVisitor): void 
    {
       visitor.Visit(this);
    }
}