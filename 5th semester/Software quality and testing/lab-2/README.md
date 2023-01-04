# Powered by
Hapii Denys Eduardovych **IP-05**

# Calculator

A simplified keyboard calculator. Calculator keys:

- "0" ... "9" — entering a number

- "+", "-", "*", "/" — performing an arithmetic operation on whole numbers

- "=" — performing an operation 

To simplify the execution of the task, we introduce the following restrictions, which are absent from a regular calculator: 

- calculations are performed on whole numbers (int, not float, not double), 

- the user can only enter a sequence of numbers, then an operation, then numbers again, then "=". Any other key sequence is prohibited

Prepared for [*second lab*](https://cloud.comsys.kpi.ua/s/Fskj393sCcwx842?dir=undefined&path=%2FLabs&openfile=23344151)

## Installation and using

1. Install .NET (Core).

2. Clone repository
```bash
  git clone https://github.com/gapiyka/KPI-Studying/5th%20semester/Software%20quality%20and%20testing/lab-2
```

3. Build Project
```bash
  C:\..\lab-2> dotnet build -c Release .\lab2
```

4. Launch application:
	- Run app with file in default test parameters
	```bash
	 C:\..\lab-2> lab2\bin\Release\netcoreapp3.1\lab2.exe
	``` 
	- Run app with parameters from certain txt.file
	```bash
	  C:\..\lab-2> lab2\bin\Release\netcoreapp3.1\lab2.exe C:\SomeFolder\text.txt
	``` 
  
5. Launch Tests
```bash
  C:\..\lab-2> dotnet test unit-tests/unit-tests.csproj
``` 
	
	
## File example

![image](https://user-images.githubusercontent.com/50524296/194217992-0190eb18-3d56-4307-8c6b-528f4c71b4c5.png)

## Tests Coverage

Tests code coverage by VS-extension `Final Code Coverage`.
![image](https://user-images.githubusercontent.com/50524296/194218316-21928b6d-85af-4bb2-82ea-312d426d1fcb.png)

I think this result is ok (`Main` function is not covered):
![image](https://user-images.githubusercontent.com/50524296/194218393-fa6b9212-8212-4bed-b55f-9f41066e78a6.png)
![image](https://user-images.githubusercontent.com/50524296/194218524-be0be12a-0778-4977-b052-63723a643e54.png)

