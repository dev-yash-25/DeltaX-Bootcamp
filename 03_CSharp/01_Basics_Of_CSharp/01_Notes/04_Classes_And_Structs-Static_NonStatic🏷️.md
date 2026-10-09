
# 4. Classes and Structures

<br>

[Static vs Non Static](https://github.com/Yash-Bandal/DeltaX-Bootcamp/blob/main/03_CSharp/01_Basics_Of_CSharp/01_Notes/Static_Class.md) 🏷️

<br>

## 4.1 Classes

### What is a Class?

A class is a blueprint for creating objects.

Think of a class as a template.

Example:

```text
Blueprint → House
Class     → Object
```

A class defines:

* Data (Properties/Fields)
* Behavior (Methods)

<br>

### Creating a Class

```csharp
class Person
{
    public string Name;
    public int Age;
}
```

Here, `Person` is a class.

<br>

### Creating an Object

```csharp
Person person1 = new Person();

person1.Name = "Yash";
person1.Age = 21;
```

Access values:

```csharp
Console.WriteLine(person1.Name);
Console.WriteLine(person1.Age);
```

Output:

```text
Yash
21
```

<br>

### Class with Methods

```csharp
class Person
{
    public string Name;

    public void Introduce()
    {
        Console.WriteLine("Hello, I am " + Name);
    }
}
```

Usage:

```csharp
Person person = new Person();

person.Name = "Yash";

person.Introduce();
```

Output:

```text
Hello, I am Yash
```

<br>

### Real-World Example

```text
Class: Car

Properties:
- Brand
- Color
- Speed

Methods:
- Start()
- Stop()
- Accelerate()
```

Objects:

```text
BMW Car
Audi Car
Tesla Car
```

All are created from the same blueprint.

<br>

### Why Use Classes?

Benefits:

* Code organization
* Reusability
* Easier maintenance
* Object-Oriented Programming (OOP)

Most enterprise applications are built using classes.

<br>

### Class Example

```csharp
class Employee
{
    public string Name;
    public double Salary;
}
```

```csharp
Employee emp = new Employee();

emp.Name = "John";
emp.Salary = 50000;
```


## What are Objects?

An object is an instance of a class.

Example:

```csharp
class Person
{
    public string Name;
}
```

Creating objects:

```csharp
Person person1 = new Person();
Person person2 = new Person();
```

Here:

```text
Class  -> Person

Objects ->
person1
person2
```

Think of a class as a blueprint and objects as actual products created from that blueprint.

<br>

## Fields and Methods

A class typically contains:

### Fields

Used to store data.

```csharp
class Person
{
    public string Name;
    public int Age;
}
```

Fields:

```text
Name
Age
```

<br>

### Methods

Used to define behavior.

```csharp
class Person
{
    public string Name;

    public void Introduce()
    {
        Console.WriteLine("Hi, I am " + Name);
    }
}
```

Method:

```text
Introduce()
```

<br>

## Accessing Members using Dot Notation

Use the dot operator (`.`) to access fields and methods.

```csharp
Person person = new Person();

person.Name = "Yash";

person.Introduce();
```

Examples:

```csharp
person.Name
person.Age
person.Introduce()
```

Dot notation is used everywhere in C#.

<br>

## Memory Allocation using new

Objects are created using the `new` keyword.

```csharp
Person person = new Person();
```

The `new` keyword:

* Creates the object
* Allocates memory
* Returns a reference to that object

Without `new`, no object is created.

<br>

## Garbage Collection

In C++, developers often free memory manually.

In C#, the CLR automatically removes unused objects using Garbage Collection.

Example:

```csharp
Person person = new Person();

person = null;
```

When the object is no longer used, the Garbage Collector eventually removes it from memory.

Benefit:

* Fewer memory leaks
* Easier memory management
* Less developer effort

<br>

---



<br>



# Static Members

| Static                                    | Non-Static                                    |
| ----------------------------------------- | --------------------------------------------- |
| Belongs to the **class**                  | Belongs to an **object (instance)**           |
| Only **one copy** exists in memory        | Every object gets its **own copy**            |
| Accessed using the **class name**         | Accessed using an **object**                  |
| Created when the class is loaded          | Created when the object is created            |
| Cannot access non-static members directly | Can access both static and non-static members |
| No object creation required               | Requires object creation                      |

<br>


> [!Note]
> ### Why Do We Need `Static`?
> We need it when we want a concept, that only has 1 single instance(object) in memory, *i.e* only 1 copy\
> Eg: `static void Main(string[] args)
>
> Here, we need only 1 instance of main function

## What is static?

Normally, fields and methods belong to objects.

A static member belongs to the class itself.

<br>

### Non-Static Example

```csharp
class Person
{
    public string Name;
}
```

Usage:

```csharp
Person p = new Person();

p.Name = "Yash";
```

An object is required.

<br>

### Static Field Example

```csharp
class Company
{
    public static string CompanyName = "Microsoft";
}
```

Usage:

```csharp
Console.WriteLine(Company.CompanyName);
```

Notice:

```csharp
Company.CompanyName
```

No object is created.

<br>



### Static Method Example

```csharp
class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}
```

Usage:

```csharp
int result = Calculator.Add(10, 20);

