# Managing cards

Most GameObjects in a scene are created in the Unity editor and have an appropriate script attached to them.

In this regard, Cards are unusual: they are instantiated as virtual (C#) objects, as this makes it easier to programatically manage the individual properties of each card, namely its suit, value and related Sprite, based on an argument to the constructor. The virtual Card creates and manages a GameObject from the Abstract Card prefab, with the GameObject having no knowledge of the `Card` script. This approach allows Cards to be handled in a more generic fashion than the original game.

*N.B. The initial Card implementation relies on a specific Sprite file naming convention: `<Suit> <value> <optional annotations>` e.g. `Heart 10 Queen` from which the constructor can assign relevant properties.*

Additionally, different areas of the game are being decoupled from the overarching game logic in an effort to move away from the monolithic `Deal.js` script which managed most aspects of the original game. The expectation going forward is that the deck, activated cards, cards in the player's hand and so on will each be represented as a `CardZone` having sole responsibility for managing any Cards which are sent to it. The higher-level flow of the game is thereby largely reduced to determining which Cards should be sent where, based on current state and player decisions.

As of 17/12/2020, Cards will have knowledge of the CardZone which is currently responsible for them. This is to prevent Cards being lost if a CardZone removes them from its own list of dependents without correctly handing them over to the next CardZone. Default behaviour will also be added to the base CardZone definition to ensure robust and consistent transference of Cards, while still allowing different CardZones to do their own thing with the Cards once they arrive.

Addendum 04/01/2021: CardZones will by default notify the GameState whenever they Accept(cards) **and have finished moving the cards**, assuming any movement takes place. This is to keep the CardZone logic focused on registering and moving cards, and have GameState as an observer which doesn't need to monitor that process but has an interest in the eventual outcome.
