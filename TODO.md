# TODO

## Current sprint (v0.7)
- aggiornare visuals a mockup figma
	- cornice player
	- fog
	+ inventory
		- titolo (bitmap)
		- quadratini item

## Bugfixing


## Improvements
- limitare movimento map camera ai bordi
- usare pathfinding per ai ostile

- krita: 
	- map tile wall (sassi) 
	- icone items:
		- pozione
		- libro

+ provare painting dungeon custom

- quote random all'inizio
- feedback visuale stato mostro
- strutturare meglio il feedback del combattimento (markup statusline?)
- creare "recipes" per combatStats, es. "1d3"
- aggiungere campo updatedFields a notifica EntityUpdate
- cache monster templates (MonsterFactory), e config probabilita' mostri

- sperimentare uso colliders vs resetVisibleEntities

- dialog scelta nome savefile

- riattivare e migliorare PathfindingRequest 
- generare cognomi degli autori dei libri
? spostare visibility status property da map tiles
- Restore ScriptableObjects in editor tools
+ refactoring Consumable
+ item identification mechanism
+ procedura iniziale di check consistency (es. validita' spriteIds)

++ refactoring Entity-Component-System
	- fare git clone AnyRPG


## Minutiae
+ key handler per inventory view (focus su item)
- grafica base per skills view
- padding in popup text inventory item
+ Use some texture/shader for fog e terrain


## Errands
- provare mappa dipinta a mano
- brainstorming actor effects, badges, etc.

? scouting musica (https://www.freesoundtrackmusic.com/)
? suoni ambientali

## Brainstorming


## Milestones



### 0.x
- ui mobile-friendly
- pathfinding per hostile ai


### 0.xx
- studiare shaders (https://www.ronja-tutorials.com/)


## Prototyping
### Procedural Generation
- world generator
	+ ghiaccio su acqua con temperature molto basse
	- pensare a multiscala
	? generare fiumi su mappe a scala ridotta
	+ indagare algorithm generazione laghi usando simulaz pioggia? calcolo minimi locali? "flood fill"
	- creare laghi
	x razze

- Simulazione villaggi con relationships
	- eventi: quarrels, falls in love, helps; 
	- friendship levels
	- children; sickness, deaths
	- merchants
	- job details: merchants, craftsmen
	- priests, scholars
	- fairs
	- competitions
	+ factions
	+ travel
	+ secrets
- visualizzazione network relazioni


### Biomes
https://github.com/GrandPiaf/Biome-and-Vegetation-PCG 
- 

### Visual Design
- design HUD (vedi esempio gioco CrossCode)
- design Character View

- https://www.gameuidatabase.com/
- provare icone pozioni con solo liquido colorato
