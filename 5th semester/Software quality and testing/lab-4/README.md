# Powered by
Hapii Denys Eduardovych **IP-05**

# Tetris + Mocks

It is necessary to develop the program made in laboratory No. 3 

and add a parameter when transferring which will display not only the last screen game, 

but also screens at each step of the game. At the same time, 

it is necessary to save the initial mode of operation (when only the last screen is displayed).

Prepared for [*fourth lab*](https://docs.google.com/document/d/1-mleCcoG0Itvs5-YbnkHdW-8-FiKnkYNWpIw6H9W2q0/edit#)

## Installation and using

1. Install .NET (Core).

2. Clone repository
```bash
  git clone https://github.com/gapiyka/KPI-Studying/5th%20semester/Software%20quality%20and%20testing/lab-4
```

3. Build Project
```bash
  C:\..\lab-4> dotnet build -c Release .\lab4
```

4. Launch application:

  `-s` flag to show steps.
  
	- Run app with file in default test parameters
	```bash
	 C:\..\lab-4> lab4\bin\Release\netcoreapp3.1\lab4.exe
	``` 
  
	- Run app with parameters from certain txt.file
	```bash
	  C:\..\lab-4> lab4\bin\Release\netcoreapp3.1\lab4.exe -s C:\SomeFolder\text.txt
	``` 
  
5. Launch Tests
```bash
  C:\..\lab-4> dotnet test unit-tests/unit-tests.csproj
``` 
	
  
## Tests Coverage

Tests code coverage by VS-extension `Final Code Coverage`.
![image](https://user-images.githubusercontent.com/50524296/203925030-c8c6b263-f832-41a1-998c-8804fce5ae50.png)
