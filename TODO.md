# TODO

## Current sprint (v0.8)

### Roguelike base e UI
- ui mobile-friendly
- limitare movimento map camera ai bordi
- usare pathfinding per ai ostile
- migliorare colori status line
- migliorare tiling fog
- animazione player e combat
- quote random all'inizio
- minimap
- provare un altro stile di painting dungeon

### Generators
- world generator
	- investigare erosion
	- investigare migliorie civ generator
		- smussare confini frastagliati
		- affinita' con terrain

	- generazione citta e strade
	- investigare gestione swatches colore


## Bugfixing


## Improvements
? visibilita' foresta

- feedback visuale stato mostro
- strutturare meglio il feedback del combattimento (markup statusline?)
- creare "recipes" per combatStats, es. "1d3"
- Restore ScriptableObjects in editor tools

- dialog scelta nome savefile

- riattivare e migliorare PathfindingRequest 
- sperimentare uso colliders vs resetVisibleEntities
+ generare cognomi degli autori dei libri
? spostare visibility status property da map tiles
+ aggiungere campo updatedFields a notifica EntityUpdate
+ cache monster templates (MonsterFactory), e config probabilita' mostri
+ refactoring Consumable
+ item identification mechanism
+ procedura iniziale di check consistency (es. validita' spriteIds)

++ refactoring Entity-Component-System
	- fare git clone AnyRPG


## Minutiae
+ key handler per inventory view (focus su item)
- grafica base per skills view
- padding in popup text inventory item


## Errands
- brainstorming actor effects, badges, etc.

? scouting musica (https://www.freesoundtrackmusic.com/)
? suoni ambientali

## Brainstorming


## Milestones



### 0.x

### 0.xx
- studiare shaders (https://www.ronja-tutorials.com/)


## Prototyping
### Procedural Generation

	x ghiaccio su acqua con temperature molto basse
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
