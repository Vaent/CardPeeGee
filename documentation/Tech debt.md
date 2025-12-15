# Tech debt

## Bugs

- (observed once while running WebGL build 0.0.1 in Chrome) During cleanup of a Trap encounter with 4 score cards: <King of Spades | 02 of Diamonds | 06 of Clubs | 04 of Spades> the King of Spades stayed in place while the other score cards and encounter cards were correctly returned to the Deck. This occurred while spamming the D key to check for any issues caused by failing to handle input correctly - may or may not be related. Debug messages in the DevTools console indicate that the 'virtual' card was returned to the Deck as expected but movement was not recorded (all other cards reported completion of their movement).
  - A similar issue was observed when serving WebGL build 0.0.1 through a local PHP server and browsing in Chrome. While dealing for a new character card at the start of a game, the cards dealt were <10 of Hearts | 07 of Diamonds | Ace of Diamonds | 06 of Clubs | 09 of Spades | 04 of Hearts | 04 of Spades | Jack of Diamonds | Queen of Clubs> of which all behaved normally except the Ace of Diamonds which became stuck in place when it reached the StagingArea, not moving when the cards started squashing together due to exceeding the total permitted width, and not returning to the Deck afterwards, although the virtual card was registered as returned. On this occasion there was no deliberate interference/additional input.
- When the game was loaded through cardpeegee.vaent.uk on an Android mobile device, a warning was displayed indicating that WebGL builds are not supported on mobile devices (this is in line with the generated HTML) but the game launched and ran normally when input was received, with the exception that both sides of the play area were cropped. It's not clear if this is due to the layout of the web page, where the game is loaded in an HTML iframe, or something in the generated (Unity) HTML. Mobile support is a planned extension, not within the initial scope, but this is being bugged now as it could be an implementation/build issue and in any case has potential to cause confusion.
- Rendering of the "LeaveButton" is inconsistent. In the Unity Editor, and fullscreen mode of WebGL build, it has the expected appearance, but the WebGL build displayed in normal windowed browser (Chrome) has the button shifted upwards, slightly overlapping other displayed text. Note: removing the Unity footer from the HTML had no effect on this visual glitch.

## In scope

### Required

- Refine hint text for played cards.
  - When paying a healer's fee, overpayment results in a negative fee being shown; it should have a floor of zero.
  - While shopping in town, the amount paid and the number of cards to be received should be displayed.
  - While healing in town, the amount of healing to be delivered from played cards should be displayed.
- Enable suspending code execution while the player is being healed/damaged - ideally without having to calculate the expected delay and apply it manually every time healing/damage methods are invoked.
- Implement player death properly (has a dependency on the above point). This should be picked up as part of the general build process but a reminder is added here since a workaround is currently in place and this is a core mechanic which **must not** be overlooked!
- Document visual layers and their priority levels, begin allocating GameObjects to the appropriate layers.
- Improve GameState locking/unlocking and use of Next()/Encounter.Advance() as that pattern is not so broadly applicable as it was initially assumed to be; q.v. traps on chests and paying healer fees.
- Uncouple Battle encounters from Healer battles so that they only share relevant logic/state.
- Rationalise debugs/reduce spam.
- Review all uses of `Timer.Delay(...)` and `Timer.DelayThenInvoke(...)` to ensure the pace of automatic actions is appropriate - some messages are appearing/disappearing much too quickly.

### Desired

- Apply namespaces to scripts in subfolders of the root directory `Assets/Scripts/` (this has already been done for `Text` and `Text.Excerpts`)
- Reduce the use of magic numbers e.g. number of cards in starting hand.
  - Ultimately most of these parameters should be redefinable to support extensions to the base game.
- Review usage of `static readonly` fields, replace with auto-implemented properties if the value may be made customisable in future.
- Amend debug/print functions to avoid line breaks in strings which contain newline characters (particularly problematic for TextManager debugs).
  - Consider replacing Text.Excerpt content with the excerpt name (and args?) in debugs.
  - It may be sufficient to have Text.Excerpt implement IFormattable with a compact/debug format option, and invoke this where appropriate, since Excerpts are the main culprit.
- Rework TextManager to recycle Excerpts instead of repeatedly destroying and instantiating new.

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
- When determining whether the player can afford to buy new cards in Town, extend the logic to account for the Ace bonus.
  - This will only be relevant if the cost of purchase (or the size of the Ace bonus) is made customisable with relatively high values allowed; otherwise it is unlikely that having the Ace active for the boost will take the total high enough even though playing the Ace along with other diamonds would not.
  - The logic required to implement this will be much more convoluted than the vanilla implementation so it should not be added unless/until it is both relevant and necessary.
- Automated testing.
