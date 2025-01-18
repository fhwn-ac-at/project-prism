import { PlayerType } from "./PlayerType";

export interface PlayerData
{
  Username: string,
  Role: PlayerType,
  Score: number,
}

export function IsPlayerData(obj: any): obj is PlayerData 
{
  return typeof obj === 'object' &&
         typeof obj.Username === "string" &&
         typeof obj.Role === "number" &&
         typeof obj.Score === "number"
}

export function IsEqual(a: PlayerData, b: PlayerData): boolean
{
  return a.Role == b.Role && a.Username == b.Username;
}