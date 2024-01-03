# Ventura

Ventura is a planet exploration game.
The player has been deployed on an unknown planet with the task of accumulating as much knowledge as possible about its geography and its inhabitants.

## Introduction

### Main Concept

The gameplay resembles a classic roguelike: movement is controlled with the keyboard, and is performed in discrete steps, on a rectangular grid.
The map is on a multiple scale, i.e. when the player encounters a place of interest he/she jumps to a higher-resolution map.

Key to the gameplay is the *familiarity* that is gained about a place or an item. More familiarity with an item means that it is more useful, be it a technological tool or a cultural artifact (in which case the lore about the originating civilization improves). More familiarity with a place is a generalization of the usual map discovery mechanism of exploration games: it improves the stats for the civilization, biome, etc associated with the place.

The player has different skills, with their own stats, embodied as the *specialists* that belong to the mission: Archaeologist, Biologist, Linguist, Technologist, ...


The growing body of knowledge being discovered can be browsed by topic, by type, by free text search.
The items and places names and features are procedurally generated. Ideally, the generation is sophisticated enough to build a world history with consistent emerging narratives.

The focus is on the exploration and collection process itself, with minimal micromanaging involved: it is almost an idle game. Some adversary mechanics (mini-games) may need to be introduced to keep the gameplay engaging.




### Gameplay Messages Examples
You have discovered a new city [Aelirun]!

You have discovered a new civilization [Green Insectoids of Rhun]!

You have discovered a new building [Cathedral of Sorrow]!

You have got more familiar with [Green Insectoids of Rhun]



### Feature Brainstorming
- map generator

- risorse/strutture/item

- item:
	- "artefatti culturali" (libri, audiovisivi, statue e decorazioni, monumenti)
	- specimen biologici
	- manufatti tecnologici

- sfruttamento degli item per migliorare le skill

- livelli di difficolta' nella research

+ automatizzazione micro-management

- civilta'
- fauna
- knowledge base

+ pathfinding


### Other Ideas
il protagonista è un costrutto artificiale con un’anima umana. non ha bisogno di mangiare.
ogni tanto avvengono frammenti di dialogo tra lui e il suo super-io artificiale.
la missione è esplorare il mondo per capire cosa è successo, dopo millenni?.
per ora mi concentro sui libri.
i libri chiave sono quelli che consentono di informarsi sui demoni.
i demoni hanno delle tane sigillate. possono essere affrontati solo quando la conoscenza è sufficiente. (esorcismi in libri specifici?)
esistono tante lingue. ogni libro è scritto in una lingua. esistono specifici manuali per migliorare la conoscenza di una lingua. la conoscenza migliora anche provando a leggere libri in quella lingua, purchè la loro difficoltà sia piccola.
esistono lingue “pidgin”, facili da apprendere ma di impatto limitato.

il protagonista dice frasi pessimistiche, sempre più spesso mano a mano che il livello di ansia aumenta. anche la musica, gli effetti sonori, la palette di colori? diventano piu' minacciosi.
al livello massimo di ansia, game over. sconfiggere i demoni riduce molto l’ansia. leggere libri riduce un pochino l’ansia. 

ci si può trasferire (teletrasportare) vicino a un sito noto anche solo digitandone il nome.


## How to Play

Keybindings (so far):

|    Key    | Action           |
| --------- | ---------------- |
| Numpad 8  | Move Up          |
| Numpad 2  | Move Down        |
| Numpad 4  | Move Left        |
| Numpad 6  | Move Right       |
| Numpad 5  | Wait             |
|           |                  |
|    I      | Inventory View   |
|    K      | sKills View      |
|    M      | Main View        |
|           |                  |
|    Q      | Quit Game        |
|    N      | New Game         |
|    S      | Save Game        |
|    L      | Load Game        |
|           |                  |
| --------- | ---------------- |


## Acknowledgements
Concept art created with [Deep Dream Generator](https://deepdreamgenerator.com/).

