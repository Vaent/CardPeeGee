# Gameplay flow

Gameplay is turn-based and managed through the following high level interactions:

- A mouse click on the Deck sprite, or pressing the `D` key, triggers `GameState.Next()`
- `GameState.Next()` determines the next action to be performed, based on the current state.
  - If a process is currently being resolved, no further action is taken.
  - If the player has not yet been created (or is dead) a new player is generated.
  - If an Encounter exists or the player is in town, trigger the next step.
  - Else, create a new Encounter.
- Encounters manage logic and temporary state inherent to the encounter type. They are given a reference to the Player so that they can directly apply damage/healing, check stats etc.
- Town stages are managed through a singleton class.

## Locking and unlocking GameState

Before beginning a new action, GameState will lock itself to prevent `Next` being repeatedly triggered while a process is playing out. Once the process is completed and player input is allowed again, the responsible entity needs to unlock GameState. This will usually happen after some animation/audio clip has finished playing, often when score cards are returned to the Deck after resolving an event.

While the GameState is locked, feedback on internal state changes will still be picked up and passed on, e.g. notification of cards arriving at a CardZone. Only player input (or code behaving like player input) is blocked.

## Generating a new player (Util/PlayerCreator)

1. Invoke `DrawCards` on the Deck to get a Character Card.
1. Instantiate `Player` using the Character Card.
1. Invoke `DealCards` targeting the StagingArea.
1. Calculate HP from the dealt cards and invoke `Heal` on the Player.
1. Return cards in the StagingArea to the Deck.
1. Invoke `DealCards` to get a starting hand.
1. Transfer the dealt cards to the Player.

## Encounter flow

### Create new encounter

This process is managed by GameState.

1. Invoke `DealCards` targeting the StagingArea.
1. Instantiate `Encounter` using the dealt cards.
1. Associate the Player through `encounter.HappensTo(player)`.
1. Invoke `encounter.Begin()`.

### Progress existing encounter

Based on player input, GameState invokes `Advance` on the current Encounter instance. Each implementation of Encounter will have its own logic for checking cards that the player has played from their hand, dealing cards from the Deck to determine temporary scores and so on.

Some encounter types resolve automatically: Healers with no jailor/fee; all Traps; and Treasure with no trap. These encounters will perform all necessary tasks as soon as they `Begin` and will then self-terminate, so will never be actively progressed.

### End existing encounter

Encounters must determine at the end of each step whether they are still active - in which case they will prompt the player to take action - or have been completed.

Once completed, the Encounter is responsible for tidying up after itself i.e. performing the following actions.

- Cards used in the encounter must be returned to the Deck, including:
  - score cards
  - any encounter cards that have not been given to the Player as loot
  - any cards played from the player's hand
- Animations must be ended/encounter graphics removed
- Text relating to the encounter must be cleared

Finally the Encounter will set a flag and notify GameState of its termination. GameState will verify that the flag has been set (so that code elsewhere cannot incorrectly terminate an active Encounter) then clear the Encounter instance and go to the next GameState Phase.
