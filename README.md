# CardPeeGee

Rebuilding the 2015 game in a current version of Unity.

The initial goal is to export the game in WebGL format, to replace the obsolete Unity Web Player build.

## Code styling

Microsoft's C# conventions should be followed by default, unless they contradict the project standards below. See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions.

### Field declarations

- Fields representing elements which are exposed in the Unity Editor should be declared with `public` accessibility, and assigned in Unity using the Inspector.
- Fields used to manage state should be declared `private`, with no accessors unless required for a particular purpose. Do not use a leading `_` in the field name.
- Where both kinds of field described above are used in a script, the declarations should be in separate blocks, preceded by the comments `// component references` and `// state variables` respectively. An empty line may be used to separate the blocks.
- Fields should be listed alphabetically by field name, one per line, with no empty lines between declarations. If some other arrangement is preferred within a particular class (for instance, certain fields are closely associated or have a unique natural ordering) use comments to clarify the reason for varying from the standard.

### Method declarations

These should come after all field declarations in a class.

If any of Unity's base methods are used in the class, these should be declared first, in the following order:

- Start()
- Update()

(this list should be extended as and when there is a need to use more of the base methods)

Any additional methods should be declared in alphabetical order by method name. For overloaded methods, ordering is determined by alphabetic sorting of the parameter types:

```
public void MyMethod(bool param1, int param2) {}  // bool,int
private String MyMethod(CustomType aParam) {}     // CustomType
public void MyMethod(int param1, bool param2) {}  // int,bool
```

As for field declarations, any variation to the above must be justified/clarified in comments.

### Single-line conditionals

May be used instead of a code block for simple `if` statements. Complex statements should not be forced onto a single line where expanding them in a code block would improve readability.

```
// this is okay
if (myInt < 10) myInt += 5;

// this is also fine
if (myInt < 10)
{
    myInt += 5;
}

// this should be avoided
if ((myInt < 10) || (myBool == false)) myObject.DoAction(new MyClass(myInt)).compute();
```

Where the condition has alternatives (`if ... else`) code blocks must be used for every branch.
