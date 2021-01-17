# Tech debt

## In scope

### Required

- Manage layering of cards which could overlap e.g. in staging area
  - Proposal: set the Sprite Renderer's "Order in Layer" property equal to each card's index in the appropriate list. If the cards are displayed in a different order to how they are stored (as is the case for the player's hand) make sure indices are taken from the sorted list, not the CardZone base list.
- Allow virtual classes (e.g. GameState) to suspend execution without extending MonoBehaviour just for the sake of accessing IEnumerator functionality.
  - Proposal: set up a utility class which extends MonoBehaviour, with functions which accept a callback and invoke it after a given delay.

### Desired

- Improve text display system (e.g. messages showing progress of player creation).
  - Add a utility to manage text placement (get rid of `"\n\n\n\n..."`).
  - See if we can find a better way to update text than directly setting a TextMesh.text value i.e. abstract the TextMesh away from the class which wants to display a message but doesn't really care how it's done.
  - Proposal: introduce a separate text display manager which can potentially create and drop TextMesh instances as required rather than relying on preset objects.

## Out of scope

- Manual/active shuffling of cards to replace random selection from deck.
