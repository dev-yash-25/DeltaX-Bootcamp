# Composition vs Inheritance (Why Composition Provides Loose Coupling)

<br>

---

| Approach | Coupling |
|---|---|
| **Inheritance** | **Strong/tight coupling** between child and parent |
| **Composition + concrete class** | **Tight coupling**, but not inheritance coupling |
| **Composition + interface + DI** | **Loose coupling** |

---

<br>


One of the most common OOP interview questions is:

> **Why is Composition preferred over Inheritance?**

The answer is:

> **Composition creates Loose Coupling, while Inheritance creates Tight Coupling.**

To understand this, let's compare both approaches using the **same example**.

<br>

# Problem Statement

We want to design a **Car**.

A car needs an **Engine**.

Question:

Should we use **Inheritance** or **Composition**?

<br>

# Approach 1 - Inheritance (Incorrect Design)

Suppose we think:

> "A car uses an engine, so Car should inherit Engine."

```csharp
class Engine
{
    public void Start()
    {
        Console.WriteLine("Engine Started");
    }
}

class Car : Engine
{
    public void Drive()
    {
        Console.WriteLine("Car Driving");
    }
}
```

Usage

```csharp
Car car = new Car();

car.Start();
car.Drive();
```

Looks correct.

But this design has several problems.

<br>

# Problem 1 - Wrong Relationship

Inheritance represents an **IS-A** relationship.

By writing

```csharp
class Car : Engine
```

we are saying

```
Car IS-A Engine
```

Ask yourself:

> Is a Car an Engine?

No.

A car **contains** an engine.

Correct relationship:

```
Car
  HAS-A
Engine
```

Whenever the sentence **IS-A** doesn't make sense,
inheritance is usually the wrong choice.

<br>

# Problem 2 - Tight Coupling

Suppose the Engine class changes.

Old version

```csharp
class Engine
{
    public void Start() { }
}
```

Now the company decides to replace it.

```csharp
class ElectricEngine
{
    public void PowerOn() { }
}
```

Since Car inherited Engine,

```
Engine Changes
       ↓
Car Breaks
```

Now Car must also be modified.

Car is tightly coupled to Engine.

<br>

# Problem 3 - Difficult to Support Multiple Engine Types

Imagine our application now supports

```
Petrol Engine

Diesel Engine

Electric Engine

Hybrid Engine
```

Which one should Car inherit?

```
Car : PetrolEngine ?

Car : DieselEngine ?

Car : ElectricEngine ?
```

Impossible.

A class can inherit only one base class.

Inheritance becomes very restrictive.

<br>

# Approach 2 - Composition (Correct Design)

Instead of saying

```
Car IS-A Engine
```

we say

```
Car HAS-A Engine
```

```csharp
class Engine
{
    public void Start()
    {
        Console.WriteLine("Engine Started");
    }
}

class Car
{
    private Engine _engine;

    public Car()
    {
        _engine = new Engine();
    }

    public void Drive()
    {
        _engine.Start();

        Console.WriteLine("Car Driving");
    }
}
```

Usage

```csharp
Car car = new Car();

car.Drive();
```

Relationship

```
Car
 |
 | HAS
 ▼
Engine
```

Notice:

Car no longer **is** an Engine.

It simply owns one.

<br>

# Is This Already Loose Coupling?

It is **better than inheritance**, but not fully loosely coupled.

Why?

Because Car still creates

```csharp
new Engine()
```

inside itself.

So Car still depends on the concrete Engine class.

<br>

# Best Composition - Using an Interface

Instead of depending on Engine,

depend on an abstraction.

## Step 1

Create an interface.

```csharp
interface IEngine
{
    void Start();
}
```

<br>

## Step 2

Create multiple implementations.

```csharp
class PetrolEngine : IEngine
{
    public void Start()
    {
        Console.WriteLine("Petrol Engine Started");
    }
}
```

```csharp
class ElectricEngine : IEngine
{
    public void Start()
    {
        Console.WriteLine("Electric Engine Started");
    }
}
```

<br>

## Step 3

Inject the engine into Car.

```csharp
class Car
{
    private IEngine _engine;

    public Car(IEngine engine)
    {
        _engine = engine;
    }

    public void Drive()
    {
        _engine.Start();

        Console.WriteLine("Driving...");
    }
}
```

Usage

```csharp
Car petrolCar = new Car(new PetrolEngine());

Car electricCar = new Car(new ElectricEngine());
```

