import { PlayerRemovedMessage } from "./PlayerRemovedMessage";
import { SetScoreMessage } from "./SetScoreMessage";

export interface CurrentPlayersMessageVisitor
{
    Visit(msg: SetScoreMessage): void;
    Visit(msg: PlayerRemovedMessage): void;
    Visit(msg: SetScoreMessage): void;
}