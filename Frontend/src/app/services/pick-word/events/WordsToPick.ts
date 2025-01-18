export class WordsToPickEvent
{
    public constructor(words: string[])
    {
        if(words.length < 1) throw new Error("Must have at least one word!");

        this.Words = words;
    }

    public Words: string[];
}