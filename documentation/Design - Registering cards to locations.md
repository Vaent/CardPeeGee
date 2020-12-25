# Registering cards to locations

## Preamble

This is a design change from the original game, where all cards along with other game elements were monitored and directly controlled by a heavyweight game state manager. The change is motivated by various considerations:

- need to break up the bloated state manager
- reduce coupling to a specific implementation of the game logic
- give cards more ownership of their inherent properties/behaviours
- open the door to future adaptation and extension of game features

### Basic concept

Each card must always have a well-defined location to prevent it being 'lost' when passed from one location (e.g. the deck) to another (e.g. the player's hand).

When a card is created, it is immediately associated with a valid section of the play area. Thereafter, any time it is transferred to a different location, the move will be recorded by the card and by both the new and old locations, to ensure all relevant parties agree on where the card is.

## Implementation

Any part of the play area which may be responsible for cards MUST extend the `CardZone` abstract class.

Cards are created as instances of the `Card` class, which construct for themselves a `GameObject` to represent them in the scene.

### CardZone

- inherits from MonoBehaviour
- maintains a private list of cards currently registered to it
- exposes a copy of that list to prevent direct modification of the underlying field
- defines core functionality for transferring cards in (`Accept`) and out (`Unregister`) of the location
  - `Accept` instructs every card transferred in to update its recorded location
  - `Unregister` expects that the supplied card no longer records the CardZone as its current location
- requires implementing classes to define any additional functionality which should be applied when new cards are received (`ProcessNewCards`)
  - this may include logging, displaying the cards in the new location, recalculating some derived value etc.

### Card

- maintains a private record of the CardZone where it is currently located, exposed via a read-only accessor (`CurrentLocation`)

### Transfer process

![Card-CardZone messaging sequence diagram](./diagrams/Card-CardZone%20messaging.png)

The process is initiated by an instruction to the receiving (new) CardZone to `Accept` a list of Card instances.

The new CardZone adds all cards in the accepted list to its existing list, which may or may not already contain some cards, and invokes `RegisterTo` on each Card, passing itself as the method argument.

When a Card receives a `RegisterTo` call, it performs no action if it is already internally registered to the supplied location, otherwise it updates its internal location and invokes `Unregister` on the previous location, passing itself as the method argument.

On receiving the `Unregister` call, that CardZone checks whether the Card is present in its current list of registered cards; verifies with the Card that the association is no longer recorded; and removes the Card from its internal list.

Finally, after all cards have been accepted in the new CardZone, it invokes the implementation-specific `ProcessNewCards` method.

#### Commentary

The Card and CardZone classes are very tightly coupled, as seen in the class diagram:

![Card-CardZone class diagram](./diagrams/Card-CardZone%20class%20diagram.png)

While not ideal, this allows for more robust communication between the classes and thereby reduces the risk of failure in the end-to-end tracking process. Example: if an observer tries to directly Unregister a Card from a CardZone, the attempt will fail since the Card has not been informed of the change.

Further consideration is needed as to whether this should be relaxed (requiring developers to ensure their code elsewhere cannot cause cards to become 'lost') or tightened further (e.g. specifying up front where the Card is being transferred from, not just the location which will receive it, and increasing the level of validation). I'm inclined toward the latter but need to see more examples of the process in action before refining it.
