# Refit Performance

Refit is a framework for easily calling Web API endpoints from C#.

This project intends to answer the question:

## Question

What is the performance overhead added by Refit?

## Answer

`Refit` is adding an overhead of around half a millisecond.

## Implementation

### The Server

`RefitPerformance.WebApi` - The web server containing two endpoints:

- `POST /dummy/{id}/test1`
  - Endpoint with a model populated from request body.
- `POST /dummy/{id}/test2`
  - Request type: `multipart/form-data`
  - Model populated from the request parts.

### The Client

`RefitPerformance.Client.Cli` - A console application to manually call the two endpoints.

### The Benchmark

`RefitPerformance.Benchmark` - A console application that uses `BenchmarkDotNet` to call the two endpoints with and without using `Refit`. It results 4 benchmark tests:

- Call the normal endpoint without `Refit`.
- Call the normal endpoint with `Refit`.
- Call the multipart endpoint without `Refit`.
- Call the multipart endpoint with `Refit`.