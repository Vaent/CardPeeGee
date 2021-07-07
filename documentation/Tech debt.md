# Tech debt

## In scope

### Required

- Document visual layers and their priority levels, begin allocating GameObjects to the appropriate layers.
- Improve GameState locking/unlocking and use of Next()/Encounter.Advance() as that pattern is not so broadly applicable as it was initially assumed to be; q.v. traps on chests and paying healer fees.
- Rationalise debugs/reduce spam

### Desired

- Add dev tools to facilitate manual testing
  - Rapid startup/character creation
  - Specify what cards to deal (for new encounter, scoring etc)
- Improve referencing in the Text utility.
- Reduce the use of magic numbers e.g. number of cards in starting hand.
  - Ultimately most of these parameters should be redefinable to support extensions to the base game.

## Out of scope

- **Accessibility!!!** Should be in scope but since the original game was not built with access needs in mind, retrofitting features like text resizing, control over colour palettes etc will require disproportionate effort and probably require delivery of additional tech debt, like being able to resize/reconfigure the play area.
- Procedurally generate card sprites from name/number and suit. It should be possible to scrap the 52 individual sprites (and additional variants) which all have exactly the same layout, and instead have an abstract card face with spaces for the number/letter and the suit pip.
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
