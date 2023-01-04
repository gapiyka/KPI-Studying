# Powered by
Hapii Denys Eduardovych **IP-05**

# Tetris

A simple tetris application, which consist of 3 layers:

"logical" - here the logic of the game is implemented, 
where you operate with abstractions that you created 
(the playing field, the code related to the rules of the game, etc.)

"input/output" - here the communication of your program with the external 
environment is implemented, where you work with the provided abstractions 
(standard output, file system)

"communication" - a layer that provides communication between "logic" and "input/output". 
In this layer, the template "DI (dependency injection)" should be implemented, 
which will allow replacing the input/output layer in the test environment.

Prepared for [*third lab*](https://docs.google.com/document/d/1um-m1uiUmloMqlXREwsykOuPLAFSXm1nskaw_jh5PI0/edit?usp=sharing)

## Installation and using

1. Install .NET (Core).

2. Clone repository
```bash
  git clone https://github.com/gapiyka/KPI-Studying/5th%20semester/Software%20quality%20and%20testing/lab-3
```

3. Build Project
```bash
  C:\..\lab-3> dotnet build -c Release .\lab3
```

4. Launch application:
	- Run app with file in default test parameters
	```bash
	 C:\..\lab-3> lab3\bin\Release\netcoreapp3.1\lab3.exe
	``` 
	- Run app with parameters from certain txt.file
	```bash
	  C:\..\lab-3> lab3\bin\Release\netcoreapp3.1\lab3.exe C:\SomeFolder\text.txt
	``` 
  
5. Launch Tests
```bash
  C:\..\lab-3> dotnet test unit-tests/unit-tests.csproj
``` 
	
  
## Tests Coverage

Tests code coverage by VS-extension `Final Code Coverage`.
![image](https://user-images.githubusercontent.com/50524296/203790300-0e4f3356-e676-46f0-8191-cb140adf12c7.png)
