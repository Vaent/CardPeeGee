Gameplay is turn-based and managed through the following high level interactions:

- A mouse click on the Deck sprite, or pressing the `D` key, triggers `GameState.Next()`
- `GameState.Next()` determines the next action to be performed, based on the current state.
  - If already in the process of changing state, no further action is taken.
  - If the player has not yet been created (or is dead) a new player is generated.
  - If an Encounter exists or the player is in town, trigger the next step.
  - Else, create a new Encounter.
- Encounters manage logic and temporary state inherent to the encounter type, and defer to GameState for general functionality like dealing cards, harming/healing the Player etc.
- Town stages are managed through a singleton class.

# Generating a new player

1. Invoke `DrawCards` on the Deck to get a Character Card.
2. Instantiate `Player` using the Character Card.
3. Invoke `DealCards` targeting the StagingArea.
4. Calculate HP from the dealt cards and invoke `Heal` on the Player.
5. Return cards in the StagingArea to the Deck.
6. Invoke `DealCards` targeting the Player.

# Encounter flow (GameState)

## Create new

1. Invoke `DealCards` targeting the StagingArea.
2. Instantiate `Encounter` using the dealt cards.

## Progress existing

Invoke `Advance` on the Encounter.
