# Tech debt

## Bugs

- The only conversion option for cards with value 2 should be Spades, and only in combat since the "Zero of Spades" has a useful function there. Currently the only check is whether a sprite exists for the requested conversion. There is no feedback to the player if that check fails.
- (observed once while running WebGL build 0.0.1 in Chrome) During cleanup of a Trap encounter with 4 score cards: <King of Spades | 02 of Diamonds | 06 of Clubs | 04 of Spades> the King of Spades stayed in place while the other score cards and encounter cards were correctly returned to the Deck. This occurred while spamming the D key to check for any issues caused by failing to handle input correctly - may or may not be related. Debug messages in the DevTools console indicate that the 'virtual' card was returned to the Deck as expected but movement was not recorded (all other cards reported completion of their movement).

## In scope

### Required

- Implement player death properly. This should be picked up as part of the general build process but a reminder is added here since a workaround is currently in place and this is a core mechanic which **must not** be overlooked!
- Document visual layers and their priority levels, begin allocating GameObjects to the appropriate layers.
- Improve GameState locking/unlocking and use of Next()/Encounter.Advance() as that pattern is not so broadly applicable as it was initially assumed to be; q.v. traps on chests and paying healer fees.
- Uncouple Battle encounters from Healer battles so that they only share relevant logic/state.
- Rationalise debugs/reduce spam.
- Review all uses of `Timer.Delay(...)` and `Timer.DelayThenInvoke(...)` to ensure the pace of automatic actions is appropriate - some messages are appearing/disappearing much too quickly.

### Desired

- Apply namespaces to scripts in subfolders of the root directory `Assets/Scripts/` (this has already been done for `Text` and `Text.Excerpts`)
- Reduce the use of magic numbers e.g. number of cards in starting hand.
  - Ultimately most of these parameters should be redefinable to support extensions to the base game.
- Separate out the responsibilities of GameState; state management is too tightly coupled to object management (e.g. movement of cards) and this class is likely to bloat if not refactored.
  - More control can be handed to Encounter and Town classes.
  - Consider giving Encounter & Town a common interface to streamline object management (they should be notified about the same events).
- Review usage of `static readonly` fields, replace with auto-implemented properties if the value may be made customisable in future.
- Amend debug/print functions to avoid line breaks in strings which contain newline characters (particularly problematic for TextManager debugs).
  - Consider replacing Text.Excerpt content with the excerpt name (and args?) in debugs.
  - It may be sufficient to have Text.Excerpt implement IFormattable with a compact/debug format option, and invoke this where appropriate, since Excerpts are the main culprit.

## Out of scope

- **Accessibility!!!** Should be in scope but since the original game was not built with access needs in mind, retrofitting features like text resizing, control over colour palettes etc will require disproportionate effort and probably require delivery of additional tech debt, like being able to resize/reconfigure the play area.
- Procedurally generate card sprites from name/number and suit. It should be possible to scrap the 52 individual sprites (and additional variants) which all have exactly the same layout, and instead have an abstract card face with spaces for the number/letter and the suit pip.
- Have converted cards show their natural suit/value as well as the converted suit/value.
  - Suggested implementation is to bisect the card face diagonally, with converted pips (on yellow background) in the top left section and natural pips (on white background) in bottom right.
- Replace fixed dimension 700x500 play area with a more flexible frame.
  - May still be constrained to landscape format but should at least accommodate different screen resolutions (SD/HD/FHD etc) and ideally different aspect ratios (between 4:3 and 16:9, and ideally beyond).
  - A nice-to-have extension would be flexible placement of game elements allowing user configuration, which may help support portrait formats.
- Manual/active shuffling of cards to replace random selection from deck.
- Allow cards to be retrieved after they are Played and before committing them.
  - This was requested by players of the original game and should be fairly straightforward to implement, but is not part of MVP.
  - Need to consider handling of Active cards which are Played and then retrieved; logically these should be returned to Active rather than to the player's hand.
- User-defined key bindings.
- Test/optimise operation on touch screens.
- Extract text messages into supplementary files rather than having them baked into the code.
  - Once this is done, localisation may be possible.
- Add a mute button to the title screen.
- Saving an ongoing game.
- Saving user settings/creating profiles.
- When determining whether the player can afford to but new cards in Town, extend the logic to account for the Ace bonus.
  - This will only be relevant if the cost of purchase (or the size of the Ace bonus) is made customisable with relatively high values allowed; otherwise it is unlikely that having the Ace active for the boost will take the total high enough even though playing the Ace along with other diamonds would not.
  - The logic required to implement this will be much more convoluted than the vanilla implementation so it should not be added unless/until it is both relevant and necessary.
- Automated testing.
