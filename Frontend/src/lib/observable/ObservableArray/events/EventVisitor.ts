import {PushedEvent} from "./PushedEvent";
import {PoppedEvent} from "./PoppedEvent";
import { ClearEvent } from "./ClearEvent";
import { SetEvent } from "./SetEvent";

export interface EventVisitor
{
    Visit<T>(event: PushedEvent<T>): void

    Visit<T>(event: PoppedEvent<T>): void

    Visit<T>(event: SetEvent<T>): void

    Visit(event: ClearEvent) : void
}

