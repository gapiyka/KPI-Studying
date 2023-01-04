# Powered by
Hapii Denys Eduardovych **IP-05**

# Conway's Game of Life

The Game of Life, also known simply as Life, is a cellular automaton devised by the 
British mathematician John Horton Conway in 1970. It is a zero-player game, meaning 
that its evolution is determined by its initial state, requiring no further input. 
One interacts with the Game of Life by creating an initial configuration and observing how it evolves. 
It is Turing complete and can simulate a universal constructor or any other Turing machine.

Prepared for [*first lab*](https://cloud.comsys.kpi.ua/s/Fskj393sCcwx842?dir=undefined&path=%2FLabs&openfile=21376862)

## Installation and using

1. Install .NET (Core).

2. Clone repository
```bash
  git clone https://github.com/gapiyka/KPI-Studying/5th%20semester/Software%20quality%20and%20testing/lab-1
```

3. Build Project
```bash
  C:\..\lab-1> dotnet build -c Release .\lab1
```

4. Launch application:
	- Run app width default grid parameters
	```bash
	 C:\..\lab-1> lab1\bin\Release\netcoreapp3.1\lab1.exe
	``` 
	- Run app width grid parameters from certain txt.file
	```bash
	  C:\..\lab-1> lab1\bin\Release\netcoreapp3.1\lab1.exe C:\SomeFolder\text.txt
	``` 
  
5. Launch Tests
```bash
  C:\..\lab-1> dotnet test unit-tests/unit-tests.csproj
``` 
	
	
## File example

![image](https://user-images.githubusercontent.com/50524296/194030454-006f14f1-f6a0-4456-ad87-d238fac884a2.png)

## Pull Requests

[Passed]

![image](https://user-images.githubusercontent.com/50524296/210516068-c5ea88c8-c566-4a30-8c38-bb45519e6ca7.png)


[Failed]

![image](https://user-images.githubusercontent.com/50524296/210516168-531eda2f-44fc-4399-b1d1-fb5214cfe2b9.png)

