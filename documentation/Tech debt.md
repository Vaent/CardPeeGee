# Tech debt

## In scope

### Required

*All caught up*

### Desired

- Procedurally generate card sprites from name/number and suit. It should be possible to scrap the 52 individual sprites (and additional variants) which all have exactly the same layout, and instead have an abstract card face with spaces for the number/letter and the suit pip.
- Improve text display system (e.g. messages showing progress of player creation).
  - Add a utility to manage text placement (get rid of `"\n\n\n\n..."`).
  - See if we can find a better way to update text than directly setting a TextMesh.text value i.e. abstract the TextMesh away from the class which wants to display a message but doesn't really care how it's done.
  - Proposal: introduce a separate text display manager which can potentially create and drop TextMesh instances as required rather than relying on preset objects.
- Reduce the use of magic numbers e.g. number of cards in starting hand.
  - Ultimately most of these parameters should be redefinable to support extensions to the base game.

## Out of scope

- **Accessibility!!!** Should be in scope but since the original game was not built with access needs in mind, retrofitting features like text resizing, control over colour palettes etc will require disproportionate effort and probably require delivery of additional tech debt, like being able to resize/reconfigure the play area.
- Replace fixed dimension 700x500 play area with a more flexible frame.
  - May still be constrained to landscape format but should at least accommodate different screen resolutions (SD/HD/FHD etc) and ideally different aspect ratios (between 4:3 and 16:9, and ideally beyond).
  - A nice-to-have extension would be flexible placement of game elements allowing user configuration, which may help support portrait formats.
- Manual/active shuffling of cards to replace random selection from deck.
- User-defined key bindings.
- Test/optimise operation on touch screens.
- Extract text messages into supplementary files rather than having them baked into the code.
  - Once this is done, localisation may be possible.
- Add a mute button to the title screen.
- Saving an ongoing game.
- Saving user settings/creating profiles.
