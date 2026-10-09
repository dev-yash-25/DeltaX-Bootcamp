# Static vs Instance Members in C# — Understanding Through Memory

<br>

---
> [!Important]
> Avoid memorizing:
>
> - "Static belongs to the class."
> - "Instance belongs to the object."
>
> Instead, ask yourself:
>
> **What actually exists in memory?**
>
> Once you understand the memory, the rules become obvious.

> [!Note]
> You can access **static method, or member** using a object, but it throws warning, Prefer class itself to access them.
---

<br>


# Example Class

```csharp
class A
{
    int x = 10;              // Instance (Non-static) field

    static void Show()
    {
        // Console.WriteLine(x);   ❌ Compile-time error
    }
}
```
```csharp
class A
{
    int x = 10;

    static void Show()
    {
        A obj = new A();

        Console.WriteLine(obj.x);  // 10
    }
}
```
<br>

# Step 1 — What Exists Before Any Object Is Created?

When **no object exists**, only the class metadata and static members are loaded.

```text
                 Program Memory

+---------------------------------------+
| Class A                               |
|---------------------------------------|
| Static Methods                        |
|   Show()                              |
|                                       |
| Static Fields (if any)                |
|   (none)                              |
+---------------------------------------+

Heap:
(empty)
```

## Notice

Where is `x`?

It doesn't exist anywhere.

Why?

Because `x` is an **instance field**.

Instance fields are created **only when an object is created**.

<br>

# Step 2 — Create an Object

```csharp
A obj = new A();
```

Memory now becomes:

```text
                 Program Memory

+---------------------------------------+
| Class A                               |
|---------------------------------------|
| Static Methods                        |
|   Show()                              |
+---------------------------------------+

Heap

obj
 |
 v
+----------------+
| Object of A    |
|----------------|
| x = 10         |
+----------------+
```

Now `x` exists.

Why?

Because an object now exists.

The object owns its own copy of `x`.

<br>

# Step 3 — Create Another Object

```csharp
A obj2 = new A();
```

Memory becomes:

```text
Heap

obj ------+
           |
           v
      +----------+
      | x = 10   |
      +----------+

obj2 -----+
           |
           v
      +----------+
      | x = 10   |
      +----------+
```

Notice carefully.

There are now:

```text
Two different x variables.
```

Each object owns its own copy.

Changing one does not affect the other.

<br>

# Where Is Show()?

Even if we create many objects:

```csharp
A a1 = new A();
A a2 = new A();
A a3 = new A();
```

Memory still contains only one `Show()`.

```text
Program Memory

Class A
-----------------------
Show()
```

There are **not** three copies of `Show()`.

Only one exists.

Every object shares that same method.

<br>

# Static vs Instance — The Real Meaning

People often say:

> Static members belong to the class.

and

> Instance members belong to objects.

This is true, but what does it actually mean?

Let's visualize it.

<br>

# Example

```csharp
class Student
{
    public static string College = "MIT";

    public string Name;
}
```

Both members are declared inside the class.

```text
Student
│
├── College
└── Name
```

So why do we treat them differently?

Because they are stored differently.

<br>

# Before Creating Any Object

```text
Program Memory

+--------------------------------------+
| Class Student                        |
|--------------------------------------|
| Static Field                         |
|   College = "MIT"                    |
|                                      |
| Instance Field Definition            |
|   Name                               |
+--------------------------------------+

Heap
(empty)
```

Notice something.

`College` already exists.

But where is `Name`?

It doesn't exist yet.

Because there is no object.

<br>

# Create One Object

```csharp
Student s1 = new Student();
```

Memory becomes:

```text
Program Memory

+--------------------------------------+
| Student Class                        |
|--------------------------------------|
| College = "MIT"                      |
+--------------------------------------+

Heap

s1
 |
 v
+----------------+
| Name = null    |
+----------------+
```

Now `Name` exists.

<br>

# Create Another Object

```csharp
Student s2 = new Student();
```

Assign values.

```csharp
s1.Name = "Yash";
s2.Name = "John";
```

Memory becomes:

```text
Program Memory

Student Class

College = "MIT"

Heap

s1
 |
 v
+----------------+
| Name = Yash    |
+----------------+

s2
 |
 v
+----------------+
| Name = John    |
+----------------+
```

Notice something important.

There are now

```text
Two Name variables.
```

But still only

```text
One College variable.
```

<br>

# Why Can We Write

```csharp
Student.College
```

Because memory contains exactly one

```text
College = "MIT"
```

The compiler knows exactly which one you mean.

<br>

