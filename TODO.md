# TODO

## Current sprint
- dialog scelta nome savefile
- cambiare nomi classi


## Bugfixing
- loadgame from start screen


## Improvements
- aggiungere placeholder per character creation scene?
- controllare se/quando si possono svuotare i campi aux per la de/serializzazione
- pensare logica nomi univoci per Entities

- non elaborare input se ci sono pending updates
+ irrobustire il layout delle ui presenti rispetto al cambio di risoluzione/aspect ratio (anchor points, etc.)
+ key handler per inventory view (focus su item)
- grafica base per skills view
- verificare funzionamento scheduling con altri Actor presenti oltre Player


## Minutiae
- introdurre livello di log "warning"
- padding in popup text inventory item
+ Use some texture/shader for fog
- uniformare messaggi azioni (player vs other actor); grammar?

## Errands
- creare progetto github
- commit trigger per organize usings/code formatting
- scegliere e documentare key bindings
- provare onGUI
- ui design con figma
? scouting musica

## Brainstorming
- Familiarity feedback
- Detail info panels



## Milestones


### 0.5
- savegame


### 0.6
- procedural generation books

### 0.7
- butterflies
- some ui features


### 0.x
- skill system
- full-fledged familiarity
- secondary ui
	- stats
	- knowledge base



## Prototyping
### Procedural Item Generation
- solo copertine libri
- dimensioni
- proporzioni
- (sfumatura di) colore
- motivo cornice
- motivo centrale
- texture (effetto di superficie)
- accessori
	- segnalibro
	- lucchetto chiusura
	- ...
- frequenza per ogni valore di ogni parametro

+ icona libro: orientamento/prospettiva
+ alcuni valori disponibili solo per certe culture


