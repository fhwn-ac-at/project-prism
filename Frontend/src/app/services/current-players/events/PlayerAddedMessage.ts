import { PlayerData } from "../../player-data/PlayerData";
import { CurrentPlayersMessage } from "./CurrentPlayersMessage";
import { CurrentPlayersMessageVisitor } from "./Visitor";

export class PlayerAddedMessage implements CurrentPlayersMessage
{
    public constructor(public Player: PlayerData)
    {  
    }
    
    Accept(visitor: CurrentPlayersMessageVisitor): void 
    {
       visitor.Visit(this);
    }
}