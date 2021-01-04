# Gameplay flow

Gameplay is turn-based and managed through the following high level interactions:

- A mouse click on the Deck sprite, or pressing the `D` key, triggers `GameState.Next()`
- `GameState.Next()` determines the next action to be performed, based on the current state.
  - If already in the process of changing state, no further action is taken.
  - If the player has not yet been created (or is dead) a new player is generated.
  - If an Encounter exists or the player is in town, trigger the next step.
  - Else, create a new Encounter.
- Encounters manage logic and temporary state inherent to the encounter type, ~~and defer to GameState for general functionality like dealing cards, harming/healing the Player etc.~~ **Amendment 04/01/2021**: GameState is already exposing too much functionality in an uncontrolled manner i.e. `HarmPlayer` should not be a public static method which can be invoked arbitrarily. Let's instead give the Encounter a direct reference to the Player, since that's where the requirement for appropriate harm will be determined.
- Town stages are managed through a singleton class.

Addendum 04/01/2021: the architecture will be changing slightly toward an observer model. GameState will not rely on other classes invoking `GameState.Next()`, instead it will receive notifications when any cards have been moved to a new location, and will infer from context what action it should take internally (much as originally described above). The main motivation is to avoid complex code for 'pausing' GameState logic while cards are moving around the play area, since methods necessarily return when the animation begins rather than when it ends.

## Generating a new player

1. Invoke `DrawCards` on the Deck to get a Character Card.
2. Instantiate `Player` using the Character Card.
3. Invoke `DealCards` targeting the StagingArea.
4. Calculate HP from the dealt cards and invoke `Heal` on the Player.
5. Return cards in the StagingArea to the Deck.
6. Invoke `DealCards` targeting the Player.

## Encounter flow (GameState)

### Create new

1. Invoke `DealCards` targeting the StagingArea.
2. Instantiate `Encounter` using the dealt cards.

### Progress existing

Invoke `Advance` on the Encounter.
