export class WordPart
{
    public constructor(char: string, isRevealed: boolean)
    {
        if(char.length != 1)
        {
            throw Error("Must be of length 1!");
        }

        this.Char = char;
        this.IsRevealed = isRevealed;
    }

    public Char: string;
    
    public IsRevealed: boolean;
}