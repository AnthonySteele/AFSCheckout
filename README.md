# AFS-Checkout

Api demo code for Checkout.com

## Notes

Assumptions have to be made around how the acquiring bank integration works. It looks as if this is to be stubbed in the Payment gateway: "We will be building the payment gateway _only_".

## Running

The easiest way to work out how to use it is to run it and browse to `/swagger`
Or check the sample requests in the file `\requests.http`

## HTTP Endpoints

* `GET /health` A simple "liveness check" that responds OK when the app is running.
* `POST /payment` to submit a new payment
* `GET /payment/{id}` to retrieve data on a previously submitted payment
* `GET /swagger/` Swagger UI
* `GET /swagger/v1/swagger.json` swagger data on endpoints
* `GET /metrics`  metrics for Prometheus

## Other features

* Swagger metadata and UI using [NSwag](https://github.com/RicoSuter/NSwag)
* Prometheus metrics using `prometheus-net.AspNetCore`

## Build features

* C# 8, and references not null by default
* Checks at compile time configured in `Directory.Build.props`
* Warnings as errors
* FxCopAnalyzers with rulesets
* Unit tests, and Integration tests using `TestHost`
* Build and test run on GitHub using Github Actions ( at https://github.com/AnthonySteele/AFSCheckout/actions ) and script in the repo at `\.github\workflows\build.yml`

## Limitations of this app

The local payment data repository is also an in-memory fake like the bank.

I didn't want to overcomplicate the data models before it is needed, so there are some DTOs that are used as both view models and database models. In a a larger app these would be separated out as it grows and the requirements start to differ.

Logging is very simple. I am using [the framework `ILogger`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) as is standard, but I haven't paid attention to how it is configured in startup.

The merchant will likely want to see a list of their recent transactions. This would mean:

* storing a merchant id on each transaction, with validation
* repo lookup method to get a list of transactions
* A GET endpoint

What happens when the bank API fails badly (e.g. timeout or HTTP 500 response)?  We have a record in the local data store at that point, but that's all. Way to deal with this well this likely involves:

* Save the payment data locally _before_ sending it (this is done).
* make use of an [Idempotency key](https://stripe.com/docs/api/idempotent_requests) to make it safe for a client to repeat failed requests by inspecting the saved data and deciding what to do.

No API client. One can be generated from metadata using [NSwag](https://github.com/RicoSuter/NSwag).

No docker support. But this is not hard for an expert to add.

No performance data. This should either be gathered from Prometheus metrics on a running app, or tested at lower levels with [BenchmarkDotNet](https://benchmarkdotnet.org/).

No test coverage measurement - that can be added with [coverlet](https://www.hanselman.com/blog/NETCoreCodeCoverageAsAGlobalToolWithCoverlet.aspx).

No authentication / authorisation of clients.

Anthony Steele
2020-05

