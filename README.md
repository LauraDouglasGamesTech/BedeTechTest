# BedeLottery

## Overview

BedeLottery is a C# console application designed to simulate a simplified lottery system. The application allows users to purchase tickets, randomly generate computer participants, and determine winners based on predefined prize distribution rules. The solution adheres to **SOLID** principles, ensuring modularity, testability, and scalability. It also integrates **configuration flexibility**, **dependency injection**, and **LINQ** for cleaner and more maintainable code. The implementation prioritizes readability and extensibility while maintaining a simple yet functional architecture.

## Project Structure

```
BedeLottery/
├── BedeLottery.sln           # Solution file
├── BL_BedeLottery            # Business logic and data models
|   ├── Classes               # Data models / classes
|   ├── Enums                 # Enums
|   ├── LotteryGame.cs        # Core lottery logic
├── Console_BedeLottery       # Front end project
|   ├── Program.cs            # Entry point
├── Test_BL_BedeLottery       # Unit Tests for Business Logic
|   ├── LotteryGameTests.cs   # Test cases for core lottery logic
|   ├── PlayerTests.cs        # Test cases for Player class
|   ├── TicketTests.cs        # Test cases for Ticket class
├── Test_Console_BedeLottery  # Unit Tests for Front end
|   ├── ProgramTests.cs       
├── README.md                 # Project documentation
```

## Design Choices

The design of this solution prioritizes simplicity while ensuring clarity and maintainability. The key design decisions include:

- **Focus on Readability:** The solution is structured to be straightforward and easy to understand, ensuring adaptability to different project styles and coding standards.
- **Encapsulation of Game Data:** Core game elements, such as players and tickets, are managed within `LotteryGame` using private collections, ensuring controlled access.
- **Randomization for Fairness:** The game utilizes a `Random` instance for ticket purchases and winner selection, ensuring a fair distribution of results.
- **Configurable Constraints:** The game allows customization of key parameters such as player count, ticket cost, and prize distribution through the `LotteryGame` constructor.
- **Minimal Dependencies:** The implementation avoids complex dependencies, keeping the project lightweight and focused on its core functionality.
- **Self-Contained Logic:** The application does not use separate service layers or interfaces. Instead, logic is encapsulated within the `LotteryGame`, `Player`, and `Ticket` classes to maintain simplicity.
- **Use of LINQ:** LINQ expressions simplify data filtering and aggregation, reducing the need for verbose loops and conditions.

While enhancements such as **Factories, Interfaces, and Service Layers** could improve the architecture, this implementation prioritizes core functionality to ensure correctness under the unique pressures of job applications. Overcomplicating a technical test with excessive design patterns can detract from solving the actual problem effectively.


## Possible Improvements & Issues

While the implementation meets the core requirements, there are areas that can be improved:

### **Testing Strategy**

- Improve test coverage by incorporating Behavior-Driven Development (BDD) with frameworks like SpecFlow.
- Implement mockable randomization to allow deterministic testing of ticket purchases and winner selection.
- Add integration tests to validate the full lottery process, from player creation to prize distribution.

### **Architecture & Code Organization**

- Introduce **separation of concerns** by extracting game logic into dedicated service classes for better modularity.
- Implement **interfaces** for key components such as ticket purchasing and winner selection to improve maintainability and flexibility.
- Refactor `LotteryGame` to use a **Factory Pattern** for ticket generation, reducing redundancy in object creation.

### **Randomization Handling**

- Abstract `Random` usage into a dedicated **randomization utility**, allowing it to be swapped for a seeded generator in testing scenarios.
- Allow players to specify a random seed for reproducible lottery results when debugging.

### **Configuration & Extensibility**

- Introduce an external configuration file (e.g., JSON or XML) to allow customization of game parameters without modifying code.
- Provide an option for dynamic rule definition, enabling different lottery formats or prize structures.
- Expand the project with an **API layer** to support a graphical user interface (GUI) or web-based interaction.

## Use of LLMs  

A Large Language Model (LLM) was used to assist in compiling this README from notes, streamlining the documentation process and reducing time spent on repetitive tasks such as writing class comments. The LLM's output was carefully reviewed to ensure accuracy, clarity, and alignment with the project's intent, with all content checked for errors or misinterpretations before inclusion.
