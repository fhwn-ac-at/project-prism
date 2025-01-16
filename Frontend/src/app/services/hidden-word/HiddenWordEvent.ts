import {Maybe} from "@sweet-monads/maybe";

export class HiddenWordEvent
{
    public constructor(lettersOrNones: Maybe<string>[])
    {
        this.LettersOrNones = lettersOrNones;
    }

    public LettersOrNones: Maybe<string>[];
}