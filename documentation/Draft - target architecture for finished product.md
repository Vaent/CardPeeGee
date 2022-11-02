# Target architecture (WIP 2022/11/02)

## Purpose

To refine the project structure with a view to improved extensibility and maintainability, informed by ongoing learning during development.

## Key points

- The "refreshed" C# code base was initially heavily informed by the original UnityScript code, in which the game logic/model was shaped around the view.
- A core aim of the refresh is to replace that structure with more modular code which better reflects the abstract concepts from which the game is formed.
- This has been attempted in a light-touch way while aiming to deliver a prototype relatively quickly. Design decisions are made based on current requirements and understanding, in a "semi-Agile" manner which provides flexibility but requires significant changes from time to time.
- Having an abstract high-level model should better inform future changes while still allowing for rapid prototyping.
- The high level model presented here is independent of the low level [Breakdown of core game logic](./Breakdown%20of%20core%20game%20logic.md).
- **Implementing this model is a long term goal.** Some deviation is expected during early stages of delivery, however there is an expectation that any significant future refactoring will aim for alignment with this model.

## Model

This is derived directly from the rules of the card game (published at [vaent.uk](https://cardpeegee.vaent.uk/) as at 2022/11/02) rather than reverse engineering the video game. It can be expanded/elaborated as necessary.

### Game Master/Dealer

All actions, decisions, events etc are handled by a GM. The GM will reference the rules which apply to any given situation, deal cards as required, increase or decrease a player's HP, determine what actions a player may take, and so on.

The GM is responsible for the following actions:

- Admitting players to the game (creating characters)
- Determining which player's turn is next
- Dealing cards
- Applying any effects caused by the current context e.g. changing the player's HP, gifting cards to the player
- Informing the player of actions they can perform
- Handling player actions

*The implementation as at 2022/11/02 has some of these functions performed by GameState and GameController, while others are handled independently by Encounters and the Town class linking directly to the player. Single-player state is assumed universally.*

### Players

Each player has ownership of their character card, their hand (including active cards) and their HP.

A player may perform actions as advised to them by the GM:

- Activate, play, or discard cards from their hand
- Progress the current scenario
- Select a target for context-specific actions e.g. disarming traps on a treasure chest
- Abandon an encounter

All the player's decisions are communicated to the GM - the player does not act in isolation nor directly influence any scenario.

### Turns

Each turn consists of one encounter followed by one Town session.

A turn is associated with a single player, though in a multiplayer game using the expanded ruleset other players may be involved in the encounter.

Every part of the turn follows this generic sequence:

- Determine what actions the player can take
- Receive input from the player
- Determine any effects to be applied

Encounters will loop this sequence as necessary until they are resolved.

There is no direct interaction between the player and the encounter/town; everything is moderated by the GM.

### Sequence of events including transfer of control at different steps

1. GM begins a game by creating a character for each player.
1. GM initiates a turn for the first player (P1) and requests confirmation to proceed.
1. P1 confirms when they are ready to begin.
1. GM deals cards to define the encounter.
1. (Begin loop) GM queries the encounter for next steps.
1. The encounter advises one of the following:
  - an outcome has been determined with effects to be applied; or
  - input is required based on a list of permitted actions; or
  - the encounter has been resolved (exit loop).
1. If an outcome is returned, GM applies the effects, then (repeat loop); or
1. If input is required, GM presents options to P1.
1. P1 communicates their selection(s) to GM.
1. GM updates encounter based on P1's selection(s), then (repeat loop).
1. (End of loop) GM queries town for P1 tax/charity eligibility.
1. Town advises one of the following:
  - charity should be delivered; or
  - input is required to pay tax; or
  - null activity in this stage.
1. If charity applies, GM deals a new card to P1; or
1. If tax applies, GM presents options to P1 => P1 communicates their selection to GM => GM updates town accordingly.
1. GM queries town for P1 shopping eligibility.
1. Town advises GM of available options.
1. GM presents options to P1.
1. P1 communicates their selection(s) to GM.
1. GM updates town based on P1 selection(s).
1. GM queries town for next steps.
1. If new cards were purchased:
  - Town instructs GM to deliver the appropriate number of cards;
  - GM deals cards to P1;
  - GM queries town for next steps.
1. Town advises GM of available options for healing.
1. GM presents options to P1.
1. P1 communicates their selection(s) to GM.
1. GM updates town based on P1 selection(s).
1. GM queries town for next steps.
1. If new cards were purchased:
  - Town instructs GM to deliver the appropriate number of cards;
  - GM deals cards to P1;
  - GM queries town for next steps.
1. Town informs GM that the turn has ended.
1. GM moves to the next player and initiates their turn, requesting confirmation and proceeding as above.

At any point where the GM applies some effect, they will also check for player death or victory conditions being met and break out of the normal flow as necessary.
