import { PlayerData } from "../../player-data/PlayerData";

export class ShowScoresEvent
{
    public constructor(word: string, scores: PlayerData[], isEnded: boolean)
    {
        this.Word = word;
        this.Scores = scores;
        this.IsEnded = isEnded;
    }

    public Word: string;
    public Scores: PlayerData[];
    public IsEnded: boolean;
}