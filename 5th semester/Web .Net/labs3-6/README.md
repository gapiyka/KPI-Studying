# Lab 3
Hapii Denys Eduardovych **IP-05**

Introduce with the multi-level architecture of software systems
The purpose of the laboratory work is to learn how to build software with multi-level architecture,
to be able to design and implement content its individual levels.

## Task

To design and develop a software system consisting of three
components-levels of a multi-level architecture. Develop further
module tests for system functionality testing.
The first component is the data access layer (DAL). Data source
choose a relational database. Access to data is organized using
Entity Framework code first. DAL organize with use
of the "Unit of Work" pattern, which combines several repositories
according to the subject area. Data is being written and read
methods of repositories. The SQL language in DAL programming is not
is used The project type is a dynamic library.

The second component is the business logic level (BLL), which is implemented
functionality (business logic) of the software system for the subject field
according to the option. The BLL references the DAL to access the
repositories for writing and reading data in the database. Project type –
dynamic library. To ensure a weak connection between
layers use IoC containers.

The third component is the presentation layer (PL) and includes the interface
software system. As a given component in the current
laboratory work is suggested to use console or graphics
application. PL refers to BLL to access the
business logic operations when performing user requests: that is
methods of the view layer, when executing a query, call certain
methods of business logic level services.

Prepared for [*third lab*](https://drive.google.com/file/d/1kumakAqVUn1qWnJBViCNSQP4nciYqQX8/view?usp=sharing)

## Variant

The registry provides data on the availability of doctors and the schedule
reception of patients. Patients can make an appointment before
the doctor, if there is free time in the doctor's schedule. In the registry office
records of visits by patients to the hospital are kept, in which
the time of the doctor's visit, the diagnosis and the doctor are recorded
put
**Functional requirements:** hospital file management and
management of patients' appointments with doctors

*Sanitas Libr* (from Latin) -- health book.

## Installation and using

1. Install .NET (Core).

2. Clone repository
```bash
  git clone https://github.com/gapiyka/KPI-Studying/5th%20semester/Web%20.Net/labs3-6
```

3. Build Project
```bash
  C:\..\labs3-6> dotnet build -c Release .\SanitasLibr
```

4. Launch application:
```bash
  C:\..\labs3-6> SanitasLibr\bin\Release\netcoreapp3.1\SanitasLibr.exe
``` 
  
5. Launch Tests
```bash
  C:\..\labs3-6> dotnet test unit-tests/unit-tests.csproj
``` 