# AFS-Checkout

Api demo code for checkout.com

## Notes

Assumptions have to be made around how the acquiring bank integration works. The spec is quite vague, even open-ended on this point. It looks as if this is to be stubbed in the Payment gateway: "We will be building the payment gateway _only_". but there is also: "Build a simulator to mock the responses from the bank"

Not clear what "This component should be able to be switched out for a real bank once we move into production" means. Does it imply an out-of process mock?


Payment gateway is "Responsible for validating requests, storing card information and forwarding
payment requests and accepting payment responses to and from the acquiring bank."

## Running

The easiest way to work out how to use it is to run it and browse to `/swagger`


## HTTP Endpoints

* `GET /health` A simple "liveness check" that responds OK when the app is running.
* `POST /payment` to submit a new payment
* `GET /payment/{id}` to retrieve data on a previously submitted payment
* `GET /swagger/` Swagger UI
* `GET /swagger/v1/swagger.json` swagger data on endpoints
* `GET /metrics`  metrics for Prometheus

## Other features

* Swagger metadata and UI using [NSwag](https://github.com/RicoSuter/NSwag)

## Build features

*  Checks at compile time configured in `Directory.Build.props`
  * Warnings as errors
  * References not null by default
  * FxCopAnalyzers with rulesets
* Unit tests, and Integration tests using `TestHost`
* Build and test run on GitHub using Github Actions ( at https://github.com/AnthonySteele/AFSCheckout/actions ) and script in the repo at `\.github\workflows\build.yml`

## Todo:

The local data repo is also an in-memory fake.
No auth or merchant id.

## Limitations of this app

I didn't want to overcomplicate the data models before it is needed, so there are some DTOs that are used as both view models and database models. In a a larger app these would be separated out as it grows.

Logging is very simple. I am using the framework `ILogger` as recommended, but I haven't paid attention to how it is configured in startup.

The merchant will likely want to see a list of their recent transactions. This would means 
 * storing a merchant id on each transaction
 * repo lookup method to get a list of transactions
 * A GET endpoint

There is potential data loss if the bank API fails badly (e.g. timeout or HTTP 500 response) and we won't have a record in the local data store at that point.
Way to mitigate this likely involves:
 * Save the payment data locally _before_ sending it. This means using a locally generated id as key, not depending on the the transaction id that the bank returns.
 * make use of an [Idempotency key](https://stripe.com/docs/api/idempotent_requests) to make it safe for a client to repeat failed requests by inspecting the saved data and deciding what to do.
 * maybe returning the ID to the client with a "bank unavailable" error message. There's no failure message yet.
 
No API client. One can be generated from metadata using [NSwag](https://github.com/RicoSuter/NSwag).

No docker support. But this is not hard for an expert to add.

No performance data. This should either be gathered from Prometheus metrics on a running app, or tested at lower levels with [BenchmarkDotNet](https://benchmarkdotnet.org/).

No test coverage measurement - that can be added with [coverlet](https://www.hanselman.com/blog/NETCoreCodeCoverageAsAGlobalToolWithCoverlet.aspx).
 
Anthony Steele 
2020-05-12