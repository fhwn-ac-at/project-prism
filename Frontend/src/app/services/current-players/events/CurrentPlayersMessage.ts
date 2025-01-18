import { CurrentPlayersMessageVisitor } from "./Visitor";

export interface CurrentPlayersMessage
{
    Accept(visitor: CurrentPlayersMessageVisitor): void;
}