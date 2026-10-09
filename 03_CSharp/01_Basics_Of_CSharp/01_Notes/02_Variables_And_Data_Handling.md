# 2. Variables and Data Handling

<br>

## Index

## Index

1. [Variables](#21-variables)
   - [What is a Variable](#what-is-a-variable)
   - [Variable Declaration](#variable-declaration)
   - [Common Data Types](#common-data-types)
   - [Auto Detecting Variables - `var`](#auto-detecting-variables---keyword-var)
   - [Variable Naming Rules](#variable-naming-rules)
   - [Updating Variables](#updating-variables)
2. [Constants](#22-constants)
   - [When to Use Constants](#when-to-use-constants)
3. [Overflowing](#23-overflowing)
   - [What is Overflow](#what-is-overflow)
   - [Checked Keyword](#checked-keyword)
   - [Unchecked Keyword](#unchecked-keyword)
   - [Why Developers Care](#why-developers-care)
4. [Scope](#24-scope)
   - [What is Scope](#what-is-scope)
   - [Local Scope](#local-scope)
   - [Invalid Access](#invalid-access)
   - [Block Scope](#block-scope)
   - [Why Scope Matters](#why-scope-matters)
5. [Using `{}`](#using-)
6. [Type Conversion](#25-type-conversion)
   - [Implicit Conversion](#implicit-conversion)
   - [Explicit Conversion (Casting)](#explicit-conversion-casting)
   - [Convert Class](#convert-class-for-non-convertible-types)
   - [Parse Method](#parse-method)
   - [TryParse](#tryparse-recommended)
   - [Common Type Conversions](#common-type-conversions)

<br>


## 2.1 Variables

### What is a Variable?

A variable is a named storage location used to hold data.

Think of a variable as a labeled box:

```text
Age Box   -> 21
Name Box  -> Yash
Price Box -> 99.99
```

The value inside the box can change during program execution.

<br>

### Variable Declaration

```csharp
int age = 21;
string name = "Yash";
double price = 99.99;
```

Syntax:

```csharp
dataType variableName = value;
```

<br>

### Common Data Types

| Type   | Example |
| ------ | ------- |
| int    | 10      |
| double | 10.5    |
| char   | 'A'     |
| string | "Hello" |
| bool   | true    |

<div align = "center">
    
<img width="500" alt="image" src="https://github.com/user-attachments/assets/bbe83ebe-40c5-4d9a-8278-9e26a1341879" />
    <img width="400" alt="image" src="https://github.com/user-attachments/assets/f9e3fd2c-55ed-44cf-98e0-21cb0fd087bb" />
    <img width="400" alt="image" src="https://github.com/user-attachments/assets/6b1d3fe1-54ea-49ae-ad52-80b6ebacaaa7" />
</div>

Example:

```csharp
int marks = 95;
double salary = 50000.50;
char grade = 'A';
string city = "Pune";
bool isActive = true;
```

---

> [!Note]
>
> ## Auto Detecting Variables - Keyword `var`
>
> Instead of explicitly writing the data type, C# can automatically determine the type from the value assigned using the `var` keyword.
>
> ### Normal Variable Declaration
>
> ```csharp
> using System;
>
> namespace LearnNumbers
> {
>     internal class Program
>     {
>         static void Main(string[] args)
>         {
>             byte Num = 2;
>             Console.WriteLine(Num);
>
>             int intNum = 10;
>             Console.WriteLine(intNum);
>
>             float fNum = 12.5f;
>             Console.WriteLine(fNum);
>
>             char ch = 'A';
>             Console.WriteLine(ch);
>
>             string str = "Yash";
>             Console.WriteLine(str);
>
>             bool isLoading = false;
>             Console.WriteLine(isLoading);
>         }
>     }
> }
> ```
>
> ### Using `var`
>
> ```csharp
> using System;
>
> namespace LearnNumbers
> {
>     internal class Program
>     {
>         static void Main(string[] args)
>         {
>             var Num1 = 2;
>             Console.WriteLine(Num1);
>
>             var intNum1 = 10;
>             Console.WriteLine(intNum1);
>
>             var fNum1 = 12.5f;
>             Console.WriteLine(fNum1);
>
>             var ch1 = 'A';
>             Console.WriteLine(ch1);
>
>             var str1 = "Yash";
>             Console.WriteLine(str1);
>
>             var isLoading1 = false;
>             Console.WriteLine(isLoading1);
>         }
>     }
> }
> ```
> 
> ### Important Points
>
> * `var` does **not** mean the variable has no type.
> * The compiler determines the type during compilation.
> * After the type is determined, it cannot change.
>
> ```csharp
> var age = 21;     // int
> var name = "Yash"; // string
> var price = 99.5; // double
> ```
>
> Invalid:
>
> ```csharp
> var age = 21;
> age = "Yash"; // Error
> ```
>
> Think of `var` as:
>
> > "Compiler, you figure out the type for me."
----






<br>

### Variable Naming Rules

Valid:

```csharp
int age;
string firstName;
double accountBalance;
```

Invalid:

```csharp
int 1age;
string first-name;
```

Rules:

* Cannot start with a number
* Cannot contain spaces
* Cannot use special characters (except _)
* Should use meaningful names

Good:

```csharp
int employeeAge;
```

Bad:

```csharp
int x;
```

<br>

### Updating Variables

```csharp
int age = 21;

age = 22;
```

The latest value replaces the old value.

<br>

## 2.2 Constants

A constant is a value that cannot change after initialization.

Syntax:

```csharp
const double PI = 3.14159;
```

Example:

```csharp
const int MAX_USERS = 100;

Console.WriteLine(MAX_USERS);
```

Invalid:

```csharp
MAX_USERS = 200;
```

Compilation error.

<br>

### When to Use Constants

Use constants for fixed values:

```csharp
const double GST_RATE = 0.18;
const int MAX_RETRY = 3;
const string COMPANY_NAME = "ABC Ltd";
```

### Naming Conventions
<div align = "center">
    <img width="400"  alt="image" src="https://github.com/user-attachments/assets/420001f7-0367-4cc9-bc67-dbeb44025677" />
</div>






<br>

## 2.3 Overflowing

### What is Overflow?

Overflow occurs when a value exceeds the storage capacity of its data type.

Example:

```csharp
byte number = 255;
```

A byte can store:

```text
0 to 255
```

Now:

```csharp
byte number = 255;
number++;
```

Result:

```text
0
```

The value wraps around because the limit was exceeded.

<br>

### Visual Example

```text
Byte Range

0 ............ 255

255 + 1

↓

0
```

<br>

### Checked Keyword

---
> [!Tip]
> Not much used, but just know that it exist 
---

Detect overflow explicitly:

```csharp
checked
{
    byte number = 255;
    number++;
}
```

This throws an exception instead of silently wrapping.

<br>

### Unchecked Keyword

Ignore overflow checking:

```csharp
unchecked
{
    byte number = 255;
    number++;
}
```

Result:

```text
0
```

<br>

### Why Developers Care

Overflow can cause:

* Incorrect calculations
* Financial bugs
* Data corruption

Common in:

* Banking systems
* Inventory systems
* Large calculations

<br>

## 2.4 Scope

### What is Scope?

Scope defines where a variable can be accessed.

A variable only exists inside the block where it is declared.

<br>

### Local Scope

```csharp
static void Main()
{
    int age = 21;

    Console.WriteLine(age);
}
```

Valid because age exists inside Main().

<br>

### Invalid Access

```csharp
static void Main()
{
    int age = 21;
}

Console.WriteLine(age);
```

Compilation error.

The variable exists only inside Main().

<br>

### Block Scope

```csharp
if (true)
{
    int number = 10;
}

Console.WriteLine(number);
```

Compilation error.

number exists only inside the if block.

<br>

### Visual Example

```text
Main()
{
    age -> Accessible

    if()
    {
        number -> Accessible only here
    }
}
```

<br>

### Why Scope Matters

Benefits:

* Prevents accidental modification
* Reduces bugs
* Improves readability
* Saves memory

<br>


## Using `{}`     
```csharp
            Console.WriteLine("======================================");
            Console.WriteLine("========  Using {} =========");
            Console.WriteLine("First Val {0} , Second Val {1}, Third Val {2} ", 1,2,3,4);
            // First Val 1 , Second Val 2, Third Val 3


            Console.WriteLine("Byte - Min value {0} , MaxValue {1} ", byte.MinValue, byte.MaxValue);
            //Byte - Min value 0 , MaxValue 255

            Console.WriteLine("Int - Min value {0} , MaxValue {1} ", int.MinValue, int.MaxValue);
            // Int - Min value -2147483648 , MaxValue 2147483647

            Console.WriteLine("======================================");
            Console.WriteLine("========  Using Const =========");

            const float PI = 3.142f; //pascal case
            //PI = 3;  //error changing
```


<br>

## 2.5 Type Conversion

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/e1a7524a-f2ad-4cf8-8bc7-7761478aa444" />
</div>

Sometimes we need to convert one data type into another.

Example:

```csharp
int number = 10;
double result = number;
```

<br>

### Implicit Conversion
 
<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/315538ed-62fa-4e0c-ad55-4676177f4e53" />

</div>

Performed automatically when there is no risk of data loss.

```csharp
int number = 10;

double result = number;
```

Result:

```text
10 → 10.0
```

<br>

### Explicit Conversion (Casting)

> When chance for data loss, no implicit type conversion
 
<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/717cd739-f999-463b-bd0b-237f07585841" />
    <img width="600" alt="image" src="https://github.com/user-attachments/assets/57ca9614-0fb5-41c5-a6d3-998fa4652bb8" />

</div>


Performed manually when data loss may occur.

```csharp
double price = 10.99;

int value = (int)price;
```

Result:

```text
10
```

Decimal part is removed.

<br>

### Convert Class (For Non Convertible Types)
<div align = "center">
    
<img width="600" alt="image" src="https://github.com/user-attachments/assets/28b011cb-ab1f-43b3-9a7b-cee8926684e2" />
<img width="600" alt="image" src="https://github.com/user-attachments/assets/d6231008-6246-4bf3-bb7b-005d93fc4a03" />
<img width="600" alt="image" src="https://github.com/user-attachments/assets/83e71662-c2be-4856-8642-299834ca2d11" />


</div>



---
> [!Tip]
> Recall **C++** `to_string()` for int to string
> 
> and
>
> **C++** `stoi()` for string to int
---

```csharp
string ageText = "25";

int age = Convert.ToInt32(ageText);
```

Result:

```text
25
```

Useful when reading user input or data from files/databases.

<br>

### Parse Method

```csharp
string numberText = "100";

int number = int.Parse(numberText);
```

Result:

```text
100
```

<br>


```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNumbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Implicit Conversion
            byte n2 = 255;
            int intNum = n2;
            Console.WriteLine("EG Implicit  - Byte to Int - {0}", intNum);

            // Explicit Conversion
            int n1  = 1000;
            byte bnum = (byte)n1; //forceful conversion 1000 % 256 = 232
            Console.WriteLine("EG Explicit - Int to Byte - {0}", bnum); //232

            //byte bnum2 = Convert.ToByte(n1);
            //Console.WriteLine("EG Explicit (Convert.ToByte) - Int to Byte - {0}", bnum2);
            // Overflow Error

            try { 
            
            byte bnum2 = Convert.ToByte(n1);
            Console.WriteLine("EG Explicit (Convert.ToByte) - Int to Byte - {0}", bnum2);
            }
            catch (Exception)
            {

                Console.WriteLine("No forceful conversion, due to overflow of byte value");
            }
            //Overflow Error

            // Non Compatible Types
            string hundred = "100";
            int strnum = Convert.ToInt32(hundred);
            Console.WriteLine("Using Convert.ToInt32 (String to Num) - {0}",strnum); //100

            int shambhar = 100;
            string intString = Convert.ToString(shambhar);
            Console.WriteLine("Using Convert.ToString (Int to String) - {0}",intString); ///100

            string testParse = "1000";
            int testP = int.Parse(testParse);
            Console.WriteLine("Using int.Parse (String to Num) - {0}", testP); //1000

        }
    }
}

```

### TryParse (Recommended)

Safely converts values.

```csharp
string input = "100";

bool success = int.TryParse(input, out int number);
```

If conversion succeeds:

```text
success = true
number = 100
```

If conversion fails:

```text
success = false
```

No exception is thrown.

Used heavily in production applications.

<br>

## Common Type Conversions

```csharp
int → double
double → int
string → int
string → double
string → DateTime
```

Examples:

```csharp
int age = 21;
double ageDouble = age;

double price = 99.99;
int rounded = (int)price;

string text = "100";
int number = Convert.ToInt32(text);
```


<br>

---

<br>

