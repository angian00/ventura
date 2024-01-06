# TODO

## Current sprint
- testare actor scheduling
- monster factory architecture

- pathfinding on tile click
	- feedback click
	- scheduling turni movimento
	- animazione e sleep turni movimento
- arricchire stats player


- theme previewer
	- impacchettare font resources
	- bitmap di sfondo


## Bugfixing
- aggiungere a GameItem.MoveTo la notifica EventManager.GameStateUpdateEvent.Invoke(...);
- hide site if there is one behind an item in map


## Improvements
-  tidy up gameMap/gameState API
- spostare PathfindingRequest da UIRequestData
- irrobustire il layout delle ui presenti rispetto al cambio di risoluzione/aspect ratio (anchor points, etc.)
- dialog scelta nome savefile
- aggiungere bitfield updatedFields agli EventData

+ refactoring Entity-Component-System

- campo id per Entities (Guid.NewGuid())

+ ottimizzare la mole di eventi circolanti


## Minutiae
+ key handler per inventory view (focus su item)
- grafica base per skills view
- padding in popup text inventory item
+ Use some texture/shader for fog e terrain


## Errands
- informarsi ScriptableObjects
- raccogliere file utili sapientia*
- raccogliere txt Poliziano
- controllare licenza https://game-icons.net/

- provare onGUI
? scouting musica (https://www.freesoundtrackmusic.com/)
? suoni ambientali

## Brainstorming
- Familiarity feedback
- Detail info panels



## Milestones

### 0.7
- shaders (https://www.ronja-tutorials.com/)
- procedural generation
	- books
	- butterflies

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

### Biomes
https://github.com/GrandPiaf/Biome-and-Vegetation-PCG 

### Visual Design
- https://www.gameuidatabase.com/
- Screenshot Strange Horticulture
- https://www.color-hex.com/color-palettes/?keyword=Book
- creare sfondo watercolor https://creativemarket.com/ArtistMef/2928714-Minimalist-Watercolor-Backgrounds
- creare palette da https://www.mountangelabbey.org/library/illuminated-manuscripts/
- scelta font
	https://www.1001fonts.com/vinque-font.html
	https://fontsgeek.com/lombardic-narrow-font
	https://www.1001fonts.com/flanker-griffo-font.html
- capire licensing fonts
	https://www.reddit.com/r/gamedev/s/NB4PcOYfY3