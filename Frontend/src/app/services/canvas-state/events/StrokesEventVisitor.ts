import { ClearedEvent } from "./ClearedEvent";
import { ClosedEvent } from "./ClosedEvent";
import { MovedEvent } from "./MovedEvent";
import { RemovedEvent } from "./RemovedEvent";
import { StartedEvent } from "./StartedEvent";

export interface StrokesEventVisitor
{
    Visit(e: ClearedEvent): void;
    Visit(e: ClosedEvent): void;
    Visit(e: MovedEvent): void;
    Visit(e: RemovedEvent): void;
    Visit(e: StartedEvent): void;
}