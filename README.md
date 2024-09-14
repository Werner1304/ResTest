# ResTest

A simple cross-platform HTTP client built using C# and Avalonia.

## Motivation 

I was looking for a nice looking, non bloated, GUI HTTP client that works on Windows and Linux. I couldn't find one, so I made my own.

## Features

Probably missing lots of features. As this is primarily for my own use I'll implement more features when I need them.


Here's the list:

+ Works on Windows and Linux (not tested on MacOS)
+ Multiple HTTP methods supported
+ Set request body and headers
+ View response body and headers

## How to Build

### Requirements

+ .Net SDK 8

### Visual Studio

1. Open the solution file
2. In the menu bar select `Build` -> `Build Solution`

### CLI

Open your terminal and navigate to the directory containing the solution file.
Then run the following commands:

```sh
dotnet restore
dotnet build
```
