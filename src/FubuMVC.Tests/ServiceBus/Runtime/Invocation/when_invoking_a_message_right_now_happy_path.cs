﻿using FubuMVC.Core.Registration;
using FubuMVC.Core.ServiceBus.Configuration;
using FubuMVC.Core.ServiceBus.Runtime;
using FubuMVC.Core.ServiceBus.Runtime.Invocation;
using FubuMVC.Tests.ServiceBus.ScenarioSupport;
using FubuMVC.Tests.TestSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.ServiceBus.Runtime.Invocation
{
    [TestFixture]
    public class when_invoking_a_message_right_now_happy_path : InteractionContext<ChainInvoker>
    {
        private OneMessage theMessage;
        private HandlerGraph theGraph;
        private HandlerChain theExpectedChain;
        private StubServiceFactory theFactory;
        private object[] cascadingMessages;

        protected override void beforeEach()
        {
            theMessage = new OneMessage();
            Services.Inject(new BehaviorGraph());

            ClassUnderTest.InvokeNow(theMessage);
        }

        [Test]
        public void invoked_through_the_pipeline()
        {
            MockFor<IHandlerPipeline>().AssertWasCalled(x => 
                x.Invoke(Arg<Envelope>.Matches(e => e.Message == theMessage)));
        }
    }
}