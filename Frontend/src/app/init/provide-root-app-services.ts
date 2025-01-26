import { SignalRService } from "../networking/services/signal-r/signal-r.service";
import { CanDrawService } from "../services/can-draw/can-draw.service";
import { ChatMessagesService } from "../services/chat-messages/chat-messages.service";
import { ConfigService } from "../services/config/config.service";
import { CountdownService } from "../services/countdown/countdown.service";
import { ActivePlayersService } from "../services/current-players/active-players.service";
import { GameRoundService } from "../services/game-round/game-round.service";
import { GameApiService } from "../networking/services/game-api/game-api.service";
import { HiddenWordService } from "../services/hidden-word/hidden-word.service";
import { LobbyApiService } from "../networking/services/lobby-api/lobby-api.service";
import { PickWordService } from "../services/pick-word/pick-word.service";
import { PlayerDataService } from "../services/player-data/player-data.service";
import { StartService } from "../services/start/start.service";
import { GameIdService } from "../services/gameId/game-id.service";
import { PlayerTypeService } from "../services/player-type/player-type.service";
import { inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LobbyUserTypeService } from "../services/lobby-user-type/lobby-user-type.service";

export const provideRootAppServices = () => 
[
  {provide: ConfigService, useClass: ConfigService},
  {provide: PlayerDataService, useClass: PlayerDataService},
  {provide: CountdownService, useClass: CountdownService},
  {provide: HiddenWordService, useClass: HiddenWordService},
  {provide: ActivePlayersService, useClass: ActivePlayersService},
  {provide: ChatMessagesService, useClass: ChatMessagesService},
  {provide: CanDrawService, useClass: CanDrawService},
  {provide: GameRoundService, useClass: GameRoundService},
  {provide: PickWordService, useClass: PickWordService},
  {provide: SignalRService, useClass: SignalRService},
  {provide: LobbyApiService, useClass: LobbyApiService},
  {provide: GameApiService, useClass: GameApiService},
  {provide: StartService, useClass: StartService},
  {provide: GameIdService, useClass: GameIdService},
  {provide: PlayerTypeService, useClass: PlayerTypeService},
  {provide: LobbyUserTypeService, useClass: LobbyUserTypeService}
];