// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DatabaseFirst.App.Benchmark;

//BenchmarkRunner.Run<BenchmarkPerson>();
//BenchmarkRunner.Run<AsNoTrackingBenchmark>();
//BenchmarkRunner.Run<QueryPerformanceBenchmark>();
//BenchmarkRunner.Run<UsingRawQueryBenchmark>();
//BenchmarkRunner.Run<NSelectQueryBenchmark>();
//BenchmarkRunner.Run<BulkQueryBenchmark>();
//BenchmarkRunner.Run<CompiledQueryBenchmark>();
//BenchmarkRunner.Run<ThisOrThatBenchmark>();
//BenchmarkRunner.Run<DataTransferBenchmark>();

//BenchmarkRunner.Run<FindOrFirstAsyncBenchmark>();
//BenchmarkRunner.Run<SingleOrDefaultAsyncBenchmark>();
//BenchmarkRunner.Run<AnyOrCountAsyncBenchmark>();
BenchmarkRunner.Run<DataTransferBenchmark>();

Console.WriteLine("Hello, World!");
Console.ReadLine();
