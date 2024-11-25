using System;

public class MyCustomException : Exception
{
    public MyCustomException() : base("A custom exception occurred.") { }

    public MyCustomException(string message) : base(message) { }

    public MyCustomException(string message, Exception innerException) 
        : base(message, innerException) { }
}