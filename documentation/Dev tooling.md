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
- Requires an extension to `GameState`
- Used via `DevTools > Quick Start` menu option or `Shift+Alt+P` shortcut
