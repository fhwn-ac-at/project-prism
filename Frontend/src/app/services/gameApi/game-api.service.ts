import { inject, Injectable } from '@angular/core';
import { SignalRService } from '../../networking/services/signal-r/signal-r.service';
import { Observable, PartialObserver, Subject, Subscription } from 'rxjs';
import { GameStarted } from '../../networking/dtos/lobby/gameStarted';
import { BuildRoundAmountChanged, RoundAmountChanged } from '../../networking/dtos/lobby/roundAmountChanged';
import { BuildRoundDurationChanged, RoundDurationChanged } from '../../networking/dtos/lobby/roundDurationChanged';
import { BuildChatMessage, ChatMessage } from '../../networking/dtos/shared/chatMessage';
import { UserDisconnected } from '../../networking/dtos/shared/userDisconnected';
import { UserJoined } from '../../networking/dtos/shared/userJoined';
import { BackgroundColor, BuildBackgroundColor } from '../../networking/dtos/game/drawing/backgroundColor';
import { BuildClear, Clear } from '../../networking/dtos/game/drawing/clear';
import { BuildClosePath, ClosePath } from '../../networking/dtos/game/drawing/closePath';
import { BuildDrawingSizeChanged, DrawingSizeChanged } from '../../networking/dtos/game/drawing/drawingSizeChanged';
import { BuildLineTo, LineTo } from '../../networking/dtos/game/drawing/lineTo';
import { BuildMoveTo, MoveTo } from '../../networking/dtos/game/drawing/moveTo';
import { BuildUndo, Undo } from '../../networking/dtos/game/drawing/undo';
import { BuildPoint, Point } from '../../networking/dtos/game/drawing/point';
import { GameEnded } from '../../networking/dtos/game/game-flow/gameEnded';
import { NextRound } from '../../networking/dtos/game/game-flow/nextRound';
import { SearchedWord } from '../../networking/dtos/game/game-flow/searchedWord';
import { BuildSelectWord, SelectWord } from '../../networking/dtos/game/game-flow/selectWord';
import { SetDrawer } from '../../networking/dtos/game/game-flow/setDrawer';
import { SetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer';
import { UserScore } from '../../networking/dtos/game/game-flow/userScore';
import { PlayerDataService } from '../player-data/player-data.service';
import { Position2d } from '../../../lib/Position2d';

@Injectable({
  providedIn: null
})
export class GameApiService 
{
  private signalRService: SignalRService = inject(SignalRService);
  private playerDataService: PlayerDataService = inject(PlayerDataService);

  private lobbyEventSub: Subject<GameStarted | RoundAmountChanged | RoundDurationChanged> = new Subject();
  private chatMessageEventSub: Subject<ChatMessage> = new Subject();
  private userConnectionEventSub: Subject<UserDisconnected | UserJoined> = new Subject();
  private drawingEventSub: 
    Subject<BackgroundColor | Clear | ClosePath | DrawingSizeChanged | LineTo | MoveTo | Point | Undo> 
    = new Subject();
  private gameFlowEventSub: 
    Subject<GameEnded | NextRound | SearchedWord | SelectWord | SetDrawer | SetNotDrawer | UserScore>
     = new Subject();


  public Start(lobbyId: string): Promise<void>
  {
    this.signalRService.Initialize(lobbyId);

    return this.signalRService.Connect();
  }

  public Stop(): Promise<void>
  {
    return this.signalRService.Stop();
  }

  public ObserveLobbyEvent(): Observable<GameStarted | RoundAmountChanged | RoundDurationChanged>
  {
    return this.lobbyEventSub.asObservable();
  }

  public ObserveChatMessageEvent(): Observable<ChatMessage>
  {
    return this.chatMessageEventSub.asObservable();
  }

  public ObserveUserConnectionEvent(): Observable<UserDisconnected | UserJoined>
  {
    return this.userConnectionEventSub.asObservable();
  }

  public ObserveDrawingEvent(): 
    Observable<BackgroundColor | Clear | ClosePath | DrawingSizeChanged | LineTo | MoveTo | Point | Undo>
  {
    return this.drawingEventSub.asObservable();
  }

  public ObserveGameFlowEvent(): 
    Observable<GameEnded | NextRound | SearchedWord | SelectWord | SetDrawer | SetNotDrawer | UserScore>
  {
    return this.gameFlowEventSub.asObservable()
  }

  public SendRoundAmount(roundAmount: number): Promise<void>
  {
    const roundAmountChanged = BuildRoundAmountChanged(roundAmount);

    return this.signalRService.SendData(roundAmountChanged);
  }

  public SendRoundDuration(roundDuration: number): Promise<void>
  {
    const roundDurationChanged = BuildRoundDurationChanged(roundDuration);

    return this.signalRService.SendData(roundDurationChanged);
  }

  public SendChatMessage(message: string): Promise<void>
  {
    if (this.playerDataService.PlayerData.value.isNone()) return Promise.reject(new Error("No userdata present!"));

    const roundDurationChanged = BuildChatMessage(
      message, 
      {
        id: this.playerDataService.PlayerData.value.value.Id, 
        name: this.playerDataService.PlayerData.value.value.Username
      }
    );

    return this.signalRService.SendData(roundDurationChanged);
  }

  public SendBackgroundColor(color: string): Promise<void>
  {
    const backgroundColor = BuildBackgroundColor(color);

    return this.signalRService.SendData(backgroundColor);
  }

  public SendDrawingSize(size: number): Promise<void>
  {
    const drawingSize = BuildDrawingSizeChanged(size);

    return this.signalRService.SendData(drawingSize);
  }

  public SendUndo(): Promise<void>
  {
    return this.signalRService.SendData(BuildUndo());
  }

  public SendClear(): Promise<void>
  {
    return this.signalRService.SendData(BuildClear());
  }

  public SendClosePath(): Promise<void>
  {
    return this.signalRService.SendData(BuildClosePath());
  }

  public SendLineTo(pos: Position2d, color: string): Promise<void>
  {
    return this.signalRService.SendData(BuildLineTo(pos, color));
  }

  public SendMoveTo(pos: Position2d): Promise<void>
  {
    return this.signalRService.SendData(BuildMoveTo(pos));
  }

  public SendPoint(point: Position2d, radius: number, color: string): Promise<void>
  {
    return this.signalRService.SendData(BuildPoint(point, radius, color))
  }

  public SendSelectedWord(word: string): Promise<void>
  {
    return this.signalRService.SendData(BuildSelectWord(word));
  }
}
