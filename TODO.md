# TODO

## Current sprint
- informarsi esempi Entity-Component
- arricchire stats player
- pathfinding on tile click
- generazione actors in mappa (butterflies)

- theme previewer

## Bugfixing


## Improvements
- controllare se/quando si possono svuotare i campi aux per la de/serializzazione
- pensare logica nomi univoci per Entities
- irrobustire il layout delle ui presenti rispetto al cambio di risoluzione/aspect ratio (anchor points, etc.)
- dialog scelta nome savefile
- aggiungere bitfield updatedFields agli EventData

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
- Screenshot gioco erboristeria
- https://www.color-hex.com/color-palettes/?keyword=Book
