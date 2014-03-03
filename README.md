# RxFind

RxFind is a Windows command line tool allowing you to search (and replace) the content of files using Regular Expressions written in C# for .NET framework 2.0.

## Additional Info
Author: Joel Thoms
HomePage: http://joel.net

## Features

- Command Line Interface - Perfect for all your scripting and administration needs.
- Regular Expressions - That's right -- Regular Expressions. The most powerful way to search.
- Search Multiple Files - Can even traverse subdirectories.
- Regex Replace - Regular Expression replace gives you access to the match patern data.
- .Bak Files - Can create a .bak file of the original (just incase you mess something up)

## Options
```
C:\>rxfind.exe /?
Performs a search and/or replace in a directory.

RXFIND [drive:][path]filename [/S] [/F] [/I] [/B:0|1|2] [/SL] [/O] [/Q] /P:[searchpattern] [/R:replacestring]

  [drive:][path]filename
              Specifies drive, directory, and files to list.

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
This tool loads the file into memory before performing searches. It is untested on extremely large files. You might not be able to use this tool with extremely large files.

## Installation
All you have to do (and this is optional) is add RxFind's path to your PATH environment variable.

WinXP - Go to your "Control Panel" and select "System". Open the "Advanced" tab and click on the "Environment Variables" button. In the "System variables" section, select "Path" and click "Edit". Add the path you've extracted the executable to into the "Variable value" section and click "OK".
