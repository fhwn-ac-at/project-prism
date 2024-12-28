import { EventVisitor } from "./EventVisitor";

export interface Event
{
    Accept(visitor: EventVisitor): void;
}