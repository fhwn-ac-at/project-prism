# Lobby:

## roundAmountChanged: 
    von client zu Backend von Backend zu den anderen clients
    einstellngs änderung, wie oft alle personen zeichnen müssen

## roundDurationChanged:
    von client zu Backend von Backend zu den anderen clients
    einstellungs änderung, wie lange das zeichnen einer person dauert
    duration in sekunden

## chatMessage:
    von client zu Backend von Backend zu den anderen clients
    reine text nachricht

## userJoined:
    nur von Backend zu Frontend sagt aus das ein neuer Client gejoined ist

## userDisconnected;
    nur von Backend zu Frontend sagt aus das ein Client disconnected ist


von lobby zu game http get request start game von einem Client
## gameStarted 
    bekommen alle clients sagt aus das auf den game screen gewechselt wird


# Game

## Drawing messages
    Messages welch nur fürs zeichnen verwendet werden und keine änderung am state haben

### backgroundColor

### clear

### closePath

### drawingSizeChanged

### lineTo

### moveTo

### point

### undo
    löscht komplete lezte lienen oder vieleicht anders wenn wir parts übertragen

## Game flow messages

### setDrawer
    erste message nur vom Backend an Frontend sagt dieser User muss jetzt zeichnen
    Übergibt auch ein array an wörtern zum auswählen welches er zeichnen will werden bei uns 3 sein
    Von da an startet ein 60sec timer um das wort auszuwählen, wird keines gwählt sucht das backend ein wort aus und versendet eine searchedWord message an alle

### setNotDrawer
    zeichnet nicht wartet auf searchedWord message


### selectWord
    wird bei auswahl eines Wortes des zeichners gesendet darauf hin bekommen alle die searchedWord message

### searchedWord
    Das wort welches gezeichnet werden soll
    Frontend zeigt die anzahl der Buchstaben als _ an also wort = _ _ _ _
    Mit dieser Message startet das Zeichnen und raten 
    Am besten anzeigen wie lange noch zeit ist zum zeichnen

### chatMessage:
    von client zu Backend von Backend zu den anderen clients
    reine text nachricht
    wird zum raten verwendet, heißt wenn richtig wird sie nicht an andere versendet
    wenn ein spieler das Wort eraten hat werden seine nachrichten nur spielern angezeigt welche es auch eraten haben 
        (alle nachrichten von usern die schon einen score für diese zeichnung haben werden in einer andern farbe angezeigt)
    Frontend:
    wenn das wort bis auf einen buchstaben bei wörten bis inklusive 6 buchstaben gleich ist sollte angezeigt werden wort fast erraten
    bei wörten ab 7 buchstaben schon wenn es bis auf zwei buchstaben richtig ist

### userScoreMessage:
    vom backen nur an alle client
    Sagt aus das ein User das wort eraten hat.
    Dieser user becommt auch einen score für das eraten
    wenn die nachricht erhalten wird wird das gesuchte wort in klartext angezeigt stat _ _ _ ...
    zeichner bekommt die nachricht ganz am ende wenn zeit abgelaufen oder alle eraten haben hat keine auswirkung auf ihn

    nach dem der zeichner den score erhalten hat bekommt das fronten als nächstes eine nextRoundMessage oder gameEnded message

### nextRoundMessage (vielleicht besserer name benötigt)
    nur vom backend an alle clients
    hat im body die aktuelle runde
    es geht danach mit setDrawer/setNotDrawer weiter also die nächste zeichnung
    optional:
        anzeigen wie hoch der Score von jedem user für diese runde war


## messages außerhalb des loops welche den state ändern

### gameEndedMessage
    von backend an alle clients 
    backend geht zurück in den lobby mode
    anzeigen der scores des spieles mit reihnefolge wer gewonnen hat und nach ca 30 sekunden wieder in die Lobby wechseln