Car class never changes.

Only the object passed changes.

<br>

# Visualization

```
                IEngine
                   ▲
        ┌──────────┴──────────┐
        │                     │
PetrolEngine          ElectricEngine
        ▲                     ▲
        │                     │
        └────── Car HAS ──────┘
```

Car only knows about **IEngine**.

It doesn't know or care whether it receives

- PetrolEngine
- DieselEngine
- ElectricEngine
- HybridEngine

<br>

# Why This Is Loose Coupling

Suppose tomorrow a new engine is introduced.

```csharp
class HydrogenEngine : IEngine
{
    public void Start()
    {
        Console.WriteLine("Hydrogen Engine Started");
    }
}
```

Usage

```csharp
Car car = new Car(new HydrogenEngine());
```

No changes to Car.

No recompilation of Car.

No new inheritance.

Everything continues to work.

<br>

# What Does "Coupling" Mean?

Coupling means

> **How dependent one class is on another.**

### Tight Coupling

```
Car
 |
 ▼
Engine
```

If Engine changes,

Car changes.

<br>

### Loose Coupling

```
Car
 |
 ▼
IEngine
 |
 ├── PetrolEngine
 ├── ElectricEngine
 ├── HybridEngine
 └── HydrogenEngine
```

Car depends only on the interface.

Implementations can change freely.

<br>

# Real-Life Example

Think about charging your phone.

Your phone doesn't care whether electricity comes from

- Wall socket
- Power bank
- Laptop
- Car charger

It only expects

```
USB-C Power
```

The charger changes.

The phone doesn't.

That is loose coupling.

<br>

# Another Example - Notification System

## Inheritance

```
Notification

↑

EmailNotification

↑

SMSNotification

↑

WhatsappNotification
```

Not flexible.

<br>

## Composition

```
Notification

      HAS

INotificationSender

      ▲
      │
 ┌────┼─────┐

EmailSender

SmsSender

WhatsappSender
```

Need Telegram tomorrow?

Just create

```csharp
class TelegramSender : INotificationSender
{
}
```

Nothing else changes.

<br>

# Advantages of Composition

## 1. Flexible

Objects can be replaced easily.

```
Petrol

↓

Electric

↓

Hybrid

↓

Hydrogen
```

Car remains unchanged.

<br>

## 2. Reusable

The same Engine can be shared by different classes.

```
Car

Truck

Bus

Generator
```

All can use the same Engine implementation.

<br>

## 3. Easier Testing

During testing

```csharp
class FakeEngine : IEngine
{
    public void Start()
    {
        Console.WriteLine("Fake Engine");
    }
}
```

```csharp
Car testCar = new Car(new FakeEngine());
```

Testing becomes very simple.

<br>

## 4. Better Maintenance

Changes stay isolated.

Changing Engine doesn't force changes to Car.

<br>

## 5. Follows SOLID Principles

Composition naturally supports

- Dependency Inversion Principle (DIP)
- Open/Closed Principle (OCP)

<br>

# Inheritance vs Composition

| Inheritance | Composition |
|-------------|-------------|
| IS-A relationship | HAS-A relationship |
| Tight Coupling | Loose Coupling |
| Derived class depends on Base class | Class depends on an abstraction/object |
| Hard to replace behavior | Easy to replace behavior |
| Less flexible | More flexible |
| Base class changes may affect child classes | Internal object can change independently |
| Single inheritance limitation | Can compose multiple objects |

<br>

# When to Use Inheritance

Use inheritance only when

```
Dog IS-A Animal

Cat IS-A Animal

Manager IS-A Employee
```

The IS-A relationship is true.

<br>

# When to Use Composition

Use composition when

```
Car HAS-A Engine

House HAS-A Door

Movie HAS-A Producer

Computer HAS-A Keyboard

Restaurant HAS-A Address
```

The HAS-A relationship is true.

<br>

# Interview Definition

> **Composition is an OOP principle where one class contains another class instead of inheriting from it. It creates loose coupling because the contained object can be replaced, modified, or extended without changing the consuming class.**

<br>

# Easy Memory Trick

```
Inheritance

IS-A

Dog → Animal

Car → Engine ❌



Composition

HAS-A

Car → Engine ✅

House → Door ✅

Movie → Producer ✅
```

Remember:

> **If the sentence reads naturally as "IS-A", prefer inheritance.**
>
> **If the sentence reads naturally as "HAS-A", prefer composition.**
