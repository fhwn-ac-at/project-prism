export class WordPart
{
    public constructor(char: string | undefined)
    {
        if(char == undefined)
        {
            this.CharOrNone = char;
        }
        else if(char.length == 1)
        {
            this.CharOrNone = char;
        }
        else
        {
            throw Error("Must be of length 1!");
        } 
    }
    
    public CharOrNone: string | undefined;
}