``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]   : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT
  ShortRun : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                     Method |        Mean |       Error |      StdDev |  Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |   Allocated |
|--------------------------- |------------:|------------:|------------:|-------:|--------:|----------:|---------:|------:|------------:|
| SimulateAAppWithoutSerilog |    247.7 μs |    170.4 μs |     9.34 μs |   1.00 |    0.00 |   24.9023 |   3.9063 |     - |   128.29 KB |
| SimulateAAppWithSerilogOff |  1,428.2 μs |    413.8 μs |    22.68 μs |   5.77 |    0.15 |  208.9844 |   1.9531 |     - |  1071.64 KB |
|  SimulateAAppWithSerilogOn | 81,049.5 μs | 47,813.0 μs | 2,620.79 μs | 327.70 |   22.28 | 5428.5714 | 142.8571 |     - | 28336.64 KB |