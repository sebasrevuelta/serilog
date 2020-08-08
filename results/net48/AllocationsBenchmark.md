``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT
  DefaultJob : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT


```
|               Method |          Mean |       Error |      StdDev |    Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |--------------:|------------:|------------:|---------:|--------:|-------:|------:|------:|----------:|
|             LogEmpty |      8.982 ns |   0.2017 ns |   0.2760 ns |     1.00 |    0.00 |      - |     - |     - |         - |
| LogEmptyWithEnricher |     61.517 ns |   1.2511 ns |   2.0203 ns |     6.85 |    0.32 | 0.0052 |     - |     - |      28 B |
|               LogMsg |  1,443.274 ns |  27.0997 ns |  27.8294 ns |   159.79 |    6.26 | 0.0153 |     - |     - |      84 B |
|         LogMsgWithEx |  1,446.306 ns |  23.9057 ns |  22.3614 ns |   159.97 |    5.76 | 0.0153 |     - |     - |      84 B |
|           LogScalar1 |  1,537.964 ns |  26.6350 ns |  24.9144 ns |   170.13 |    6.64 | 0.0401 |     - |     - |     216 B |
|           LogScalar2 |  1,605.484 ns |  24.7219 ns |  23.1249 ns |   177.60 |    6.80 | 0.0458 |     - |     - |     240 B |
|           LogScalar3 |  1,661.809 ns |  24.8785 ns |  23.2714 ns |   183.81 |    6.44 | 0.0496 |     - |     - |     264 B |
|        LogScalarMany |  1,696.916 ns |  24.1120 ns |  22.5543 ns |   187.72 |    7.33 | 0.0687 |     - |     - |     369 B |
|     LogScalarStruct1 |  1,570.161 ns |  20.4875 ns |  19.1640 ns |   173.68 |    6.29 | 0.0420 |     - |     - |     228 B |
|     LogScalarStruct2 |  1,658.559 ns |  23.5696 ns |  22.0470 ns |   183.46 |    6.84 | 0.0496 |     - |     - |     264 B |
|     LogScalarStruct3 |  1,743.538 ns |  25.0873 ns |  23.4667 ns |   192.85 |    6.72 | 0.0572 |     - |     - |     300 B |
|  LogScalarStructMany |  1,816.306 ns |  23.2167 ns |  21.7169 ns |   200.94 |    8.02 | 0.0782 |     - |     - |     417 B |
|   LogScalarBigStruct |  1,669.383 ns |  22.8531 ns |  21.3768 ns |   184.66 |    6.67 | 0.0515 |     - |     - |     272 B |
|        LogDictionary |  4,306.657 ns |  82.6295 ns | 104.4999 ns |   478.98 |   15.55 | 0.2441 |     - |     - |    1294 B |
|          LogSequence |  2,212.909 ns |  43.2084 ns |  54.6448 ns |   246.11 |    7.99 | 0.0839 |     - |     - |     453 B |
|         LogAnonymous |  6,874.079 ns | 105.7125 ns |  98.8835 ns |   760.39 |   28.83 | 0.3586 |     - |     - |    1915 B |
|              LogMix2 |  1,613.243 ns |  27.2060 ns |  25.4485 ns |   178.45 |    6.91 | 0.0477 |     - |     - |     252 B |
|              LogMix3 |  1,716.813 ns |  33.5548 ns |  32.9553 ns |   189.74 |    8.35 | 0.0553 |     - |     - |     292 B |
|              LogMix4 |  1,758.425 ns |  21.9287 ns |  20.5121 ns |   194.50 |    6.83 | 0.0801 |     - |     - |     421 B |
|              LogMix5 |  1,851.703 ns |  18.4929 ns |  17.2983 ns |   204.81 |    6.94 | 0.0858 |     - |     - |     457 B |
|           LogMixMany | 12,177.057 ns | 241.0979 ns | 225.5231 ns | 1,346.94 |   51.81 | 0.7019 |     - |     - |    3702 B |