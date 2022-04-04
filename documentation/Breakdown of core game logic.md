# Breakdown of core game logic (CardPeeGee vanilla)

This is a coarse-grained, blow-by-blow description of events managed by the game engine, presented as pseudocode to help identify traits which could be more effectively shared between components.

This work is motivated by a need to refine/augment the original [Gameplay flow](Design decisions - Gameplay flow.md) and refactor the low-level GameState model.

## 0. Pre-game

- Prompt the user to create their character ("player")

## 1. Create player

- While character card is null:
  - Deal 1 card
  - If the dealt card is a King or Queen:
    - Assign it to the player as their character card
    - Return all other dealt cards to the deck
  - Loop
- Deal 3 cards
- Calculate initial HP using the dealt cards
- Apply initial HP to player
- Return dealt cards to the deck
- Deal 5 cards to the player's hand

## 2. Main game loop

### 2a. New day

- Prompt the user to start an encounter

### 2b. Encounter

ENCOUNTER_SETUP:
- Deal 3 cards
- Assign the dealt cards to the encounter
- Determine the encounter type using the first card ("agitator")
- Determine the encounter stats using all the cards (agitator and "props")

Resolve encounter according to type

ENCOUNTER_CLEANUP:
- Return any played cards to the deck
- Return any remaining encounter cards to the deck

#### Encounter type: Monster

- Allow the user to select/play/activate their cards
- Prompt the user to begin the battle when they have selected all the cards they want
- When the user begins the battle:
  - Calculate the player's final stats based on played cards
  - While player is alive and monster is alive:
    - Deal [player_DL] cards
    - Calculate the player's score from dealt cards and player_ATK
    - Deal [monster_DL] cards
    - Calculate the monster's score from dealt cards and monster_ATK
    - If player score > monster score:
      - Apply [player_ATK] damage to monster
    - ElseIf monster score > player score:
      - Apply [monster_ATK] damage to player
    - Else: (nothing happens)
    - Return dealt cards to the deck
    - If monster is alive:
      - Prompt user to start the next round of combat
      - When the user begins the next round:
        - Loop While
- Transfer the encounter "prop" cards to the player's hand

#### Encounter type: Treasure

- While the treasure is protected by any trap:
  - Allow the user to select/play/activate their cards
  - Prompt the user to try and disarm a trap, or abandon the treasure
  - If the user abandons the treasure:
    - Goto ENCOUNTER_CLEANUP
  - When the user makes a disarm attempt:
    - Deal 1 card
    - Calculate the player's score from played cards and dealt card
    - Deal 1 card
    - Calculate the trap's score from trap card value and dealt card
    - If player score > trap score:
      - Return trap card to the deck
    - ElseIf trap score > player score:
      - Apply [trap card value] damage to player
    - Else: (nothing happens)
    - Return dealt cards to the deck
    - Loop While
- Transfer all remaining encounter cards to the player's hand

#### Encounter type: Healer

- While there is any jailor:
  - Allow the user to select/play/activate their cards
  - Prompt the user to fight a round when ready, or abandon the healer
  - If the user abandons the healer:
    - Goto ENCOUNTER_CLEANUP
  - When the user begins a round:
    - Calculate the player's stats based on played cards
    - Deal [player_DL] cards
    - Calculate the player's score from dealt cards and player_ATK
    - For each jailor:
      - Deal [jailor_DL] cards
      - Calculate the jailor's score from dealt cards and jailor_ATK
      - If player score > jailor score:
        - Apply [player_ATK] damage to jailor
        - If jailor is dead:
          - Return jailor card to deck
      - ElseIf jailor score > player score:
        - Apply [jailor_ATK] damage to player
      - Else: (nothing happens)
      - Return jailor's dealt cards to the deck
    - Return dealt cards to the deck
    - Loop While
- While there is a fee:
  - Allow the user to select/play/activate their cards
  - Prompt the user to abandon the healer if they don't want to pay
  - If the user abandons the healer:
    - Goto ENCOUNTER_CLEANUP
  - When the user plays a card:
    - Calculate the total that has been paid
    - If total paid > fee:
      - Exit While
    - Loop While
