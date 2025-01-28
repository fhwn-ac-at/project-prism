import { WordPart } from "./WordPart";


export class HiddenWordEvent
{
    public constructor(lettersOrNones: WordPart[])
    {
        this.LettersOrNones = lettersOrNones;
    }

    public LettersOrNones: WordPart[];
}