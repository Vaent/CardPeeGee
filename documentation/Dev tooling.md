# Dev tooling

## Editor scripts

These may be used to facilitate manual testing in the Unity Editor.

Editor scripts should be placed in `Assets/Editor/Scripts` or a subfolder.

Production code should only be extended **AS A LAST RESORT** if necessary to support dev tools, in which case the extensions **MUST** be wrapped with an `#if` directive as shown below:

```
#if UNITY_EDITOR
    public void DevToolMethod() { }
#endif
```

This directive flags the extension code to be stripped from builds to ensure it won't interfere with production code.

### QuickStart tool

Bypasses the normal use of PlayerCreator to quickly launch a new game.

- Defined in `Assets/Editor/Scripts/ManualTestHelper.cs`
- Requires an `#if UNITY_EDITOR` extension to `GameState`
- Used via `DevTools > Quick Start` menu option or `Shift+Alt+P` shortcut

### Stacking the Deck

Allows flagging individual cards to ensure they are dealt next.

- Defined as an `#if UNITY_EDITOR` extension in `Deck.DrawCards`, supported by further extensions in `Card` and `CardController`
- Setting a card's tag as `StackDeck` in the Unity Editor will flag it for stacking
- Stacked cards will be dealt in order of the `stackedCardOrder` property which can be set on the CardController script, starting with the lowest value
- The deck will still deal the number of cards requested in the method call; random cards will be selected as normal to make up any shortfall
- All stacking details are cleared from cards when they are dealt
- If stacking details are added to a card which is not in the deck, they will have no effect. All cards returned to the deck from elsewhere in the play area are checked for stacking details and these are wiped if present
