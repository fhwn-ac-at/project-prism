import { just, Maybe, none } from "@sweet-monads/maybe";
import { WordPart } from "./WordPart";

export class StringToWordPartsConverter
{
    public static ConvertToWordPart(word: string): WordPart[] 
    {
        if (word.length < 1) throw Error("Must be at least of length 1!");

        let wp: WordPart[] = [];
  
        for(let i = 0; i < word.length; i++)
        {
          wp.push(new WordPart(word[i] != '_' ? word[i] : undefined));
        }
  
        return wp;
    }
}