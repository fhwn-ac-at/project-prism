import { PlayerData } from "../../player-data/PlayerData";
import { CurrentPlayersMessage } from "./CurrentPlayersMessage";

export class PlayerAddedMessage implements CurrentPlayersMessage
{
    public constructor(public Player: PlayerData)
    {  
    }
}