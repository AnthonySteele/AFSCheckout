# AFS-Checkout

Api demo code for checkout.com

## HTTP Endpoints

* GET `/health` A simple "liveness check" that responds OK when the app is running.
* PUT `/payments`

## Other features

*  Checks at compile time configured in `Directory.Build.props`
  * Warnings as errors
  * References not null by default
  * FxCopAnalyzers with rulesets
* Integration tests using `TestHost`
* Build and test run on github using github actions

## Todo:

drop down to unit tests for the details
Swagger
Prometheus Monitoring
 
Anthony Steele 
2020-05-12