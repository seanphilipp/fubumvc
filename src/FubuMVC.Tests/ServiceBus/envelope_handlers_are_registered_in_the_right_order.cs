﻿using FubuMVC.Core;
using FubuMVC.Core.ServiceBus.Configuration;
using FubuMVC.Core.ServiceBus.Runtime.Invocation;
using NUnit.Framework;
using Shouldly;

namespace FubuMVC.Tests.ServiceBus
{
    [TestFixture]
    public class envelope_handlers_are_registered_in_the_right_order
    {
        [Test]
        public void the_order()
        {
            using (var runtime = FubuApplication.For<Defaults>().Bootstrap())
            {
                var handlers = runtime.Factory.Get<IHandlerPipeline>().ShouldBeOfType<HandlerPipeline>().Handlers;


                handlers[0].ShouldBeOfType<DelayedEnvelopeHandler>();
                handlers[1].ShouldBeOfType<ResponseEnvelopeHandler>();
                handlers[2].ShouldBeOfType<ChainExecutionEnvelopeHandler>();
                handlers[3].ShouldBeOfType<NoSubscriberHandler>();
            }
        }

        public class Defaults : FubuRegistry
        {
            public Defaults()
            {
                Features.ServiceBus.Enable(true);
                EnableInMemoryTransport();
            }
        }
    }
}