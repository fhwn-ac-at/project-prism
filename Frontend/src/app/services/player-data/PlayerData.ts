import { PlayerType } from "./PlayerType";

export interface PlayerData
{
  Username: string,
  Score: number,
  Id: string,
}

export function IsPlayerData(obj: any): obj is PlayerData 
{
  return typeof obj === 'object' &&
         typeof obj.Username === "string" &&
         typeof obj.Score === "number" &&
         typeof obj.Id === "string";
}

export function IsEqual(a: PlayerData, b: PlayerData): boolean
{
  return a.Id == b.Id;
}