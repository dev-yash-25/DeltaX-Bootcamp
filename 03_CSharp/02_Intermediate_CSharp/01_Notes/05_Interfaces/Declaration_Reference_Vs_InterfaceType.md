## Doubt
Can i say in both case first `Demo demo` is just a reference type var pointing to actual demo object, now demo can be treated as a normal field of Demo type datatype\
second `IDemo demo` is a reference type var like above but here + addition that this reference type var also has a defined boundry / contract top on?

**Yes**
### 1. `Demo demo`

```csharp
Demo demo = new Demo();
```

You can think:

> `demo` is a **reference variable** pointing to the actual `Demo` object.

Because its reference type is `Demo`, you can access the members available on `Demo`.

```text
demo
  │
  ↓
[ Demo object ]
```

So yes, `demo` is being treated as a **`Demo`-typed reference**.

---

### 2. `IDemo demo`

```csharp
IDemo demo = new Demo();
```

Again:

> `demo` is a **reference variable** pointing to the actual `Demo` object.

But now there is an additional layer:

```text
demo
  │
  ↓
[ Demo object ]
```

The reference is of type `IDemo`, so you are accessing that object **through the boundary/contract defined by `IDemo`**.

For example:

```csharp
public interface IDemo
{
    void Method();
}
```

Then:

```csharp
IDemo demo = new Demo();

demo.Method();              // ✅
demo.SomeDemoSpecificMethod(); // ❌
```

Even though the actual `Demo` object may have `SomeDemoSpecificMethod()`, your `IDemo` reference doesn't expose it.

So your idea can be phrased very naturally as:

> **Both are reference variables pointing to the actual `Demo` object. The difference is the type of the reference. With `Demo demo`, the reference exposes the `Demo` type's members. With `IDemo demo`, the reference exposes only what the `IDemo` contract guarantees.**

And your word **"boundary"** is actually a useful mental model:

```text
Demo demo
   ↓
┌─────────────────────┐
│ All accessible      │
│ Demo members        │
└─────────────────────┘
        ↓
   Demo object
```

versus:

```text
IDemo demo
   ↓
┌─────────────────────┐
│ IDemo boundary      │
│                     │
│ Method()       ✅   │
│ OtherDemoStuff ❌   │
└─────────────────────┘
        ↓
   Demo object
```

One subtle correction: **the interface doesn't physically restrict the object**. The `Demo` object still contains all its members. The `IDemo` reference simply gives you access to the **contractual view** of that object.
