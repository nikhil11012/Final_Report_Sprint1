# Practice Assignment 1 – Calculator Library with Unit Tests

## Overview

This project is a basic .NET solution that demonstrates how to build a class library and write unit tests for it. It contains two projects:

- **CalculatorLib** – A class library with a `Calculator` class that performs basic arithmetic operations.
- **CalculatorLib.Tests** – A unit test project using the NUnit framework to test all calculator operations.

---

## Project Structure

```
Practice Assignment 1/
├── CalculatorApp.slnx               # Solution file
├── CalculatorLib/
│   ├── CalculatorLib.csproj         # Library project file
│   └── Calculator.cs                # Calculator class
└── CalculatorLib.Tests/
    ├── CalculatorLib.Tests.csproj   # Test project file
    └── CalculatorTests.cs           # NUnit unit tests
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download) (preview or later)

To check your installed version, run:

```bash
dotnet --version
```

---

## How to Run

### 1. Restore Dependencies

Open a terminal in the project root folder and run:

```bash
dotnet restore CalculatorApp.slnx
```

This downloads all required NuGet packages (NUnit, test adapters, etc.).

### 2. Build the Solution

```bash
dotnet build CalculatorApp.slnx
```

### 3. Run the Unit Tests

```bash
dotnet test CalculatorApp.slnx --verbosity normal
```

You should see output like:

```
Test summary: total: 12, failed: 0, succeeded: 12, skipped: 0
Build succeeded
```

---

## Calculator Class

Located in `CalculatorLib/Calculator.cs`, the `Calculator` class exposes four public methods:

| Method | Description | Example |
|--------|-------------|---------|
| `Add(a, b)` | Returns the sum of two numbers | `Add(5, 3)` → `8` |
| `Subtract(a, b)` | Returns the difference | `Subtract(10, 4)` → `6` |
| `Multiply(a, b)` | Returns the product | `Multiply(4, 5)` → `20` |
| `Divide(a, b)` | Returns the quotient; throws `DivideByZeroException` if `b == 0` | `Divide(10, 2)` → `5` |

---

## Unit Tests

Located in `CalculatorLib.Tests/CalculatorTests.cs`, the tests are written using **NUnit** and cover:

| Test | What It Verifies |
|------|-----------------|
| `Add_TwoPositiveNumbers_ReturnsCorrectSum` | Basic addition |
| `Add_WithZero_ReturnsSameNumber` | Adding zero |
| `Add_TwoNegativeNumbers_ReturnsCorrectSum` | Negative number addition |
| `Subtract_TwoPositiveNumbers_ReturnsCorrectDifference` | Basic subtraction |
| `Subtract_WithZero_ReturnsSameNumber` | Subtracting zero |
| `Subtract_ResultIsNegative` | Negative result from subtraction |
| `Multiply_TwoPositiveNumbers_ReturnsCorrectProduct` | Basic multiplication |
| `Multiply_WithZero_ReturnsZero` | Multiply by zero |
| `Multiply_WithNegativeNumber_ReturnsNegativeResult` | Negative multiplication |
| `Divide_TwoPositiveNumbers_ReturnsCorrectQuotient` | Basic division |
| `Divide_ByZero_ThrowsDivideByZeroException` | Exception on divide by zero |
| `Divide_ZeroByNumber_ReturnsZero` | Zero divided by a number |

**Total: 12 tests — all passing.**

---

## Technologies Used

- **Language:** C# (.NET 10)
- **Testing Framework:** NUnit 4.x
- **Test Runner:** NUnit3TestAdapter + Microsoft.NET.Test.Sdk
- **Code Coverage:** coverlet.collector
