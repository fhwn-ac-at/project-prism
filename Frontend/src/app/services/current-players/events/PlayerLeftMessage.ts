import { PlayerData } from "../../player-data/PlayerData";
import { CurrentPlayersMessage } from "./CurrentPlayersMessage";

export class PlayerRemovedMessage implements CurrentPlayersMessage
{
    public constructor(public Player: PlayerData)
    {  
    }
}