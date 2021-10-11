# Displaying messages via the Text utility class

## Background

The original game displayed prompts and informational messages using three 'event text' GameObjects. It was assumed these would stay in fixed positions relative to the scene, so a lot of messages ended up prefixed with multiple newlines (`\n`) to set the vertical position of text when the default for that TextMesh was unsuitable. Any extensions or modifications would require a lot of manual testing and would be limited by the need to keep everything consistent with the established model.

A decision was made to replace this setup with something more robust rather than reproducing it in the new C# code base.

## The Text utility (Scripts/Util/Text.cs)

*As of 06/10/2021 this has been replaced by the TextManager pattern*

## TextManager and Excerpts (`Scripts/Text/` directory structure)

### Purpose

- Message content is abstracted away from events, improving maintainability and extensibility.
  - Eventually, messages will likely be loaded from files to decouple them from the code base and better support localisation.
- Messages with dynamic content clearly show what inputs they expect
- Management of GameObjects/components is abstracted away/masked from callers which are not interested in the mechanics of how a message is displayed.

### Structure

The basic 'unit of text' is now defined by `Text.Excerpt` and its generic variants, all of which extend the abstract `Text.BaseExcerpt`. Excerpts always have a message string and a Vector2 position indicating where they should be displayed; they may also be set up with formatting options; and generic versions always have exactly one instance of each type argument, which will be inserted into the message using `string.Format(...)`.

Excerpts for specific messages are set up and exposed by classes in the `Text.Excerpts` directory/namespace. A simple Excerpt is created for each message as a property in the relevant class, and if it has no dynamic content the property will be public; or if it is a format string it will be private and delivered via a public method which brings in the appropriate arguments.

```
namespace Text.Excerpts
{
    public static class Example
    {
        public static Excerpt Alert { get; } = new Excerpt("This is used unmodified", Text.Position.examplePosition);
        private static Excerpt Calculation_ { get; } = new Excerpt("{0} + 42 = {1}", new Vector2(5.1f, 6.3f));

        public static Excerpt<int, int> Calculation(int input, int result)
        {
            return new Excerpt<int, int>(Calculation_, input, result);
        }
    }
}
```

Points of note from the above example:

- The private property is suffixed with an underscore `_` to avoid naming clashes while maintaining a clear link to the public method.
- The properties, methods and arguments can be named in a way that makes their purpose clear to callers.
- `Text.Position` is a static class referencing some commonly-used positions.

A static class `Text.TextManager` provides a common 'point of entry' for message display functionality. `Text.TextManager` maintains a reference to a basic TextMesh prefab, from which instances are created as required, to be destroyed when the relevant context ends.

Instances of the `Text.Options` class can be used to configure certain properties of the TextMesh. This may be set up in a `Text.Excerpts` class as a default configuration for that Excerpt, and/or by invoking the appropriate overload of `TextManager.DisplayText(...)`.

These changes from the previous Text utility increase the effort involved in creating or amending text content - this cost is considered acceptable for the significant improvements in readability and reduced risk of defects relating to improperly formatted strings.

### Potential improvements

Code which needs to hide an Excerpt some time after displaying it, or display one Excerpt as an extension of another, needs to maintain a reference to the Excerpt and pass it to the TextManager. This is cumbersome and therefore undesirable; however, so far no solution has been found that doesn't introduce even worse side effects.

Excerpt strings may at some point be abstracted away from `Text.Excerpts` classes and maintained in a separate data structure; this is a low priority as in-game text is currently managed in parallel with code.

## Exceptions to the use of TextManager

This utility is aimed at displaying temporary, static messages. Highly dynamic text (e.g. the player's HP display) and permanent labels (e.g. for the "Played cards" area) will still be managed separately.
