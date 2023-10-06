```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 11 (10.0.22621.2283/22H2/2022Update/SunValley2)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 7.0.400
  [Host]     : .NET 6.0.22 (6.0.2223.42425), X64 RyuJIT AVX2
  Job-FHBYDI : .NET 6.0.22 (6.0.2223.42425), X64 RyuJIT AVX2

IterationCount=200  LaunchCount=3  

```
| Method                       | Mean     | Error     | StdDev    | Median   |
|----------------------------- |---------:|----------:|----------:|---------:|
| NormalEndpoint_HttpClient    | 5.720 ms | 0.0358 ms | 0.2591 ms | 5.641 ms |
| NormalEndpoint_Refit         | 6.160 ms | 0.0366 ms | 0.2639 ms | 6.075 ms |
| MultipartEndpoint_HttpClient | 5.757 ms | 0.0355 ms | 0.2547 ms | 5.674 ms |
| MultipartEndpoint_Refit      | 6.293 ms | 0.0403 ms | 0.2877 ms | 6.197 ms |
