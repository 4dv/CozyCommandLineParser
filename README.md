# Cozy Command Line Parser
The project is intended to make adding CLI to your program as easy and cozy as possible. All you have to do, just:

1. Add `[Command]` attribute to the methods you want to work as commands
2. Create new `CommandLine` instance 
3. Run its `Execute` method with args from your Main.
 
 Everything else will be done by CCLP:
* It finds all methods which have `Command` attribute
* Checks that not more than one method corresponds to the same command. Otherwise throws `CommandLineExcpetion`
* Finds which method matches provided command
* Creates an instance of a class where the method is defined
* If the class has properties with `Option` attribute and user provides named arguments, fills those properties with arguments, converting to expected type if it is int, double, etc
* If the method has arguments fill them with positional command-line arguments
* Or use default values it specified
* Executes the command 
* And prints returned value to console, unfolding enumerable (can be disabled, see `ParserOptions.OutputPrinter`)

## Install
Install with 
```
Install-Package CozyCommandLineParser
```

Nuget is here: https://www.nuget.org/packages/CozyCommandLineParser/

## Examples
In the following examples lets assume you have a program named **prog** and want to add some commands to it. The simplest way to add a couple of commands will be:
```C#
 class Program
 {
     // executed on `prog sayHi`
     [Command]
     public void SayHi()
     {
         Console.WriteLine("Hi there");
     }
    
     // executed on `prog sayBuy`
     [Command]
     public void SayBuy()
     {
         Console.WriteLine("Buy there");
     }
    
     static void Main(string[] args)
     {
         new CommandLine().Execute(args);
     }
 }
```
It will add the following commands:
* `prog sayHi` SayHi method is executed. 
* `prog sayBuy` SayBuy method is called.
* `prog` or `prog help` or `prog -h` or `prog --help` help is printed
* `prog version` or `prog --version` program version is printed 

Docs are not finished, meanwhile you can find more examples [here](SampleProject/Commands.cs) 
 
## Current limitations
* CCLP is centered around the command concept like it is done in git. You can have one default command, then if the method is not specified it will be called. But it is still considered as a command so you can also call it directly by name. 
* Commands should be instance methods, static methods with `Command` attribute are ignored
