# 5. Data Structures and Types

<br>

**Index:**

1. [5.1 Arrays](#51-arrays)
2. [5.2 Strings](#52-strings)
3. [5.3 Enums](#53-enums)
4. [5.4 Reference vs Value Types](#54-reference-types-vs-value-types)
5. [5.5 Collections (Lists)](#55-collections-list)
6. [5.6 Random Class](#56-random-class)


<br>

## 5.1 Arrays

#### [Arrays in Depth](https://github.com/Yash-Bandal/DeltaX-Bootcamp/blob/main/03_CSharp/01_Basics_Of_CSharp/01_Notes/Concepts/Arrays_In_Depth.md)

### What is an Array?



An array stores multiple values of the same type in a single variable.

Without an array:

```csharp
int mark1 = 80;
int mark2 = 85;
int mark3 = 90;
```

Using an array:

```csharp
int[] marks = { 80, 85, 90 };
```

> [!Note]
> An Array is a class datatype (reference value based)

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/0224e9cc-f77e-4cf0-bc4c-4487992b1c30" />
</div>

<br>


### Types of Arrays in `C#`
<table align="center">
  <tr>
    <td colspan="2" align="center">
      <img
        width="500"
        alt="image"
        src="https://github.com/user-attachments/assets/afcf734c-3c0c-4e7f-a78b-a3a8a0c822a0"
      />
    </td>
  </tr>
  <tr>
    <td align="center">
      <img
        width="450"
        alt="image"
        src="https://github.com/user-attachments/assets/c47b5b72-5eff-4649-9b36-6ec49907b5aa"
      />
    </td>
    <td align="center">
      <img
        width="450"
        alt="image"
        src="https://github.com/user-attachments/assets/33be9fc3-e4c4-4fb6-b745-0ac2803cc286"
      />
    </td>
  </tr>
</table>

**Rectangular Array**
<div align = "center">
<img width="500"  alt="image" src="https://github.com/user-attachments/assets/f7640676-b15c-452b-a6bb-d16a7426f31c" />
</div>

**Jagged array**
<div align = "center">
<img width="500"  alt="image" src="https://github.com/user-attachments/assets/0a82f53a-a42f-443d-afb8-d0670bbc43bc" />
    <p>In jagged array, we have 2 square brackets, and in rect array, we have 1 square array, but we seperate the dimentsions using a `,`</p>
    <img width="500" alt="image" src="https://github.com/user-attachments/assets/0bb0471e-0835-4845-9f72-2858f041a908" />

</div>



<br>

### Accessing Elements

Arrays use zero-based indexing.

```csharp
int[] marks = { 80, 85, 90 };

Console.WriteLine(marks[0]);
Console.WriteLine(marks[1]);
```

Output:

```text
80
85
```

<br>

### Array Length

```csharp
int[] marks = { 80, 85, 90 };

Console.WriteLine(marks.Length);
```

Output:

```text
3
```

<br>

### Looping Through an Array

```csharp
int[] marks = { 80, 85, 90 };

for (int i = 0; i < marks.Length; i++)
{
    Console.WriteLine(marks[i]);
}
```

<br>

### Foreach Loop

Best way to read all items.

```csharp
foreach (int mark in marks)
{
    Console.WriteLine(mark);
}
```

<br>

### Important

Arrays have a fixed size.

```csharp
int[] numbers = new int[3];
```

Once created, the size cannot change.


<br>


---

<br>

## 5.2 Strings

### [Strings in Depth](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/a1f66e59a0f22db0e649249db36f01ed2c9faed0/03_Basics_Of_CSharp/01_Notes/Concepts/Strings_In_Depth.md)

### What is a String?

A string represents text.

```csharp
string str = "Text Data";
```

> [!Note]
> ```csharp
> string name = "Yash"
> ```
> and
> ```csharp
> using System; //namespace
> String name = "Yash"
> ```
> Are the Same, `S` and `s` difference just
>
> Same as
> ```csharp
> INT32 i;
> ```
> and
> ```csharp
> int i;
> ```

<br>

### Common String Operations

#### Length

```csharp
string name = "Yash";

Console.WriteLine(name.Length);
```

Output:

```text
4
```

<br>

#### ToUpper

```csharp
Console.WriteLine(name.ToUpper());
```

Output:

```text
YASH
```

<br>

#### ToLower

```csharp
Console.WriteLine(name.ToLower());
```

Output:

```text
yash
```

<br>

#### Contains

```csharp
Console.WriteLine(name.Contains("Ya"));
```

Output:

```text
True
```

<br>

#### Replace

```csharp
string text = "Hello World";

Console.WriteLine(text.Replace("World", "C#"));
```

Output:

```text
Hello C#
```

<br>

### String Concatenation

```csharp
string firstName = "Yash";
string lastName = "Bandal";

string fullName = firstName + " " + lastName;
```

<br>

### String Interpolation (Recommended)

```csharp
string name = "Yash";

Console.WriteLine($"Welcome {name}");
```
<div align = "center">
    
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/2caa21fe-cbe1-4697-ac75-aa4383d9bc6b" />

<img width="600" alt="image" src="https://github.com/user-attachments/assets/9ec5de3d-ae9e-4f69-bf5d-0cc303cc8e11" />


<img width="600" alt="image" src="https://github.com/user-attachments/assets/964f36be-85b5-4b1f-8877-bbc68b4a6d6b" />

</div>


```csharp
int appleCnt = 5;

// String Interpolation
string approach1 = $"I have {appleCnt} apples."; 

// string.Format method
string approach2 = string.Format("I have {0} apples.", appleCnt); 
```

Output:

```text
Welcome Yash
```

Widely used in modern C# code.

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/0d6fadc2-1dea-48f0-8ee9-a6b1de4f2e37" />

<img width="600"  alt="image" src="https://github.com/user-attachments/assets/e64b1cd9-fe59-4c1f-a950-19877b8b97f7" />



<img width="600"  alt="image" src="https://github.com/user-attachments/assets/80ef82de-d1d5-42ce-ac17-08f55f9ea862" />


<img width="600"  alt="image" src="https://github.com/user-attachments/assets/5188e25d-df50-415a-8125-a2a784ef412e" />

</div>


### Example
```csharp
using LearnNumbers.Math;
using System;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace LearnNumbers
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string hw = "Hellow" + " World";
            Console.WriteLine(hw);
           //Hellow World


            //==========================================

            string fname = "Yash";
            string lname = "Bandal";

            string fn1 = $"My name is {fname} {lname}";
            Console.WriteLine(fn1);

            // My name is Yash Bandal


            //=================================

            string ffname = "Darshan";
            string flname = "Patil";

            string fn2 = string.Format("My friend name is {0} {1}", ffname, flname);
            Console.WriteLine(fn2);
            
            // My friend name is Darshan Patil

            //================================

            string[] strnums =  { "1", "2", "3", "5", "5"};
            string res = string.Join(",", strnums);
            Console.WriteLine(res); // 1,2,3,4,5
            string res2 = string.Join("", strnums);
            Console.WriteLine(res2); //12345


            //=====================================
            var text = "Hey Darshan ..! \nLook at this new path \nc:\\folder1\\folder2\nc:\\folder3\\folder4";
            Console.WriteLine(text);

            //very unreadable
            /*
                Hey Darshan ..!
                Look at this new path
                c:\folder1\folder2
                c:\folder3\folder4
             */

            //readable verbatim text

            Console.WriteLine("\n");

            var text2 = @"Hey Darshan again ..! 
Look at this new clean path
c:\folder1\folder2
c:\folder3\folder4";

            Console.WriteLine(text2);
            /*
                Hey Darshan again..!
                Look at this new clean path
                c:\folder1\folder2
                c:\folder3\folder4
             */

        }
    }
}
```

<br>

---


<br>



## 5.3 Enums

### What is an Enum?

Enum (Enumeration) is a special type used to represent a fixed set of constants.

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/e5bd090e-9606-4882-b3e0-de09cbf68943" />

</div>

Without enum:

```csharp
int status = 1;
```

What does 1 mean?

Not obvious.

<br>

### Using Enum

```csharp
enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}
```

Usage:

```csharp
OrderStatus status = OrderStatus.Completed;
```

<br>

### Benefits

* Readable code
* Avoid magic numbers
* Better maintainability

<br>

### Example

```csharp
enum UserRole
{
    Admin,
    Manager,
    Customer
}
```

```csharp
UserRole role = UserRole.Admin;
```

**Example:**
```csharp
using System;

namespace CSharpFundamentals
{
    public enum ShippingMethod
    {
        RegularAirMail = 1,
        RegisteredAirMail = 2,
        Express = 3
    }

    class Program
    {
        static void Main(string[] args)
        {
            var method = ShippingMethod.Express;
            Console.WriteLine((int)method); //3
            Console.WriteLine(method);  //Express

            var methodId = 3;
            Console.WriteLine((ShippingMethod)methodId); //Express

            Console.WriteLine(method.ToString()); //express

            var methodName = "Express";
            var shippingMethod = (ShippingMethod)Enum.Parse(typeof(ShippingMethod), methodName);
        }
    }
}
```


<br>


---

<br>



## 5.4 Reference Types vs Value Types

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/a49282bd-365e-479e-a01a-a08e1c1f57a7" />
</div>


<div align = "center">
    <table>
        <tr>
            <td>
                <img width="500"  alt="image" src="https://github.com/user-attachments/assets/93b264a4-d8c6-4584-a47d-deecb430ee96" />
                <p align = "center">Primitive and Non primitive types </p>
            </td>
        </tr>
    </table>
</div>

This is one of the most important C# concepts.

<br>

---
> [!Note]
> **Q: Does the word `arr` when defined point to first element, or a seperate object?**
>
> (Diff than C++)
>  
> For C# arrays, think:
>
> ```
> arr
>   │
>   ▼
> Array Object
> +---------------------+
> | Length = 5          |
> | [0] = 1             |
> | [1] = 2             |
> | [2] = 3             |
> | [3] = 4             |
> | [4] = 5             |
> +---------------------+
>```
> 
> So the most accurate answer is:
>
> `arr` stores a reference to the entire array object. The elements are stored inside that object, starting at the first element,
>
> but `arr` itself is not considered a reference to just the first element as it often is in `C/C++`.
> 

<br>

### Value Types

Store the actual value.

Examples:

```csharp
int
double
bool
char
struct
enum
```

Example:

```csharp
int x = 10;
int y = x;

y = 20;
```

Result:

```text
x = 10
y = 20
```

A copy is created.

<br>

### Reference Types

Store a reference (address) to an object.

Examples:

```csharp
class
string
array
List<T>
```

Example:

```csharp
Person p1 = new Person();

Person p2 = p1;
```

Both variables point to the same object.

<br>

### Visual Comparison

#### Value Type

```text
x -> 10

y -> 10
```

Separate copies.

<br>

#### Reference Type

```text
p1 ----\
        > Object
p2 ----/
```

Both refer to the same object.

<br>

**Example:**
```csharp
using System;
using System.Security.Cryptography.X509Certificates;


namespace LearnNumbers
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Value type
            int a = 10;
            int b = a;
            b++;

            /*
              value type variable, so no increment of a happens,
              as b holds its own seperate copy of a and 
              does not point to a itself
             */

            Console.WriteLine(string.Format("a is {0}, and b is {1}",a,b));
            //a is 10 ,  b is 11


            //=======================================

            //Reference type 
            int[] arr1 = { 1, 2, 3 };
            int[] arr2 = arr1;  //ref type, arr2 points to arr1 

            arr2[0] = 10; //modify

            Console.WriteLine(string.Format("arr1[0] : {0} , arr2[0] : {1}", arr1[0] , arr2[0]));
            //arr1[0] : 10 , arr2[0] : 10

            /*
             Notice, unlike value type, 
            here reference to actual arr1[0] index 0 was made
            while modifying arr2[0],

            so both change
             */



        }
    }
}
```

<div align = "center">
<img width="600"  alt="image" src="https://github.com/user-attachments/assets/c50da620-0d5e-47b4-818e-77e1a121bf64" />
</div>



<br>


**Example 2:**


```csharp
using System;
using System.Security.Cryptography.X509Certificates;


namespace LearnNumbers
{
    internal class Program
    {
        //define class
        public class Person
        {
            public int age;
        }

        //Helper functions
        //pass value type variable (integer here)
        public static void incrementVal(int Number)   //copy of Number var recieved
        {
            Number += 10;  //changes made to the copy
        }


        //pass reference type variable (class object)
        public static void incrementRef(Person p1) //actual Person object recieved
        {
             p1.age += 10;   //changes made to the actual object
        }

        static void Main(string[] args)
        {
            // Test Value type 
            int Number = 0;
            incrementVal(Number);
            Console.WriteLine(string.Format("After incremnt of Value type var : {0}", Number));
            //No change happen
            // After incremnt of Value type var : 0

            //============================================


            // Test value type
            Person p1 = new Person();
            p1.age = 10;

            incrementRef(p1);
            Console.WriteLine(string.Format("After incremnt of Ref type var : {0}", p1.age));
            //Change happened
            // After incremnt of Ref type var : 20

        }
    }
}
```
```
Main()
└── Number = 0

incrementVal()
└── Number = 0   (copy)
```
The function receives a copy.

Changes affect only the copy.

Result:
```
0
```


<br>

### Quick Rule

```text
Struct = Value Type

Class = Reference Type
```

Remember this for interviews.

<br>


---


<br>


## 5.5 Collections (List<T>)

#### [Lists in Depth](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/03_Basics_Of_CSharp/01_Notes/Concepts/Lists_In_Depth.md)

### Why Not Always Use Arrays?


Arrays have fixed size.

```csharp
int[] numbers = new int[3];
```

Cannot grow automatically.

<br>

### What is List<T>?


<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/48792860-07a6-4a04-a4f8-a72a7fa17a4d" />
<img width="454" height="406" alt="image" src="https://github.com/user-attachments/assets/a36048ce-3043-441f-8199-e7492033c57d" />
</div>


A List is a dynamic collection.

It can grow and shrink automatically.

```csharp
List<int> numbers = new List<int>();
```

<br>

### Add Items

```csharp
numbers.Add(10);
numbers.Add(20);
numbers.Add(30);
```

<br>

### Access Items

```csharp
Console.WriteLine(numbers[0]);
```

Output:

```text
10
```

<br>

### Remove Items

```csharp
numbers.Remove(20);
```

<br>

### Count

```csharp
Console.WriteLine(numbers.Count);
```

<br>

### Loop Through List

```csharp
foreach (int number in numbers)
{
    Console.WriteLine(number);
}
```

<br>

### Why Developers Prefer List<T>

* Dynamic size
* Easy to use
* Rich built-in methods
* Common in real-world applications

<br>

## Array vs List

| Array            | List<T>         |
| ---------------- | --------------- |
| Fixed Size       | Dynamic Size    |
| Faster           | Slightly Slower |
| Limited Features | Many Features   |
| Less Flexible    | More Flexible   |

Most business applications use `List<T>` far more often than arrays.


<br>

**Example:**
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace LearnNumbers
{
    internal partial class Program
    {
        public static void printList(List<int> ls)
        {
            for (int i = 0; i < ls.Count; i++)
            {
                Console.Write(ls[i] + " ");
            }

            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            // 0   1   2
            List<int> arr = new List<int>() { 10, 20, 30 };
            printList(arr);
            // 10 20 30

            //Add()
            var arr2 = new[] { 40, 50, 60 };
            foreach (var item in arr2)
            {
                arr.Add(item);
            }

            printList(arr);
            // 10 20 30 40 50 60

            //===========================================================

            // AddRange()
            //arr.AddRange(new List<int>() { 70, 80, 90 });

            //or
            List<int> arr3 = new List<int>() { 30, 20, 10};
            arr.AddRange(arr3);

            printList(arr);
            //10 20 30 40 50 60 30 20 10

            //===========================================================

            // IndexOf and LastIndexOf 
            Console.WriteLine(string.Format("Index of 10 is {0}",arr.IndexOf(10)));
            Console.WriteLine(string.Format("Last Index of 10 is {0}",arr.LastIndexOf(10)));
            // Index of 10 is 0
            // Last Index of 10 is 8

            //===========================================================

            //Count 
            Console.WriteLine("Count of elements : " + arr.Count );
            //Count of elements : 9

            //===========================================================
            // Remove first occurance, single value
            arr.Remove(10);
            printList(arr);
            //20 30 40 50 60 30 20 10


            // using foreach gives error while modifying
            //foreach (var item in arr)
            //{
            //    if(item == 20)
            //    {
            //        arr.Remove(item);
            //    }
            //}
            //printList(arr);
            //Collection was modified; enumeration operation may not execute


            //Removing multiple elements  - here 20

            Console.WriteLine("Remove multiple elements:");
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == 20)
                {
                    arr.Remove(arr[i]);
                }
            }
            printList(arr);
            //30 40 50 60 30 10

            //===========================================================
            Console.WriteLine("After clearing List: ");
            arr.Clear();
            printList(arr);

            Console.WriteLine("Count of elements : " + arr.Count );
            // Count of elements : 0 


        }
    }
}

```


<br>

---


<br>



## 5.6 Random Class

### What is Random?

Used to generate random numbers.

```csharp
Random random = new Random();
```

<br>

### Generate Random Number

```csharp
Random random = new Random();

int number = random.Next();
```

<br>

### Random Number in Range

```csharp
Random random = new Random();

int number = random.Next(1, 11);
```

Possible values:

```text
1 to 10
```

Upper limit is excluded.

<br>

### Example

```csharp
Random random = new Random();

Console.WriteLine(random.Next(1, 7));
```

Simulates a dice roll.

Output:

```text
1 to 6
```

<br>

### Random Double

```csharp
Console.WriteLine(random.NextDouble());
```

Output:

```text
0.0 to 1.0
```

<br>

### Common Use Cases

* Games
* OTP generation
* Simulations
* Test data generation
* Random selections

<br>

**Examples:**
1. Random 8 character password generator
- This generates a randowm string of 8 characters, between ASCII value `97 to 122` or `'a' + (0,26)` 
```csharp
using System;

namespace LearnNumbers
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                Console.Write((char)('a' + random.Next(0,26)));
                //Console.WriteLine();
            }
        }
    }
}

```
Eg Output:
```
bkyqvyjq
```

Neatway:
```csharp
using System;

namespace LearnNumbers
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            const int passwordLength = 8;
            
            //Character buffer
            char[] buffer = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                buffer[i] = (char)('a' + random.Next(0,26)); //(char)('a' +25) -> (char)(97 + 25) -> (char)(122) -> 'y'
            }

            //eg output buffer = {'b', 'v','b', 'o','x', 'n','e', 'g'}

            //convert to string
            string password = new String(buffer);

            Console.WriteLine(password); 
            //bvboxneg

        }
    }
}

```

<br>

## Key Takeaways

* Arrays store multiple values of the same type and have fixed size.
* Strings represent text and provide many built-in methods.
* Enums represent a fixed set of named constants.
* Value Types store actual values.
* Reference Types store references to objects.
* List<T> is a dynamic collection used extensively in real applications.
* Random is used to generate random values.
* If collection size can change, prefer List<T> over arrays.


<br>

---
---
