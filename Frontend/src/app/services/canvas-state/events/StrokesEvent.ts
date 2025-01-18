import { StrokesEventVisitor } from "./StrokesEventVisitor";

export interface StrokesEvent
{
    Accept(visitor: StrokesEventVisitor): void;
}