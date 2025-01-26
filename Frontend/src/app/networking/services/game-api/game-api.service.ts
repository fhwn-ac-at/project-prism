import { inject, Injectable } from '@angular/core';
import { SignalRService } from '../signal-r/signal-r.service';
import { Observable, Subject } from 'rxjs';
import { GameStarted } from '../../dtos/lobby/gameStarted';
import { BuildRoundAmountChanged, RoundAmountChanged } from '../../dtos/lobby/roundAmountChanged';
import { BuildRoundDurationChanged, RoundDurationChanged } from '../../dtos/lobby/roundDurationChanged';
import { BuildChatMessage, ChatMessage } from '../../dtos/shared/chatMessage';
import { UserDisconnected } from '../../dtos/shared/userDisconnected';
import { UserJoined } from '../../dtos/shared/userJoined';
import { BackgroundColor, BuildBackgroundColor } from '../../dtos/game/drawing/backgroundColor';
import { BuildClear, Clear } from '../../dtos/game/drawing/clear';
import { BuildClosePath, ClosePath } from '../../dtos/game/drawing/closePath';
import { BuildDrawingSizeChanged, DrawingSizeChanged } from '../../dtos/game/drawing/drawingSizeChanged';
import { BuildLineTo, LineTo } from '../../dtos/game/drawing/lineTo';
import { BuildMoveTo, MoveTo } from '../../dtos/game/drawing/moveTo';
import { BuildUndo, Undo } from '../../dtos/game/drawing/undo';
import { BuildPoint, Point } from '../../dtos/game/drawing/point';
import { GameEnded } from '../../dtos/game/game-flow/gameEnded';
import { NextRound } from '../../dtos/game/game-flow/nextRound';
import { SearchedWord } from '../../dtos/game/game-flow/searchedWord';
import { BuildSelectWord, SelectWord } from '../../dtos/game/game-flow/selectWord';
import { SetDrawer } from '../../dtos/game/game-flow/setDrawer';
import { SetNotDrawer } from '../../dtos/game/game-flow/setNotDrawer';
import { UserScore } from '../../dtos/game/game-flow/userScore';
import { PlayerDataService } from '../../../services/player-data/player-data.service';
import { Position2d } from '../../../../lib/Position2d';
import { isGameStarted } from '../../dtos/lobby/gameStarted.guard';
import { isRoundAmountChanged } from '../../dtos/lobby/roundAmountChanged.guard';
import { isRoundDurationChanged } from '../../dtos/lobby/roundDurationChanged.guard';
import { Connected } from '../signal-r/events/Connected';
import { Closed } from '../signal-r/events/Closed';
import { Reconnecting } from '../signal-r/events/Reconnecting';
import { isBackgroundColor } from '../../dtos/game/drawing/backgroundColor.guard';
import { hasHeaderAndBody } from '../../dtos/shared/hasHeaderAndBody.guard';
import { isClear } from '../../dtos/game/drawing/clear.guard';
import { isClosePath } from '../../dtos/game/drawing/closePath.guard';
import { isDrawingSizeChanged } from '../../dtos/game/drawing/drawingSizeChanged.guard';
import { isLineTo } from '../../dtos/game/drawing/lineTo.guard';
import { isMoveTo } from '../../dtos/game/drawing/moveTo.guard';
import { isPoint } from '../../dtos/game/drawing/point.guard';
import { isUndo } from '../../dtos/game/drawing/undo.guard';
import { isGameEnded } from '../../dtos/game/game-flow/gameEnded.guard';
import { isNextRound } from '../../dtos/game/game-flow/nextRound.guard';
import { isSearchedWord } from '../../dtos/game/game-flow/searchedWord.guard';
import { isSelectWord } from '../../dtos/game/game-flow/selectWord.guard';
import { isSetDrawer } from '../../dtos/game/game-flow/setDrawer.guard';
import { isSetNotDrawer } from '../../dtos/game/game-flow/setNotDrawer.guard';
import { isUserScore } from '../../dtos/game/game-flow/userScore.guard';
import { isChatMessage } from '../../dtos/shared/chatMessage.guard';
import { isUserDisconnected } from '../../dtos/shared/userDisconnected.guard';
import { isUserJoined } from '../../dtos/shared/userJoined.guard';

@Injectable({
  providedIn: null
})
export class GameApiService 
{
  private signalRService: SignalRService = inject(SignalRService);
  private playerDataService: PlayerDataService = inject(PlayerDataService);

  // event subs
  private lobbyEventSub: Subject<GameStarted | RoundAmountChanged | RoundDurationChanged> = new Subject();
  private chatMessageEventSub: Subject<ChatMessage> = new Subject();
  private userConnectionEventSub: Subject<UserDisconnected | UserJoined> = new Subject();
  private drawingEventSub: 
    Subject<BackgroundColor | Clear | ClosePath | DrawingSizeChanged | LineTo | MoveTo | Point | Undo> 
    = new Subject();
  private gameFlowEventSub: 
    Subject<GameEnded | NextRound | SearchedWord | SelectWord | SetDrawer | SetNotDrawer | UserScore>
     = new Subject();
  private connectionEventSub: Subject<Closed | Connected | Reconnecting> = new Subject();

