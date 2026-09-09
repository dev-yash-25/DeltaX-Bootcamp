# C# Basics

## Index

1. [Introduction](#1-introduction)
   * 1.1 [What is C#?](#11-what-is-c)
   * 1.2 [Why Was C# Created?](#12-why-was-c-created)
   * 1.3 [Real-World Analogy](#13-real-world-analogy)
   * 1.4 [Where is C# Used?](#14-where-is-c-used)
   * 1.5 [Features of C#](#15-features-of-c)
   * 1.6 [Why Companies Use C#](#16-why-companies-use-c)
   * 1.7 [C# in the Real Industry](#17-c-in-the-real-industry)
   * 1.8 [Key Takeaways](#18-key-takeaways)
2. [C# and .NET](#2-c-and-net)
   * 2.1 [What is C#?](#21-what-is-c)
   * 2.2 [What is .NET?](#22-what-is-net)
   * 2.3 [Simple Analogy](#23-simple-analogy)
   * 2.4 [Relationship Between C# and .NET](#24-relationship-between-c-and-net)
   * 2.5 [Why Do We Need .NET?](#25-why-do-we-need-net)
   * 2.6 [.NET Libraries](#26-net-libraries)
   * 2.7 [Modern .NET](#27-modern-net)
   * 2.8 [Popular Technologies Built on .NET](#28-popular-technologies-built-on-net)
   * 2.9 [Real Industry Example](#29-real-industry-example)
   * 2.10 [Interview Quick Revision](#210-interview-quick-revision)
   * 2.11 [Key Takeaways](#211-key-takeaways)
3. [Common Language Runtime (CLR)](#3-common-language-runtime-clr)
   * 3.1 [What is CLR?](#31-what-is-clr)
   * 3.2 [Why Do We Need CLR?](#32-why-do-we-need-clr)
   * 3.3 [How a C# Program Runs](#33-how-a-c-program-runs)
   * 3.4 [Intermediate Language (IL)](#34-intermediate-language-il)
   * 3.5 [What is JIT Compiler?](#35-what-is-jit-compiler)
   * 3.6 [Memory Management](#36-memory-management)
   * 3.7 [Garbage Collection (GC)](#37-garbage-collection-gc)
   * 3.8 [Exception Handling](#38-exception-handling)
   * 3.9 [Security](#39-security)
   * 3.10 [Benefits of CLR](#310-benefits-of-clr)
   * 3.11 [Interview Quick Revision](#311-interview-quick-revision)
4. [.NET Architecture](#4-net-architecture)
   * 4.1 [What is .NET Architecture?](#41-what-is-net-architecture)
   * 4.2 [Big Picture (Full Execution Flow)](#42-big-picture-full-execution-flow)
   * 4.3 [Main Components of .NET Architecture](#43-main-components-of-net-architecture)
   * 4.4 [.NET Class Library](#44-net-class-library)
   * 4.5 [Operating System](#45-operating-system)
5. [First Application](#5-first-application)
   * 5.1 [Introduction](#51-introduction)
   * 5.2 [Creating the First C# Program](#52-creating-the-first-c-program)
   * 5.3 [Complete Program Structure](#53-complete-program-structure)
   * 5.4 [Understanding Each Part](#54-understanding-each-part)
   * 5.5 [Program Execution Flow](#55-program-execution-flow)
   * 5.6 [Why the Main Method is Important](#56-why-the-main-method-is-important)

<br>

---

<br>

# 1. Introduction

## 1.1 What is C#?

C# (pronounced **C Sharp**) is a modern, object-oriented programming language developed by Microsoft.

It is mainly used to build:

* Web Applications
* Desktop Applications
* APIs and Backend Services
* Cloud Applications
* Mobile Applications
* Games (using Unity)

Today, C# is one of the most widely used programming languages in enterprise software development.

<br>

## 1.2 Why Was C# Created?

Microsoft created C# to provide:

* Simplicity of Java
* Performance of C++
* Better safety and productivity

The goal was to create a language that is:

* Easy to learn
* Easy to maintain
* Powerful for large applications

<br>

## 1.3 Real-World Analogy

Think of programming as constructing a building.

| Real World                          | Programming                 |
| ------------------------------------ | ---------------------------- |
| Architect creates blueprint          | Developer writes code        |
| Construction workers build building  | Compiler builds application  |
| Building                             | Software Application         |

C# is the language used to write the blueprint.

<br>

## 1.4 Where is C# Used?

### Web Development

Used to build websites and web APIs using ASP.NET Core.

Examples:

* E-commerce websites
* Banking systems
* Hospital management systems
* Learning platforms

<br>

### Desktop Applications

Used for Windows applications.

Examples:

* Accounting software
* Management systems
* Internal company tools

<br>

### Game Development

One of the most popular uses of C#.

Unity game engine uses C# scripting.

Examples:

* Mobile games
* PC games
* VR applications

<br>

### Cloud Applications

Used heavily with Microsoft Azure.

Examples:

* Cloud APIs
* Microservices
* Enterprise systems

<br>

## 1.5 Features of C#

### Simple

Readable syntax and beginner-friendly structure.

Example:

```csharp
Console.WriteLine("Hello World");
```

<br>

### Object-Oriented

Everything revolves around objects and classes.

Benefits:

* Reusability
* Maintainability
* Scalability

<br>

### Type Safe

C# prevents many common programming mistakes.

Example:

```csharp
int age = 25;

// Invalid
age = "John";
```

The compiler catches the error before execution.

<br>

### Automatic Memory Management

C# automatically cleans unused memory using Garbage Collection — imagine a cleaner removing unused files from your desk automatically, so you focus on work, not cleaning.

(Covered in full detail, with a code example, in [§3.7 Garbage Collection](#37-garbage-collection-gc).)

<br>

### Cross Platform

Using .NET, C# applications can run on:

* Windows
* Linux
* macOS

<br>

## 1.6 Why Companies Use C#

Companies prefer C# because it provides:

* Fast development
* Strong security
* Large ecosystem
* Excellent tooling
* Long-term maintainability

Common use cases:

* Enterprise applications
* Banking software
* ERP systems
* Healthcare systems
* Government software
* Cloud services

<br>

## 1.7 C# in the Real Industry

Some common technologies used with C#:

| Technology        | Purpose            |
| ------------------ | -------------------- |
| ASP.NET Core       | Web Development      |
| Entity Framework   | Database Access       |
| SQL Server         | Database              |
| Azure              | Cloud Hosting          |
| Unity              | Game Development       |

<br>

## 1.8 Key Takeaways

* C# is a programming language developed by Microsoft.
* It is used for web, desktop, cloud, and game development.
* C# is object-oriented, type-safe, and easy to maintain.
* It supports automatic memory management.
* It runs on multiple operating systems through .NET.
* It is one of the most popular languages in enterprise software development.

<br>

---

<br>

# 2. C# and .NET

## 2.1 What is C#?

C# is a **programming language**.

Just like:

* English is a language for humans.
* C# is a language for computers.

We use C# to write instructions that tell a computer what to do.

Example:

```csharp
Console.WriteLine("Hello World");
```

This tells the computer to display text on the screen.

<br>

## 2.2 What is .NET?

.NET (pronounced "Dot Net") is a **development platform** created by Microsoft.

It provides everything needed to run C# applications:

* Runtime
* Libraries
* Tools
* Compilers

Without .NET, C# code cannot run.

<br>

## 2.3 Simple Analogy

Think of building a car.

| Component     | Real World              | C# Ecosystem     |
| -------------- | ------------------------ | ------------------ |
| Driver         | Person driving            | Developer          |
| Language       | Spoken language            | C#                  |
| Car Engine     | Powers the car              | .NET Runtime         |
| Car Parts      | Wheels, brakes, seats         | .NET Libraries        |
| Complete Car   | Working vehicle                 | .NET Platform            |

### Important

C# is only the language.

.NET is the entire platform that makes the application work.

<br>

## 2.4 Relationship Between C# and .NET

A common beginner mistake is thinking C# and .NET are the same thing.

They are different.

### C#

Used to write code.

Example:

```csharp
int age = 25;
Console.WriteLine(age);
```

### .NET

Provides:

* Runtime to execute code
* Libraries for common tasks
* Memory management
* File handling
* Networking
* Database connectivity

<br>

## 2.5 Why Do We Need .NET?

Imagine writing a program that:

* Reads files
* Connects to databases
* Sends emails
* Creates web APIs

Writing everything from scratch would be impossible.

.NET already provides these features through built-in libraries (like C++ STL, or Node modules).

Example:

```csharp
File.ReadAllText("data.txt");
```

You can read a file using a single line because .NET already contains the implementation.

<br>

## 2.6 .NET Libraries

.NET comes with thousands of ready-made classes.

These classes help developers perform common tasks.

Examples:

| Task             | Library Support |
| ------------------ | ------------------ |
| File Handling      | File                |
| Date and Time      | DateTime             |
| Collections        | List<T>               |
| Math Operations    | Math                    |
| Networking         | HttpClient                |

Example:

```csharp
DateTime today = DateTime.Now;

Console.WriteLine(today);
```

The DateTime class is provided by .NET.

<br>

## 2.7 Modern .NET

Older versions:

* .NET Framework

Modern versions:

* .NET 6
* .NET 7
* .NET 8
* .NET 9

Modern .NET is:

* Faster
* Cross-platform
* Open-source
* Cloud-friendly

Most companies today use modern .NET versions.

<br>

## 2.8 Popular Technologies Built on .NET

### ASP.NET Core

Used for:

* Websites
* REST APIs
* Backend services

<br>

### Entity Framework Core

Used for:

* Database operations
* Data access

<br>

### Blazor

Used for:

* Web applications using C#

<br>

### MAUI

Used for:

* Mobile applications
* Desktop applications

<br>

### Unity

Uses C# for game development.

Examples:

* Mobile games
* PC games
* VR applications

<br>

## 2.9 Real Industry Example

Suppose you are building an online shopping website.

### C# is used to:

* Write business logic
* Process orders
* Calculate totals

### .NET is used to:

* Run the application
* Connect databases
* Handle requests
* Manage memory
* Send responses

Together they create the complete application.

<br>

## 2.10 Interview Quick Revision

### What is C#?

A modern object-oriented programming language developed by Microsoft.

### What is .NET?

A development platform that provides the runtime, libraries, and tools needed to build and run applications.

### Is C# the same as .NET?

No.

* C# = Programming Language
* .NET = Platform

### Can C# run without .NET?

No.

C# code requires .NET to compile and execute.

<br>

## 2.11 Key Takeaways

* C# is a programming language.
* .NET is a development platform.
* C# is used to write code.
* .NET provides runtime, libraries, and tools.
* Most modern C# applications run on .NET.
* C# and .NET work together to build real-world software.

<br>

---

<br>

# 3. Common Language Runtime (CLR)

<div align = "center">
       
<img width="500" alt="image" src="https://github.com/user-attachments/assets/d9fc8ba9-8c43-4328-a025-bd7cd0fcfbdc" />
</div>

## 3.1 What is CLR?

CLR stands for **Common Language Runtime**.

It is the execution engine of .NET.

Its job is to:

* Run .NET applications
* Manage memory
* Handle exceptions
* Provide security
* Execute code efficiently

Without CLR, a C# program cannot run.

<br>

### Analogy

Think of a restaurant.

| Restaurant                                 | C# Application            |
| -------------------------------------------- | ---------------------------- |
| Customer places order                        | Developer writes code           |
| Kitchen prepares food                        | Compiler converts code            |
| Restaurant manager supervises everything     | CLR manages execution               |

The manager ensures everything runs smoothly. Similarly, CLR ensures your application runs correctly and efficiently. This same "manager" role comes up again in [§4.3](#43-main-components-of-net-architecture) when we look at the full .NET architecture.

<br>

## 3.2 Why Do We Need CLR?

Before designing the .NET platform, Microsoft recognized that different computers have different specifications, OS, and hardware.

So there is a need for a single runtime that converts the Intermediate Language generated by the compiler (common for all platforms) into machine code (specific to the machine) — and that's CLR, an approach adopted from the `Java JIT` framework.

Imagine developers had to manually:

* Allocate memory
* Free memory
* Handle crashes
* Optimize execution
* Manage security

Software development would become difficult and error-prone.

CLR handles these responsibilities automatically.

<br>

## 3.3 How a C# Program Runs

Consider this code:

```csharp
Console.WriteLine("Hello World");
```

### Step 1: Write C# Code

Developer writes source code.

↓

### Step 2: Compilation

The C# compiler converts source code into:

**Intermediate Language (IL)**

Also called:
* MSIL
* CIL

> [!Note]
> IL is not machine code.
> 
> It is a middle language understood by .NET.

↓

### Step 3: CLR Loads IL

CLR receives the IL code.

↓

### Step 4: JIT Compilation

CLR uses a JIT Compiler to convert IL into machine code.

↓

### Step 5: Execution

CPU executes the machine code.

↓

### Output

```text
Hello World
```

### Quick-recap diagram

```text
C# Source Code
       |
       v
C# Compiler
       |
       v
IL (Intermediate Language)
       |
       v
CLR
       |
       v
JIT Compiler
       |
       v
Machine Code
       |
       v
Execution
```

This is the complete flow followed by most C# applications — see [§4.2](#42-big-picture-full-execution-flow) for the fuller version that adds Metadata and Assembly packaging.

<br>

## 3.4 Intermediate Language (IL)

IL is a platform-independent language.

Why?

Because the same compiled application can run on multiple operating systems. CLR converts IL into machine code suitable for the operating system it's running on — this is one reason .NET is cross-platform (full OS list in [§4.5](#45-operating-system)).

<br>

## 3.5 What is JIT Compiler?

JIT stands for:

**Just-In-Time Compiler**

Its job is to convert IL into machine code only when needed.

### Why not directly generate machine code?

Different operating systems and processors use different machine instructions.

JIT allows the same IL code to work everywhere.

<br>

### Simple Analogy

Imagine a movie with subtitles.

The movie is the same.

Subtitles change depending on the audience.

Similarly:

* IL stays the same
* JIT generates machine code specific to the system

<br>

## 3.6 Memory Management

One of the biggest responsibilities of CLR is memory management.

When objects are created:

```csharp
Person p = new Person();
```

Memory is allocated automatically.

When objects are no longer needed, CLR cleans them up automatically.

Developers do not need to manually release memory in most cases.

<br>

## 3.7 Garbage Collection (GC)

Garbage Collection is a feature provided by CLR.

Its job is to remove unused objects from memory.

### Example

```csharp
Person p = new Person();

p = null;
```

The object is no longer referenced.

Garbage Collector will eventually remove it and free memory.

<br>

### Real-World Analogy

Think of garbage collection as a cleaning staff.

When an employee leaves unwanted papers on a desk:

* Cleaner identifies unused items
* Removes them
* Frees space

Similarly, Garbage Collector removes unused objects.

<br>

## 3.8 Exception Handling

CLR helps manage runtime errors.

Example:

```csharp
int number = 10;
int result = number / 0;
```

This causes an error.

CLR detects the issue and throws an exception instead of crashing unpredictably.

Benefits:

* Better debugging
* Safer applications
* Easier error handling

<br>

## 3.9 Security

CLR provides security checks during execution.

It helps:

* Prevent invalid operations
* Verify code safety
* Protect application resources

This reduces many common programming mistakes.

<br>

## 3.10 Benefits of CLR

### Automatic Memory Management

No need to manually free memory.

<br>

### Garbage Collection

Unused objects are cleaned automatically.

<br>

### Better Performance

JIT compiles code efficiently.

<br>

### Platform Independence

Same IL can run on multiple operating systems.

<br>

### Exception Handling

Runtime errors are managed safely.

<br>

### Security

Additional protection during execution.

<br>

## 3.11 Interview Quick Revision

### What is CLR?

CLR (Common Language Runtime) is the execution engine of .NET responsible for running applications and managing resources.

### What are the major responsibilities of CLR?

* Code execution
* Memory management
* Garbage collection
* Exception handling
* Security
* JIT compilation

### What is IL?

Intermediate Language generated by the C# compiler before execution.

### What is JIT?

Just-In-Time Compiler that converts IL into machine code during execution.

### Does CLR manage memory?

Yes. CLR automatically manages memory and performs garbage collection.

<br>

---

<br>

# 4. .NET Architecture

## 4.1 What is .NET Architecture?

.NET Architecture describes how different components work together to run a C# application.

When we write a C# program, many things happen behind the scenes before the output appears on the screen.

Understanding the architecture helps answer:

* How code gets executed
* What CLR does
* Where .NET libraries fit in
* How applications run on different operating systems

### [Namespace](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/e2f20b92c613979c752b33b00fda705fa88fc20f/03_Basics_Of_CSharp/01_Notes/Namespace.md)

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/2192894f-6351-4c62-ab45-543107d7179c" />
</div>

<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/d2744671-5fd3-4275-8698-7396c3e442d7" />
</div>

<br>

## 4.2 Big Picture (Full Execution Flow)

<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/79f15f6c-6211-45fb-8516-6602bf146ac3" />
</div>

When a developer writes a C# application, the full flow — including the pieces §3.3's simpler diagram left out (Metadata, Assembly packaging) — looks like this:

```text
       Developer
           |
           v
       C# Source Code (Press Run Button)
           |
           v
       C# Compiler
           |
           v
       Intermediate Language (IL - Universal to all devices) + Metadata
           |
           v
       Byte Code packed into Assembly (.exe or .dll) package - IL
           |
           v
   CLR Environment
+----------------------+
|          CLR         |
|           |          |
|           v          |
|       JIT Compiler   |
+----------------------+
           |
           v
       Native Machine Code (Specific to that device)
           |
           v
       Operating System
```

This is the complete journey from code to execution.

<br>

## 4.3 Main Components of .NET Architecture

The architecture mainly consists of:

* C# Source Code
* Compiler
* Intermediate Language (IL)
* CLR
* JIT Compiler
* .NET Class Library
* Operating System

CLR again plays the "manager" role described back in [§3.1](#31-what-is-clr) — the workers (compiler, JIT) do the actual work, but CLR ensures everything runs properly.

<br>

## 4.4 .NET Class Library

.NET provides thousands of ready-made classes.

These libraries save developers from writing everything from scratch.

Examples:

| Task            | Class     |
| ------------------ | ------------ |
| Display Output      | Console        |
| Current Date        | DateTime         |
| File Handling       | File               |
| Lists               | List<T>              |
| Mathematics         | Math                   |

Example:

```csharp
DateTime today = DateTime.Now;
```

The `DateTime` class comes from the .NET Class Library, and it — along with every other class in the table above — is available to the application at any point in the flow shown in §4.2, alongside the compiler/CLR/JIT pipeline.

<br>

## 4.5 Operating System

After JIT compilation, machine code runs on the operating system.

Examples:

* Windows
* Linux
* macOS

Modern .NET supports all major operating systems — the same IL can run on any of them, which is why .NET is called cross-platform.

<br>

---

<br>

# 5. First Application

## 5.1 Introduction

The traditional first program in almost every programming language is:

```text
Hello World
```

Its purpose is simple:

* Verify the development environment is working
* Understand basic program structure
* Learn how code gets executed

<br>

## 5.2 Creating the First C# Program

```csharp
Console.WriteLine("Hello World");
```

Output:

```text
Hello World
```

This displays the text on the console window.

<br>

## 5.3 Complete Program Structure

A typical C# console application looks like this:

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello World");
    }
}
```

Output:

```text
Hello World
```

<br>

## 5.4 Understanding Each Part

### using System;

Imports the System namespace.

It provides access to useful classes such as:

* Console
* DateTime
* Math

<br>

### class Program

Defines a class named Program.

Think of a class as a container that holds code.

<br>

### static void Main()

This is the entry point of the application.

When the program starts, execution begins from Main().

<br>

### Curly Braces {}

```csharp
{
}
```

Used to define a block of code.

Everything inside belongs together.

<br>

## 5.5 Program Execution Flow

When you run a C# application:

### Step 1

Application starts.

↓

### Step 2

CLR looks for Main().

↓

### Step 3

Code inside Main() executes.

↓

### Step 4

Output appears.

```text
Hello World
```

### Visual Flow

```text
Program Starts
       |
       v
Main()
       |
       v
Console.WriteLine()
       |
       v
Output Displayed
```

Execution happens from top to bottom.

<br>

## 5.6 Why the Main Method is Important

Every console application needs a starting point.

The CLR starts execution from Main().

Example:

```csharp
static void Main()
{
    Console.WriteLine("Program Started");
}
```

Output:

```text
Program Started
```

Without Main(), the application does not know where to begin execution.

<br>

---

<br>
