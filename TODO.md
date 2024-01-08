# TODO

## Current sprint
- add copyright header to all sources

- tidy up gameMap/gameState API

- tidy up uso entity.Name
- fighter component
- monster factory architecture
- ripristinare colore custom per monster



## Bugfixing


## Improvements
- cambiare icona libro; differenziare mapIcon/inventoryIcon
- trasformare graphicsConfig in ScriptableObject
- irrobustire il layout delle ui presenti rispetto al cambio di risoluzione/aspect ratio (anchor points, etc.)
- sperimentare uso colliders vs resetVisibleEntities

- dialog scelta nome savefile
- riattivare e migliorare PathfindingRequest 
- generare cognomi degli autori dei libri
? spostare visibility status property da map tiles

+ refactoring Entity-Component-System


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
+ animazione e messa in sequenza movimento mostri


### 0.x
- skill system
- full-fledged familiarity
- secondary ui
	- stats
	- knowledge base

### 0.xx
- shaders (https://www.ronja-tutorials.com/)
- procedural generation
	- books
	- butterflies


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
- provare icone pozioni con solo liquido colorato

- scelta font
	https://www.1001fonts.com/vinque-font.html
	https://fontsgeek.com/lombardic-narrow-font
	https://www.1001fonts.com/flanker-griffo-font.html
	https://www.1001fonts.com/camelotcaps-font.html