  private headerTypeToDecoderFunctionsMap: Map<string, (val: unknown) => void> = new Map
  (
    [
      // drawing
      ["bakgroundColor", (val) => {
         if (!isBackgroundColor(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      ["clear", (val) => {
        if (!isClear(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      ["closePath", (val) => {
        if (!isClosePath(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      ["drawingSizeChanged", (val) => {
        if (!isDrawingSizeChanged(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      ["lineTo", (val) => {
        if (!isLineTo(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      ["moveTo", (val) => {
        if (!isMoveTo(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
     }],
      ["point", (val) => {
        if (!isPoint(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      ["undo", (val) => {
        if (!isUndo(val)){this.HandleInvalidData(val); return;}  this.drawingEventSub.next(val); 
      }],
      // game flow
      ["gameEnded", (val) => {
        if (!isGameEnded(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      ["nextRound", (val) => {
        if (!isNextRound(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      ["searchedWord", (val) => {
        if (!isSearchedWord(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      ["selectWord", (val) => {
        if (!isSelectWord(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      ["setDrawer", (val) => {
        if (!isSetDrawer(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      ["setNotDrawer", (val) => {
        if (!isSetNotDrawer(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      ["userScore", (val) => {
        if (!isUserScore(val)){this.HandleInvalidData(val); return;}  this.gameFlowEventSub.next(val); 
      }],
      // lobby
      ["gameStarted", (val) => {
        if (!isGameStarted(val)){this.HandleInvalidData(val); return;}  this.lobbyEventSub.next(val); 
      }],
      ["roundAmountChanged", (val) => {
        if (!isRoundAmountChanged(val)){this.HandleInvalidData(val); return;}  this.lobbyEventSub.next(val); 
      }],
      ["roundDurationChanged", (val) => {
        if (!isRoundDurationChanged(val)){this.HandleInvalidData(val); return;}  this.lobbyEventSub.next(val); 
      }],
      // chat
      ["chatMessage", (val) => {
        if (!isChatMessage(val)){this.HandleInvalidData(val); return;}  this.chatMessageEventSub.next(val); 
      }],
      // user connection
      ["userDisconnected", (val) => {
        if (!isUserDisconnected(val)){this.HandleInvalidData(val); return;}  this.userConnectionEventSub.next(val); 
      }],
      ["userJoined", (val) => {
        if (!isUserJoined(val)){this.HandleInvalidData(val); return;}  this.userConnectionEventSub.next(val); 
      }],
    ]
  );

  public constructor()
  {
    console.log(this.headerTypeToDecoderFunctionsMap);
    this.signalRService.DataReceivedEvent.subscribe(this.OnDataReceived);
    this.signalRService.ConnectionObservable.subscribe(this.OnConnectionEvent);
    console.log(this.headerTypeToDecoderFunctionsMap);
  }

  public Start(userId: string): Promise<void>
  {
    this.signalRService.Initialize(userId);

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

  public async SendRoundAmount(roundAmount: number): Promise<void>
  {
    const roundAmountChanged = BuildRoundAmountChanged(roundAmount);

    return this.signalRService.SendData(roundAmountChanged);
  }

  public async SendRoundDuration(roundDuration: number): Promise<void>
  {
    const roundDurationChanged = BuildRoundDurationChanged(roundDuration);

    return this.signalRService.SendData(roundDurationChanged);
  }

  public async SendChatMessage(message: string): Promise<void>
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

  public async SendBackgroundColor(color: string): Promise<void>
  {
    const backgroundColor = BuildBackgroundColor(color);

    return this.signalRService.SendData(backgroundColor);
  }

  public async SendDrawingSize(size: number): Promise<void>
  {
    const drawingSize = BuildDrawingSizeChanged(size);

    return this.signalRService.SendData(drawingSize);
  }

  public async SendUndo(): Promise<void>
  {
    return this.signalRService.SendData(BuildUndo());
  }

  public async SendClear(): Promise<void>
  {
    return this.signalRService.SendData(BuildClear());
  }

  public async SendClosePath(): Promise<void>
  {
    return this.signalRService.SendData(BuildClosePath());
  }

  public async SendLineTo(pos: Position2d, color: string): Promise<void>
  {
    return this.signalRService.SendData(BuildLineTo(pos, color));
  }

  public async SendMoveTo(pos: Position2d): Promise<void>
  {
    return this.signalRService.SendData(BuildMoveTo(pos));
  }

  public async SendPoint(point: Position2d, radius: number, color: string): Promise<void>
  {
    return this.signalRService.SendData(BuildPoint(point, radius, color))
  }

  public async SendSelectedWord(word: string): Promise<void>
  {
    return this.signalRService.SendData(BuildSelectWord(word));
  }

  // handlers
  private OnDataReceived(data: unknown) 
  {
    if (!hasHeaderAndBody(data))
    {
      console.log("Received data without header and body: " + JSON.stringify(data));
      return;
    }

    console.log(data.header.type);
    console.log(this);
    const decoderFunction: ((data: unknown) => void) | undefined = this.headerTypeToDecoderFunctionsMap.get(data.header.type);
    console.log(decoderFunction);

    if (decoderFunction == undefined)
    {
      console.log("received data without known header type: " + data.header.type);
      return;
    }

    decoderFunction(data);
  }

  private OnConnectionEvent(onConnectionEvent: Connected | Closed | Reconnecting) 
  {
    this.connectionEventSub.next(onConnectionEvent);
  }

  private HandleInvalidData(data: unknown)
  {
    console.log("Received invalid data with known header: " + data);
  }
}
