// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using DatabaseFirst.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFirst.App.Benchmark;

[RPlotExporter]
[MinColumn, MaxColumn]
[Config(typeof(Config))]
public class AsyncMethodBenchmark : Config
{
    [Benchmark]
    public IList<Person> GetPeopleNotOptmizedYet()
    {
        AdventureWorks2019Context context = new();

        return context.People.Take(500).ToList();
    }

    [Benchmark]
    public async Task<IList<Person>> GetPeopleOptmized()
    {
        AdventureWorks2019Context context = new();

        return await context.People.Take(500).ToListAsync();
    }

    [Benchmark]
    public IList<Person> GetPeople_FullFields_NotOptmizedYet()
    {
        AdventureWorks2019Context context = new();

        return context.People.Take(500).ToList();
    }

    [Benchmark]
    public async Task<IList<Person>> GetPeople_FullFields_Optmized()
    {
        AdventureWorks2019Context context = new();

        return await context.People
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.BusinessEntity).ThenInclude(y => y.BusinessEntityAddresses)
            .Take(500)
            .Select(
            x => new Person
            {
                BusinessEntity = x.BusinessEntity,
                Password = x.Password,
                FirstName = x.FirstName,
                Rowguid = x.Rowguid,
                LastName = x.LastName,
                AdditionalContactInfo = x.AdditionalContactInfo,
                BusinessEntityContacts = x.BusinessEntityContacts,
                BusinessEntityId = x.BusinessEntityId,
            }
            )
            .ToListAsync();
    }

    [Benchmark]
    public Person? Get_FirstOrDefault_NotOptmizedYet()
    {
        AdventureWorks2019Context context = new();

        return context.People.FirstOrDefault();
    }

    [Benchmark]
    public async Task<Person?> Get_FirstOrDefault_Optmized()
    {
        AdventureWorks2019Context context = new();

        return await context.People.FirstOrDefaultAsync();
    }

    [Benchmark]
    public int Get_Any_NotOptmizedYet()
    {
        AdventureWorks2019Context context = new();

        return context.People.Select(x => x.BusinessEntityId > 1).Count();
    }

    [Benchmark]
    public async Task<int?> Get_Any_Optmized()
    {
        AdventureWorks2019Context context = new();

        return await context.People
            .CountAsync(x => x.BusinessEntityId > 1);
    }
}

public class Config : ManualConfig
{
    public Config()
    {
        Add(Job.Dry); 
        //AddLogger(ConsoleLogger.Default);
        AddColumn(TargetMethodColumn.Method, StatisticColumn.StdDev);
        AddColumn(TargetMethodColumn.Method, StatisticColumn.Error);
        AddColumn(TargetMethodColumn.Method, StatisticColumn.OperationsPerSecond);
        AddDiagnoser();
        AddAnalyser(EnvironmentAnalyser.Default);
        AddEventProcessor();
        //UnionRule = ConfigUnionRule.AlwaysUseLocal;
        // You can add custom tags per each method using Columns
        //Add(new TagColumn("Action Name", name => name.Substring(3)));

    }
}