Console.WriteLine(result);
```

Output:

```text
30
```


Mental Model

```
Class
│
├── Static Members
│   └── One copy shared by everyone
│
└── Instance Members
    └── Each object gets its own copy
```

<br>



## Why Use static?

Because only one copy is needed.

Example:

```text
Current Date
Company Name
Utility Functions
Configuration Values
```

Creating multiple copies would waste memory.


<br>



## Real Example: DateTime

You have already used a static member without realizing it.

```csharp
DateTime.Now
```

Here:

```text
DateTime -> Class
Now      -> Static Property
```

No object creation is required.


<br>



## Main Method is Static

```csharp
static void Main()
{
}
```

The application starts before any object exists.

Therefore Main() must belong to the class itself, not an object.


<br>



## Rule to Remember

### Non-Static Members

Access through an object:

```csharp
person.Name
person.Introduce()
```

### Static Members

Access through the class:

```csharp
DateTime.Now

Math.Abs(-10)

Calculator.Add(1, 2)
```


<br>

---


<br>


## 4.2 Structs

### What is a Struct?

A struct is similar to a class.

It can contain:

* Fields
* Properties
* Methods
* Constructors

Example:

```csharp
struct Point
{
    public int X;
    public int Y;
}
```

Usage:

```csharp
Point point;

point.X = 10;
point.Y = 20;

Console.WriteLine(point.X);
```

Output:

```text
10
```

<br>

### When to Use Structs?

Use structs for small data objects.

Examples:

```text
Point
Coordinate
Color
Rectangle
Date
```

<br>

### Class vs Struct

| Class                    | Struct                           |
| ------------------------ | -------------------------------- |
| Reference Type           | Value Type                       |
| Stored on Heap           | Usually stored on Stack          |
| Can be Null              | Cannot be Null (unless nullable) |
| Better for large objects | Better for small objects         |
| More commonly used       | Less commonly used               |

<br>

### Example

#### Class

```csharp
class Person
{
    public string Name;
}
```

#### Struct

```csharp
struct Point
{
    public int X;
    public int Y;
}
```

<br>

### Industry Rule

Most of the time use:

```csharp
class
```

Use:

```csharp
struct
```

Only when:

* Object is small
* Represents a single value
* Does not require inheritance
* Performance is important

<br>

### Common Struct Examples in .NET

```csharp
DateTime
TimeSpan
Guid
Decimal
```

These are all structs.

<br>

## Quick Comparison

### Class

* Blueprint for objects
* Reference Type
* Most commonly used
* Supports inheritance
* Ideal for business objects

Examples:

```text
Employee
Customer
Product
Order
Student
```

<br>

### Struct

* Small lightweight object
* Value Type
* Faster for small data
* No inheritance
* Used less frequently

Examples:

```text
Point
DateTime
Color
Coordinates
```

<br>

## Key Takeaways

* A class is a blueprint used to create objects.
* Objects contain data and behavior.
* Classes are Reference Types.
* Structs are Value Types.
* Classes are used in most real-world applications.
* Structs are best for small, lightweight data structures.
* If unsure, use a class.


<br>

---
---

 
<br>


