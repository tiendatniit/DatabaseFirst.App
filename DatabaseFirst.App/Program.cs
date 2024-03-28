// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using DatabaseFirst.App.Benchmark;

//BenchmarkRunner.Run<BenchmarkPerson>();
//BenchmarkRunner.Run<AsNoTrackingBenchmark>();
//BenchmarkRunner.Run<QueryPerformanceBenchmark>();
//BenchmarkRunner.Run<UsingRawQueryBenchmark>();
//BenchmarkRunner.Run<NSelectQueryBenchmark>();
BenchmarkRunner.Run<BulkQueryBenchmark>();
//BenchmarkRunner.Run<CompiledQueryBenchmark>();

Console.WriteLine("Hello, World!");
Console.ReadLine();
