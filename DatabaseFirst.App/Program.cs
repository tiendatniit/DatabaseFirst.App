// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using DatabaseFirst.App;

BenchmarkRunner.Run<BenchmarkPerson>();
Console.WriteLine("Hello, World!");
Console.ReadLine();
