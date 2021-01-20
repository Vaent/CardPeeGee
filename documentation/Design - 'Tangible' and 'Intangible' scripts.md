# Tangible vs intangible scripts

## Preamble

Two distinct categories of C# classes are emerging from the early development work, namely those which extend MonoBehaviour and those which don't.

Since the former tend to interact directly with GameObjects in the Unity environment, I have begun referring to them as "tangible" classes, while those which only exist to manage internal state and logic are "intangible".

The distinction is important because intangible classes ultimately rely on some tangible class to produce an effect based on whatever processing they have performed. That effect may be a graphic or text displayed on the screen, a sound clip played through the JukeBox etc.

Suspending execution of code is a key piece of functionality which requires a tangible class, but is often needed within intangible methods - see Scripts/Util/Timer.cs and its various uses.

This document is a first attempt at formalising the structure and relationship between these two categories of scripts for consistency and ease of comprehension.

## Pattern

- Any tangible script which directly relates to a specific GameObject will be attached to that object in the Unity environment.
  - Public methods of a "specific tangible" script will normally be accessed through a reference to the instance.
- Tangible scripts which provide generic functionality should be attached to the EventSystem GameObject.
  - Public methods of a "generic tangible" script will normally be static, delegating to a singleton instance if functionality from MonoBehaviour is required.
- Intangible scripts do not appear in the Unity environment.

## Existing implementations

The below is not a comprehensive list of all C# scripts; rather, it is a quick summary of those classes whose purpose and nature might not be immediately clear.

### Intangible scripts

| C# class | Description |
| --- | --- |
| GameState | Acts as a central hub for game logic |
| Card | The virtual representation of an individual card; also provides an interface to the card's GameObject and its tangible CardController |
| Player | The virtual representation of a player; has direct access to tangible CharacterCardZone and HandZone instances |
| PlayerCreator | Utility class which manages the process of generating a new Player |
| Encounter | Abstract class; implementations will contain logic for a specific type of encounter and will operate accordingly on items passed in by reference, while remaining broadly ignorant of the game world |

### Tangible scripts - specific

| C# class | Description |
| --- | --- |
| CardController | Manages the in-game representation of an individual card: movement, user interaction etc |
| CardZone | Abstract class which must be implemented by any script which may assume responsibility for some number of cards |
| Deck : CardZone | Represents the in-game deck of cards |
| Player.CharacterCardZone : CardZone | Lightweight class containing a single card which represents the player in the game world |
| Player.HandZone : CardZone | Manages any cards received/held by the player |
| StagingArea : CardZone | The section of the play area to which new cards are dealt by default, e.g. when creating encounters |

### Tangible scripts - generic

| C# class | Description |
| --- | --- |
| JukeBox | This sits on a GameObject with an AudioSource component, but its functionality is exposed globally through static methods so that intangible classes don't need to maintain a reference to its instance |
| Timer | Provides an overloaded "DelayThenInvoke" method which accepts a callback along with any parameters which should be passed through to it |
