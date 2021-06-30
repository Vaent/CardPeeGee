# Displaying messages via the Text utility class

## Background

The original game displayed prompts and informational messages using three 'event text' GameObjects. It was assumed these would stay in fixed positions relative to the scene, so a lot of messages ended up prefixed with multiple newlines (`\n`) to set the vertical position of text when the default for that TextMesh was unsuitable. Any extensions or modifications would require a lot of manual testing and would be limited by the need to keep everything consistent with the established model.

A decision was made to replace this setup with something more robust rather than reproducing it in the new C# code base.

## The new Text utility (Scripts/Util/Text.cs)

### Purpose

- Message content is abstracted away from events, improving maintainability and extensibility.
  - Eventually, messages will likely be loaded from files to decouple them from the code base and better support localisation.
- Management of GameObjects/components is abstracted away/masked from callers which are not interested in the mechanics of how a message is displayed.

### Structure

- A static `Text` class provides a common 'point of entry' for message display functionality.
- `Text` maintains a reference to a basic TextMesh prefab.
  - TextMesh instances are created from the prefab as required, and destroyed when the relevant context ends.
- `Text` defines a nested `Options` class which can be used to configure certain properties of the TextMesh.
- Other nested classes within `Text` provide specialised content grouped by event/purpose.
  - Initially these mirror the classes which will call on the relevant content; this convention is likely to persist.
- An abstract `BaseText` class defines the functionality of these specialised classes.
- Implementations of `BaseText` therefore only need to define message content and lightweight formatting details.
- `BaseText` and its implementations are structured as 'pseudo-singletons' with all interactions through static members (constructors are public to satisfy the generic definition only).

Example: the `PlayerCreator` utility displays progress updates as cards are dealt. The default text of each message is stored in `Text.PlayerCreator` and associated to a publicly visible reference. To display the `HPCalculated` message, `PlayerCreator` makes a call to `Text.PlayerCreator.Display()` passing in the reference and additional arguments for the format string.

### Planned improvements

Named references are defined in a enum but methods provided by the abstract base class take the reference as an `int` parameter, which requires casting the argument and allows arbitrary/unsafe values to be passed in. This aspect of the design needs to be reworked.

Callers must look up the message text in the relevant implementation and determine what arguments (if any) the format string expects. This is cumbersome and likely to be error-prone as bad calls will only be detected at runtime. Messages should be associated with details of expected objects for interpolation; at minimum the number of objects should be known and a description provided.

Introducing a custom `TextReference` class/struct to replace the enum could solve both of the above concerns and future design work is likely to consider how this can best be implemented.

## Exceptions to the use of Text classes

This utility is aimed at displaying temporary, static messages. Highly dynamic text (e.g. the player's HP display) and permanent labels (e.g. for the "Played cards" area) will still be managed separately.
