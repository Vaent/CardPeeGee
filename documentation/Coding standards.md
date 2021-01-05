# Code styling

Microsoft's C# conventions should be followed by default, unless they contradict the project standards below. See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions.

## Field declarations

- If static fields are required they may be placed at the top of the class declaration, separate from instance variables.
- Fields representing elements which are exposed in the Unity Editor should be declared with `public` accessibility, and assigned in Unity using the Inspector.
- Fields used to manage state should be declared `private`, with no accessors unless required for a particular purpose. Do not use a leading `_` in the field name.
- Where both kinds of field described above are used in a script, the declarations should be in separate blocks, preceded by the comments `// component references` and `// state variables` respectively. An empty line may be used to separate the blocks.
- Fields should be listed alphabetically by field name, one per line, with no empty lines between declarations. If some other arrangement is preferred within a particular class (for instance, certain fields are closely associated or have a unique natural ordering) use comments to clarify the reason for varying from the standard.

### Accessors

If required for any fields, these should be listed alphabetically in a block following the field declaration block(s), before any constructors or other methods. Consideration should be given to whether a custom method is more appropriate than an accessor; for example, `Card.Convert(int newValue)` might behave like a simple setter, but calling a `Convert` method makes the caller's intention clearer than directly setting a `convertedValue` property and reduces coupling to the `Card` class.

## Constructors

If required, these are listed after fields/accessors and before methods. Overloads are ordered as for methods (see below).

Do not include the default constructor unless it is needed alongside overloads.

## Method declarations

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

Any variation to the above must be justified/clarified in comments, like for field declarations.

## Inner classes/enums

These come at the end of the class definition, after all method declarations of the outer class.

Same rules as above apply to the contents of inner classes.

It's unlikely there will be so many inner classes/enums within a single outer class to require instructions as to how they must be ordered relative to each, but try to stick to the spirit of the above guidelines.

## Single-line conditionals

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
