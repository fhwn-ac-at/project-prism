export class WordsToPickEvent
{
    public constructor(words: {word: string, difficulty: number}[])
    {
        if(words.length < 1) throw new Error("Must have at least one word!");

        this.Words = words;
    }

    public Words: {word: string, difficulty: number}[];
}