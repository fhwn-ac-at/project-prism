import { CurrentPlayersMessage } from "./CurrentPlayersMessage";
import { CurrentPlayersMessageVisitor } from "./Visitor";

export class SetScoreMessage implements CurrentPlayersMessage
{
    public constructor(public Playername: string, public OldScore: number, public NewScore: number)
    {}

    Accept(visitor: CurrentPlayersMessageVisitor): void 
    {
       visitor.Visit(this);
    }
}