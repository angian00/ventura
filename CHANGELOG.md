# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).


## [Unreleased]
### Added
- item generation in MapGenerator
- azione PickupItem
- zoom in/out in map

### Changed
- Extracted SystemManager from GameManager to remove dirty DontDestroyOnLoad hack


## [0.5] - 2024-01-04
### Added
- start menu
- book title generator
- book author field
- system commands (New Game, Exit Game) with confirmation popup
- saving and loading games

### Changed
- Tidied up Awake and Start for MonoBehaviours
- Refactored input handlers to be more selective
- Used camera stack to switch between views, popup
- Extracted GameState from Orchestrator
- Switched to EventSystem for decoupling game state, views, inputs, system commands
- Refactored Actions architecture with EventSystem
- Given more significant names to Behaviour classes


### Fixed
- game exit on Q keypress


## [0.4] - 2023-12-30
### Added
- status line
- view switching
- skeleton inventory view
- skeleton skills view
- Consumable game component
- Skills game component
- inventory GameLogic


### Changed
- Refactoring of input system
- Refactoring of PendingUpdates


## [0.3] - 2023-12-28
### Added
- Perlin noise for wilderness map generation
- name generation from text files
- keybinding for numpad keys (with diagonal movement)
- keybinding for wait action
- auto-repeat behaviour for keyboard keys
- more detailed tile info in ui

### Changed
- tuned map generation thresholds


## [0.2] - 2023-12-28
### Added
- fog of war
- multi-level maps
- basic arrow-key movement
- basic screen layout (map view, ui view)
- basic game logic architecture
