﻿using FubuMVC.Core.ServiceBus;
using NUnit.Framework;

namespace FubuMVC.Tests.ServiceBus.Docs.GettingStarted
{
    // SAMPLE: GettingStartedTest 
    [TestFixture, Explicit]
    public class GettingStartedTester
    {
        [Test]
        public void can_run_getting_started()
        {
            var applicationSource = new GettingStartedApplicationSource();
            var application = applicationSource.BuildApplication();
            var runtime = application.Bootstrap();
            var bus = runtime.Factory.Get<IServiceBus>();
            bus.Send(new StartPing());
            //Thread.Sleep(10000); //to hit breakpoints, etc. not meant to be an integration test
        }
    }
    // ENDSAMPLE
}