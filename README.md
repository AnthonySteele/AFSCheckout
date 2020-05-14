# AFS-Checkout

Api demo code for checkout.com

## Notes

The spec is quite vague, even open-ended and therefore assumptions have to be made. 
Particularly around how the acquiring bank integration works. It looks as if this is to be stubbed in the Payment gateway?

An API that only forwards requests to another API is of course perfectly useless and should not be done; 
or if the routing is important, done in an automated way with an API Gateway product.

Payment gateway is "Responsible for validating requests, storing card information and forwarding
payment requests and accepting payment responses to and from the acquiring bank."

"We will be building the payment gateway only"



## HTTP Endpoints

* `GET /health` A simple "liveness check" that responds OK when the app is running.
* `PUT /payments`
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
* Build and test run on github using github actions

## Todo:

Deeper into The bake Bank. How far to go?

## Shortcomings

The merchant will likely want to see a list of their recent transactions. This would means 
 * storing a merchant id on each transaction
 * repo lookup method to get a list of transactions
 * A GET endpoint


There is potential data loss if the bank API fails badly (e.g. timeout or HTTP 500 response) and we won't have a record in the local data store.
Way to mitigate this likely involves:
 * Save the payment data locally _before_ sending it. This means using a locally generated id as key, not depending on the the transaction id that the bank returns.
 * make use of an [Idempotency key](https://stripe.com/docs/api/idempotent_requests) to make it safe for a client to repeat failed requests by inspecting the saved data and deciding what to do.
 
 
No API client. One can be generated from metadata using [NSwag](https://github.com/RicoSuter/NSwag).

No docker support. Not hard for an expert to add.

No performance data. This should either be gathered from metrics on a running app, or tested with [BenchmarkDotNet](https://benchmarkdotnet.org/).
 
Anthony Steele 
2020-05-12