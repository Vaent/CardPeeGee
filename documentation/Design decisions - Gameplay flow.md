# Gameplay flow

**Updated 2022/04/06-2022/10/17** to reflect planned changes.

Gameplay is defined by a series of phases as outlined in [Breakdown of core game logic](./breakdown-of-core-game-logic.md) which are managed through the following high level behaviours:

- `GameState` keeps track of the phase currently in progress, and universal game elements such as the player and deck. GameState is also responsible for initialising the next phase when the previous one wraps up.
- Individual phase definitions manage their own low-level logic and state.
- `GameController` handles events, including user input, and forwards messages as appropriate based on the current game state. Input is no longer filtered based on locking/unlocking GameState - the recipient of the message is solely responsible for determining whether the message requires any action.

## Phases (general)

When the scene is set up, phase 0 (Pre-game/player creation) will be loaded in GameState, which thereafter will always have exactly one phase loaded.

The new `GamePhase` interface will ensure messages can be passed through in a consistent manner. All phases will be notified of the following events:

- Phase loaded in GameState (i.e. initialisation)
- User interaction with the deck (mouse click or D key)
- User interaction with cards
- User interaction with context-specific GUI elements
- Cards have arrived at a new location
- Player HP has been updated

### Progression of game phases

0. Pre-game/player creation -> Encounter
0. Encounter -> Town
0. Town -> Encounter

PlayerCreator and Town will expose their singleton instances and will no longer be updated via static methods.

Encounters will still be instantiated case-by-case due to their complexity and highly variable nature. A new EncounterPhase class provides lightweight logic to create and manage those instances.

Endings are treated as artifacts which may appear within the above, not as separate game phases, since play resumes in the originating phase following a victory.

TODO: determine whether or not GameController will forward messages to the active GamePhase while an ending screen is open; most input should be blocked temporarily but the GamePhase might need to be notified of other events while an ending screen is open (i.e. if some cards have not finished moving before the ending is triggered). **Design to be revisited once the relevant low-level systems are fleshed out enough to inform this.** Preliminary expectation is that everything will be fully resolved prior to launching the ending sequence, but need to check for any events where this cannot be guaranteed.

### End existing encounter

Encounters must determine at the end of each step whether they are still active - in which case they will prompt the player to take action - or have been completed.

Once completed, the Encounter is responsible for tidying up after itself i.e. performing the following actions.

- Cards used in the encounter must be returned to the Deck, including:
  - score cards
  - any encounter cards that have not been given to the Player as loot
  - any cards played from the player's hand
- Animations must be ended/encounter graphics removed
- Text relating to the encounter must be cleared

Finally the Encounter will set a flag and notify GameState of its termination. GameState will verify that the flag has been set (so that code elsewhere cannot incorrectly terminate an active Encounter) then drop the Encounter instance and load the next phase.
