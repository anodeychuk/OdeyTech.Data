# OdeyTech.Data

## Overview
**OdeyTech.Data** is a comprehensive C# library for managing, loading, and manipulating data models. It provides a range of interfaces, abstract classes, and data providers that can be easily integrated into your project to handle a variety of data operations, including batch operations and dependent model handling.

## Features

- Interfaces for defining basic model structure and functionality.
- Abstract classes for creating basic and dependent models.
- Base class for data grid provider and data provider.
- A set of data loader providers that implement data operations.
- Repository interfaces and abstract classes that support batch operations, dependency management, read, and write operations:
    - `IRepository<T>`: Provides basic CRUD operations.
    - `IReadableRepository<T>`: Provides read-only access to data items.
    - `IBatchRepository<T>`: Supports batch read and write operations.
    - `IDependentModelRepository<T>`: Allows handling data items with parent dependencies.
- Concrete repository classes that implement these interfaces:
    - `ModelRepository<T>`: Offers a base for repositories with read, write, and batch operations.
    - `DependentModelRepository<T>`: Abstract repository for managing dependent data items.
    - `ReadableRepository<T>`: Base repository that provides read access to data items.
    - `SqlRepository`: A base class for repositories that work with SQL databases.

## Usage

Before you can use the **OdeyTech.Data**, you will need to create your model classes implementing the `IModel` or `IDependentModel` interface. For example implement your own model by deriving from the `BasicModel` or `DependentModel` abstract classes:
~~~csharp
public class YourModel : BasicModel
{
    public YourModel() : base()
    {...}
    
    public YourModel(ulong identifier) : YourModel(identifier)
    {...}
    
    // Define your properties and methods here.
}
~~~

Then, you can use the interfaces and abstract classes provided by this library to create repositories for managing these models.

For example, to create a repository that provides batch read and write access to data items, you can implement the `IBatchRepository<T>` interface in your repository class.

If you have models that have a parent dependency, you can use the `IDependentModelRepository<T>` interface and `DependentModelRepository<T>` abstract class to manage these models.

You can also utilize `IReadableRepository<T`>, `IRepository<T>`, `ReadableRepository<T>`, and `ModelRepository<T>` for a variety of read and write operations on your data models.

Finally, if your repositories work with SQL databases, you can inherit from the `SqlRepository` abstract class to take advantage of its features and functionalities.

## Getting Started
To start using OdeyTech.Data, install it as a NuGet package in your C# project.

## Contributing
We welcome contributions to `OdeyTech.Data`! Feel free to submit pull requests or raise issues to help us improve the library.

## License
`OdeyTech.Data` is released under the [Non-Commercial License][LICENSE]. See the LICENSE file for more information.

## Stay in Touch
For more information, updates, and future releases, follow me on [LinkedIn][LIn] I'd be happy to connect and discuss any questions or ideas you might have.

[//]: #
   [LIn]: <https://www.linkedin.com/in/anodeychuk/>
   [LICENSE]: <https://github.com/anodeychuk/OdeyTech.Data/blob/main/LICENSE>