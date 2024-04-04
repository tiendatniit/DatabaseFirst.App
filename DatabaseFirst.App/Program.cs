// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using DatabaseFirst.App.Benchmark;


BenchmarkRunner.Run<QueryPerformanceBenchmark>();
//BenchmarkRunner.Run<NSelectQueryBenchmark>();
//BenchmarkRunner.Run<UsingRawQueryBenchmark>();
//BenchmarkRunner.Run<BulkQueryBenchmark>();
//BenchmarkRunner.Run<DataTransferBenchmark>();

Console.WriteLine("Hello, World!");
Console.ReadLine();
