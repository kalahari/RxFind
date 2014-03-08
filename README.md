# RxFind
This is a fork of http://rxfind.codeplex.com

RxFind is a Windows command line tool allowing you to search (and replace) the
content of files using Regular Expressions written in C# for .NET framework 2.0.

## Authors
- Original author: [Joel Thoms](http://joel.net)
- Line number patch: [Bartizan](http://www.codeplex.com/site/users/view/Bartizan)
- Recent changes: [Blake Mitchell](https://github.com/kalahari)

## Features

- __Command Line Interface__: Perfect for all your scripting and administration
  needs.
- __Regular Expressions__: That's right -- Regular Expressions. The most powerful
  way to search.
- __Search Multiple Files__: Can even traverse subdirectories.
  `RxFind code\*.cs /s /p:"Joel Thoms"` will search for the text "Joel Thoms"
  in all .cs files under the code directory.
- __Regex Replace__: Regular Expression replace gives you access to the match
  patern data. `RxFind test.txt /p:"Full Name: (?<first>\w+) (?<last>\w+)"
  /r:"Full Name: ${last}, ${first}"` will rewrite lines in test.txt from:
  "Full Name: Joel Thoms" to: "Full Name: Thoms, Joel".
- __.Bak Files__: Can create a .bak file of the original (just incase you mess
  something up)

## Options
```
C:\>RxFind.exe /?
RxFind version: 0.9.8

Performs a regular expression search, and optional replace,
across the specified files.

RXFIND [drive:][path]file
  [/S] [/F] [/LN] [/FL] [/I] [/B:0|1|2] [/SL] [/O] [/Q]
  [/DQ:doublequotetoken] (/P:searchpattern)|(/PV:searchpatternvariable)
  [/R:replacementstring]|[/RV:replacementstringvariable]

  [drive:][path]file
         Specifies drive, directory, and file(s) to search

  /?     Displays this help text.
  /S     Search recursively through subdirectories.
  /DQ    A substitue for "" in search and replace strings.
  /P     Regex search pattern.
  /PV    Name of environment variable containing rexgex search pattern.
  /R     Replacement string.
  /RV    Name of environment variable containing replacement string.
  /I     Ignore case when matching.
  /SL    Single line pattern matching.
  /F     Includes the filename in console output.
  /LN    Includes the line number in console output.
  /FL    Console output is only the matching directory and file names.
  /B:0   If .bak file exists, no replace will be performed. (default)
  /B:1   If .bak file exists, it is overwritten.
  /B:2   No .bak file is created.
  /O     Console output only, no files are modified
  /Q     Quiet operation, no results output to console");
```

## Warning
This tool loads the file into memory before performing searches. It is untested
on extremely large files. You might not be able to use this tool with extremely
large files.

## Installation
All you have to do (and this is optional) is add RxFind's path to your PATH
environment variable.

WinXP - Go to your "Control Panel" and select "System". Open the "Advanced" tab
and click on the "Environment Variables" button. In the "System variables"
section, select "Path" and click "Edit". Add the path you've extracted the
executable to into the "Variable value" section and click "OK".