# Why Can't We Write

```csharp
Student.Name
```

Imagine:

```csharp
Student s1 = new Student();
Student s2 = new Student();

s1.Name = "Yash";
s2.Name = "John";
```

Memory:

```text
s1 ----> Name = "Yash"

s2 ----> Name = "John"
```

Now suppose you write

```csharp
Student.Name
```

The compiler asks:

> Which object's `Name`?

```text
Yash ?

John ?
```

There is no single answer.

That's why you must specify the object.

```csharp
s1.Name

or

s2.Name
```

<br>

# Why Can't a Static Method Access an Instance Field?

Suppose you write:

```csharp
class A
{
    int x = 10;

    static void Show()
    {
        Console.WriteLine(x);
    }
}
```

Now imagine:

```csharp
A a1 = new A();
A a2 = new A();

a1.x = 5;
a2.x = 100;
```

Memory:

```text
a1 ----> x = 5

a2 ----> x = 100

Program Memory

Class A

Show()
```

Inside `Show()` you wrote

```csharp
Console.WriteLine(x);
```

The compiler asks:

> Which object's `x`?

```text
5 ?

100 ?
```

The static method has **no object reference**.

It doesn't know about `a1`.

It doesn't know about `a2`.

Therefore the compiler reports an error.

<br>

# How Can a Static Method Access an Instance Field?

Pass the object.

```csharp
class A
{
    public int x = 10;

    public static void Show(A obj)
    {
        Console.WriteLine(obj.x);
    }
}
```

Usage:

```csharp
A a = new A();

A.Show(a);
```

Memory:

```text
Program Memory

Class A

Show(A obj)

Heap

a
 |
 v
+----------------+
| x = 10         |
+----------------+

Show(a)
   |
   +-------> Reads a.x
```

Now the method knows exactly which object's `x` to read.

<br>

# Why Can an Instance Method Access `x`?

Example:

```csharp
class A
{
    int x = 10;

    void Display()
    {
        Console.WriteLine(x);
    }
}
```

Usage:

```csharp
A obj = new A();

obj.Display();
```

Conceptually, the runtime treats it like:

```text
Display(obj)
```

There is a hidden reference called

```text
this
```

So inside the method,

```csharp
Console.WriteLine(x);
```

is effectively

```csharp
Console.WriteLine(this.x);
```

Since

```text
this = obj
```

the runtime knows exactly which object's `x` to read.

<br>

# Static Methods Have No `this`

Instance method

```text
obj.Display()

↓

Display(this = obj)
```

Static method

```text
A.Show()

↓

Show()

No this

No object
```

This is the real reason static methods cannot directly access instance members.

<br>

# Can We Call a Static Method Using an Object?

Yes.

```csharp
class A
{
    public static void Show()
    {
        Console.WriteLine("Hello");
    }
}

A obj = new A();

obj.Show();      // ⚠ Allowed, but not recommended
```

The compiler issues a warning:

> CS0176: Member 'A.Show()' cannot be accessed with an instance reference; qualify it with a type name instead.

The preferred way is:

```csharp
A.Show();
```

<br>

# Does `obj.Show()` Use the Object?

No.

Memory:

```text
Program Memory

Class A

Show()

Heap

obj
 |
 v
+----------------+
| x = 10         |
+----------------+
```

When you write

```csharp
obj.Show();
```

Conceptually, the compiler treats it as

```csharp
A.Show();
```

The object is completely ignored.

Unlike an instance method,

```text
obj.Display()

↓

Display(this = obj)
```

the object is **not passed** to a static method.

<br>

# Visual Comparison

## Instance Method

```text
obj.Display()

        Hidden

Display(this = obj)

        |

Reads obj.x
```

<br>

## Static Method

```text
obj.Show()

Compiler

↓

A.Show()

No object passed

No this
```

<br>

# Final Mental Picture

```text
Instance Field

Class defines it

↓

Object is created

↓

Field is created inside the object

↓

Every object gets its own copy
```

```text
Static Field / Static Method

Class defines it

↓

Created once with the class

↓

Shared by every object

↓

No object required
```

<br>

# Key Takeaways

- Every member is **declared inside a class**.
- Instance fields **do not exist until an object is created**.
- Every object gets its **own copy** of instance fields.
- Static fields and static methods have **only one shared copy**.
- Static methods have **no hidden `this` reference**.
- Therefore, a static method cannot directly access instance members.
- To access an instance member from a static method, you must **provide an object**.
- Calling `obj.Show()` does **not** pass the object; it is treated conceptually as `A.Show()` and the object is ignored.
