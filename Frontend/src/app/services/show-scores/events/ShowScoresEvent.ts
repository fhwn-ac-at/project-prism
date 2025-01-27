import { PlayerData } from "../../player-data/PlayerData";

export class ShowScoresEvent
{
    public constructor(word: string, scores: PlayerData[])
    {
        this.Word = word;
        this.Scores = scores;
    }

    public Word: string;

    public Scores: PlayerData[];
}