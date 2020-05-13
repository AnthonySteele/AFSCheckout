using System;
using Checkout.Controllers;
using CheckoutApi.Controllers;
using NUnit.Framework;

namespace CheckoutApi.IntegrationTests
{
    public class ControllerServicesTests
    {
        [TestCase(typeof(HealthController))]
        [TestCase(typeof(PaymentController))]
        public void CanCreateControllerFromContainer(Type controllerType)
        {
            var controller = TestFixture.Server.GetRequiredService(controllerType);
            Assert.That(controller, Is.Not.Null);
        }
    }
}
