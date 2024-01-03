# Technical Notes

## Code organization
All source file are contained in Assets/Scripts.
The subfolder structure matches the namespaces.
`GameLogic/*` classes have no dependencies on Unity libraries.
Most Unity-related classes are in `Unity/*`; there can be exception, as in `Util/UnityUtils`.
In particular, `Unity/Behaviours/*` contains all scripts which are 1-to-1 associated with non-empty GameObjects. while `Unity/*Manager.cs` are more abstract classes, which nonetheless tap into Unity infrastructure, mainly into Unity EventSystem.

