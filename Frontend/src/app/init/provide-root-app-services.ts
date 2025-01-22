import { ApiService } from "../networking/services/api/api.service";
import { SignalRService } from "../networking/services/signal-r/signal-r.service";
import { CanDrawService } from "../services/can-draw/can-draw.service";
import { ChatMessagesService } from "../services/chat-messages/chat-messages.service";
import { ConfigService } from "../services/config/config.service";
import { CountdownService } from "../services/countdown/countdown.service";
import { ActivePlayersService } from "../services/current-players/active-players.service";
import { GameRoundService } from "../services/game-round/game-round.service";
import { HiddenWordService } from "../services/hidden-word/hidden-word.service";
import { PickWordService } from "../services/pick-word/pick-word.service";
import { PlayerDataService } from "../services/player-data/player-data.service";

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
  {provide: ApiService, useClass: ApiService},
  {provide: SignalRService, useClass: SignalRService},
];