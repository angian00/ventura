# TODO

## Current sprint
- completare trasformaz graphicsConfig in ScriptableObject(s)
- verificare DevTools per backup ScriptableObjects
	https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f9

- verificare altri TODOs nel codice


## Bugfixing


## Improvements
- aggiungere campo updatedFields a notifica EntityUpdate

- sperimentare uso colliders vs resetVisibleEntities

- dialog scelta nome savefile
- generatore mappe dungeon (e citta'?)
	https://github.com/angian00/giangen/tree/master/src/giangen/generators
- visibility algorithms:
https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#:~:text=Bresenham's%20line%20algorithm%20is%20a,straight%20line%20between%20two%20points.
https://www.roguebasin.com/index.php/Restrictive_Precise_Angle_Shadowcasting

- riattivare e migliorare PathfindingRequest 
- generare cognomi degli autori dei libri
? spostare visibility status property da map tiles
+ refactoring Consumable

++ refactoring Entity-Component-System
	- fare git clone AnyRPG


## Minutiae
+ key handler per inventory view (focus su item)
- grafica base per skills view
- padding in popup text inventory item
+ Use some texture/shader for fog e terrain


## Errands
- raccogliere file utili sapientia*
- raccogliere txt Poliziano

- provare onGUI
? scouting musica (https://www.freesoundtrackmusic.com/)
? suoni ambientali

## Brainstorming
- Familiarity feedback
- Detail info panels



## Milestones


### 0.7
- irrobustire il layout delle ui presenti rispetto al cambio di risoluzione/aspect ratio (anchor points, etc.)
- aggiornare visuals a mockup figma

- fighter component
- monster factory architecture
- ripristinare colore e icone custom per entities


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
- disegnare HUD (vedi esempio gioco CrossCode)
- disegnare Character View
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