- Apply [healer's healing amount] HP of healing to player
- If healer has any potions:
  - Transfer all potions to the player's hand

#### Encounter type: Trap

- Deal 1 card
- Deal [number of activated cards] additional cards
- Calculate the player's evasion score from dealt cards
- If evasion score > trap difficulty:
  - Calculate actual damage = trap damage - 2*(evasion score - trap difficulty)
  - If actual damage < 0:
    - Set actual damage = 0
- Else:
  - Set actual damage = trap damage
- Apply [actual damage] damage to player

### 2c. Town

- If cards held by player (hand + activated) > 6:
  - Allow the user to select/discard their cards
  - Prompt the user to discard a card as tax
  - When the user discards a card:
    - Prevent further card selection
    - Return the discarded card to the deck
- ElseIf cards held by player (hand + activated) < 4:
  - Deal 1 card to the player's hand as charity
- Else: (nothing happens)
- Allow the user to select/play/activate their cards for shopping
- Prompt the user to proceed when they have selected all the cards they want
- When the user proceeds:
  - Prevent further use of player's cards
  - If any cards were played:
    - Calculate the amount spent
    - Calculate the number of cards purchased
    - Return all played cards to the deck
    - Deal [number purchased] cards to the player's hand
- Allow the user to select/play/activate their cards for healing
- Prompt the user to proceed when they have selected all the cards they want
- When the user proceeds:
  - Prevent further use of player's cards
  - If any cards were played:
    - Calculate the amount of healing
    - Apply [calculated amount] HP of healing to player
    - Return all played cards to the deck

### ITERATE MAIN GAME LOOP

## 3. Endings

Endings interrupt the [main game loop](#2-main-game-loop) as soon as they are triggered.

ENDING_SETUP:
- Freeze gameplay
- Block user input
- Fade the game view (mask it with a semi-transparent overlay)

### 3a. Player death

- Display death message
- Prompt the user to click anywhere to restart
- Enable user input
- When the user clicks to restart:
  - Reload the scene (clear all and restart from the [pre-game stage](#0-pre-game))

**Triggered when:** Player HP is reduced to or below zero

**Can occur during:**
- Monster encounter (while fighting a round of combat)
- Treasure encounter with trap (when attempting to disarm)
- Healer encounter with jailor (while fighting a round of combat)
- Trap encounter

### 3b. Victory: Goliath

- Display Goliath victory message
- Prompt the user to click anywhere to continue
- Enable user input
- When the user clicks to continue:
  - Restore the game view
  - Resume gameplay from immediately before the ending sequence

**Triggered when:** Player HP rises to or above 150 for the first time

**Can occur during:**
- Healer encounter (when healing is received)
- Town - healing phase

### 3c. Victory: Full Court

- Display Full Court victory message
- Highlight all cards contributing to the victory and move them to "display slots"
- Prompt the user to click anywhere to continue
- Enable user input
- When the user clicks to continue:
  - Restore the game view
  - Return all "victory cards" to their previous locations in the player's hand/active cards
  - Resume gameplay from immediately before the ending sequence

**Triggered when:** Player's [hand + active cards] includes a Jack, Queen and King all of the same suit, for the first time (per suit)

*N.B. in rare circumstances this victory can be triggered for multiple suits simultaneously, and/or at the same time as Elemental Union*

**Can occur during:**
- Monster encounter (when receiving loot)
- Treasure encounter (when receiving treasure)
- Healer encounter with potions (when receiving potions)
- Town - charity phase
- Town - shopping phase

### 3d. Victory: Elemental Union

- Display Elemental Union victory message
- Highlight all cards contributing to the victory and move them to "display slots"
- Prompt the user to click anywhere to continue
- Enable user input
- When the user clicks to continue:
  - Restore the game view
  - Return all "victory cards" to their previous locations in the player's hand/active cards
  - Resume gameplay from immediately before the ending sequence

**Triggered when:** Player's [hand + active cards] includes a Jack, Queen and/or King from every suit, for the first time

*N.B. in rare circumstances this victory can be triggered at the same time as Full Court*

**Can occur during:**
- Monster encounter (when receiving loot)
- Treasure encounter (when receiving treasure)
- Healer encounter with potions (when receiving potions)
- Town - charity phase
- Town - shopping phase

### 3e. Victory: Master of the Elements

- Display Master of the Elements victory message
- Highlight all cards contributing to the victory and move them to "display slots"
- Prompt the user to click anywhere to continue
- Enable user input
- When the user clicks to continue:
  - Restore the game view
  - Return all "victory cards" to their previous locations in the player's active cards
  - Resume gameplay from immediately before the ending sequence

**Triggered when:** Player has a Jack and/or Ace from every suit activated, for the first time

**Can occur during:**
- Monster encounter (while selecting cards before the fight begins)
- Healer encounter with jailor (while selecting cards before each round of combat)
- Treasure encounter with trap (while selecting cards before each attempt to disarm)
- Town - shopping phase
- Town - healing phase
