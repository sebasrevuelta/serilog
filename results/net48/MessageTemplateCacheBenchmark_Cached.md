``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]   : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT
  ShortRun : .NET Framework 4.8 (4.8.4180.0), X86 LegacyJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|     Method | Items | MaxDegreeOfParallelism |        Mean |     Error |    StdDev | Ratio | RatioSD |
|----------- |------ |----------------------- |------------:|----------:|----------:|------:|--------:|
| **Dictionary** |    **10** |                     **-1** |   **409.58 μs** | **188.20 μs** | **10.316 μs** |  **1.00** |    **0.00** |
|  Hashtable |    10 |                     -1 |    29.85 μs |  25.23 μs |  1.383 μs |  0.07 |    0.00 |
| Concurrent |    10 |                     -1 |    29.99 μs |  10.05 μs |  0.551 μs |  0.07 |    0.00 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |    **10** |                      **1** |    **87.82 μs** |  **19.73 μs** |  **1.081 μs** |  **1.00** |    **0.00** |
|  Hashtable |    10 |                      1 |    85.68 μs |  63.96 μs |  3.506 μs |  0.98 |    0.03 |
| Concurrent |    10 |                      1 |    86.26 μs |  86.78 μs |  4.757 μs |  0.98 |    0.07 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |    **20** |                     **-1** |   **688.57 μs** | **194.56 μs** | **10.664 μs** |  **1.00** |    **0.00** |
|  Hashtable |    20 |                     -1 |    42.79 μs |  32.68 μs |  1.791 μs |  0.06 |    0.00 |
| Concurrent |    20 |                     -1 |    42.17 μs |  34.52 μs |  1.892 μs |  0.06 |    0.00 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |    **20** |                      **1** |   **170.99 μs** |  **66.00 μs** |  **3.618 μs** |  **1.00** |    **0.00** |
|  Hashtable |    20 |                      1 |   146.91 μs |  92.65 μs |  5.078 μs |  0.86 |    0.03 |
| Concurrent |    20 |                      1 |   157.64 μs |  66.22 μs |  3.630 μs |  0.92 |    0.04 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |    **50** |                     **-1** | **1,080.81 μs** | **699.08 μs** | **38.319 μs** |  **1.00** |    **0.00** |
|  Hashtable |    50 |                     -1 |    85.43 μs |  60.55 μs |  3.319 μs |  0.08 |    0.00 |
| Concurrent |    50 |                     -1 |    82.65 μs |  55.53 μs |  3.044 μs |  0.08 |    0.00 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |    **50** |                      **1** |   **413.55 μs** |  **92.66 μs** |  **5.079 μs** |  **1.00** |    **0.00** |
|  Hashtable |    50 |                      1 |   396.15 μs | 141.95 μs |  7.781 μs |  0.96 |    0.03 |
| Concurrent |    50 |                      1 |   385.71 μs | 128.54 μs |  7.046 μs |  0.93 |    0.02 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |   **100** |                     **-1** | **1,747.05 μs** | **211.12 μs** | **11.572 μs** |  **1.00** |    **0.00** |
|  Hashtable |   100 |                     -1 |   169.96 μs |  36.15 μs |  1.982 μs |  0.10 |    0.00 |
| Concurrent |   100 |                     -1 |   158.49 μs |  81.77 μs |  4.482 μs |  0.09 |    0.00 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |   **100** |                      **1** |   **832.56 μs** | **208.76 μs** | **11.443 μs** |  **1.00** |    **0.00** |
|  Hashtable |   100 |                      1 |   763.14 μs | 210.46 μs | 11.536 μs |  0.92 |    0.02 |
| Concurrent |   100 |                      1 |   757.03 μs | 291.32 μs | 15.968 μs |  0.91 |    0.03 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |  **1000** |                     **-1** | **1,788.41 μs** | **228.74 μs** | **12.538 μs** |  **1.00** |    **0.00** |
|  Hashtable |  1000 |                     -1 |   162.13 μs |  91.48 μs |  5.014 μs |  0.09 |    0.00 |
| Concurrent |  1000 |                     -1 |   159.50 μs |  29.63 μs |  1.624 μs |  0.09 |    0.00 |
|            |       |                        |             |           |           |       |         |
| **Dictionary** |  **1000** |                      **1** |   **834.85 μs** | **151.00 μs** |  **8.277 μs** |  **1.00** |    **0.00** |
|  Hashtable |  1000 |                      1 |   765.66 μs | 253.91 μs | 13.918 μs |  0.92 |    0.02 |
| Concurrent |  1000 |                      1 |   766.49 μs | 259.72 μs | 14.236 μs |  0.92 |    0.03 |