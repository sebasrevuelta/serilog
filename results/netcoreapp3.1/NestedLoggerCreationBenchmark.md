``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.301
  [Host]     : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT


```
|           Method |     Mean |    Error |   StdDev |   Median |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|---------:|---------:|---------:|-------:|------:|------:|----------:|
|    ForContextInt | 85.48 ns | 2.723 ns | 7.634 ns | 82.56 ns | 0.0242 |     - |     - |     152 B |
| ForContextString | 49.49 ns | 0.980 ns | 1.204 ns | 49.01 ns | 0.0204 |     - |     - |     128 B |
|   ForContextType | 85.76 ns | 1.302 ns | 1.218 ns | 85.87 ns | 0.0204 |     - |     - |     128 B |