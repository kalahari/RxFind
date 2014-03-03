# RxFind
This is a fork of http://rxfind.codeplex.com

RxFind is a Windows command line tool allowing you to search (and replace) the
content of files using Regular Expressions written in C# for .NET framework 2.0.

## Authors
- Original author: [Joel Thoms](http://joel.net)
- Line number patch: [Bartizan](http://www.codeplex.com/site/users/view/Bartizan)

## Features

- Command Line Interface - Perfect for all your scripting and administration
  needs.
- Regular Expressions - That's right -- Regular Expressions. The most powerful
  way to search.
- Search Multiple Files - Can even traverse subdirectories.
  `RxFind code\*.cs /s /p:"Joel Thoms"` will search for the text "Joel Thoms"
  in all .cs files in the code directory.
- Regex Replace - Regular Expression replace gives you access to the match
  patern data. `RxFind test.txt /p:"Full Name: (?<first>\w+) (?<last>\w+)"
  /r:"Full Name: ${last}, ${first}"` will rewrite lines in test.txt from:
  "Full Name: Joel Thoms" to: "Full Name: Thoms, Joel".
- .Bak Files - Can create a .bak file of the original (just incase you mess
  something up)

## Options
```
C:\>RxFind.exe /?
Performs a search and/or replace in a directory.

RXFIND [drive:][path]filename [/S] [/F] [/LN] [/I] [/B:0|1|2] [/SL] [/O] [/Q]
  /P:[searchpattern] [/R:replacestring]

  [drive:][path]filename
              Specifies drive, directory, and file(s) to search.

  /?          Displays this help file.
  /S          Search this directory and all subdirectories
  /P          Regex search pattern
  /R          Replacement string
  /I          Ignore case
  /SL         Single line pattern matching
  /F          Includes the filename in console output
  /LN         Includes the line number in console output
  /B:0        if .bak file exists no replace will be performed. (default)
  /B:1        if .bak file exists, it is overwritten
  /B:2        No .bak file is created
  /O          Console output only.  File is not modified.
  /Q          Quiet operation - No results output to console
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
