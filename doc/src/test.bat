@echo off

type result >> result.bak 2>NUL
echo -------------------------- >> result.bak
del result 2>NUL
rmdir /s /q tmp 2>NUL
mkdir tmp 2>NUL

set runs=10
rem "" for all, available: NaiveBayes, RandomForest
set algorithms=(NaiveBayes, RandomForest)
set datasets=("")

if NOT [%1]==[] goto %1

:java
echo Setting up java
mkdir java 2>NUL
copy /y src\Program.java java >NUL
copy /y lib\weka.jar java >NUL
copy /y data\* java >NUL
cd java
javac -cp weka.jar;. Program.java
echo Running java %time%
echo java >> ..\result
for %%a in %algorithms% do (
  echo Running %%a
  echo %%a >> ..\result
  for %%d in %datasets% do (
    echo %%d >> ..\result
    java -cp weka.jar;. Program %runs% %%a %%d >> ..\result
  )
)
echo Finished java %time%
cd ..
move java tmp 2>NUL
if NOT [%1]==[] goto finish

:javaikvm
echo Setting up javaikvm
mkdir javaikvm 2>NUL
copy /y src\Program.java javaikvm  >NUL
copy /y lib\weka.jar javaikvm  >NUL
copy /y data\* javaikvm  >NUL
cd javaikvm
javac -cp weka.jar;. Program.java
echo Running javaikvm %time%
echo javaikvm >> ..\result
for %%a in %algorithms% do (
  echo Running %%a
  echo %%a >> ..\result
  for %%d in %datasets% do (
    echo %%d >> ..\result
    D:\Desktop\WEKA\ikvm\bin\ikvm -cp weka.jar;. Program %runs% %%a %%d >> ..\result
  )
)
echo Finished javaikvm %time%
cd ..
move javaikvm tmp 2>NUL
if NOT [%1]==[] goto finish

:net
echo Setting up net
mkdir net 2>NUL
copy /y src\Program.cs net  >NUL
copy /y lib\*.dll net  >NUL
copy /y data\* net  >NUL
cd net
csc Program.cs -r:weka.dll,IKVM.OpenJDK.Core.dll,IKVM.Runtime.dll
echo Running net %time%
echo net >> ..\result
for %%a in %algorithms% do (
  echo Running %%a
  echo %%a >> ..\result
  for %%d in %datasets% do (
    echo %%d >> ..\result
    Program.exe %runs% %%a %%d >> ..\result
  )
)
echo Finished net %time%
cd ..
move net tmp 2>NUL
if NOT [%1]==[] goto finish

:mono
echo Setting up mono
mkdir mono 2>NUL
copy /y src\Program.cs mono  >NUL
copy /y lib\*.dll mono  >NUL
copy /y data\* mono  >NUL
cd mono
cmd /c "dmcs Program.cs -r:weka.dll,IKVM.OpenJDK.Core.dll,IKVM.Runtime.dll"
echo Running mono %time%
echo mono >> ..\result
for %%a in %algorithms% do (
  echo Running %%a
  echo %%a >> ..\result
  for %%d in %datasets% do (
    echo %%d >> ..\result
    mono Program.exe %runs% %%a %%d >> ..\result
  )
)
echo Finished mono %time%
cd ..
move mono tmp 2>NUL
if NOT [%1]==[] goto finish

:fsharp
echo Setting up fsharp
mkdir fsharp 2>NUL
copy /y src\Program.fsx fsharp  >NUL
copy /y lib\wekasharp\*.dll fsharp  >NUL
copy /y data\* fsharp  >NUL
cd fsharp
fsc Program.fsx -r:weka.dll -r:IKVM.OpenJDK.Core.dll -r:IKVM.Runtime.dll
echo Running fsharp %time%
echo fsharp >> ..\result
for %%a in %algorithms% do (
  echo Running %%a
  echo %%a >> ..\result
  for %%d in %datasets% do (
    echo %%d >> ..\result
    Program.exe %runs% %%a %%d >> ..\result
  )
)
echo Finished fsharp %time%
cd ..
move fsharp tmp 2>NUL
if NOT [%1]==[] goto finish

:finish
type result
