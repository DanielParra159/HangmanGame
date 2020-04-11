using Application.Services.EventDispatcher;
using Domain.Services.EventDispatcher;
using NSubstitute;
using NUnit.Framework;

namespace Application.Services.Tests.EventDispatcher
{
    public class TestSignal : Signal
    {
        public readonly string SomeData;

        public TestSignal(string someData)
        {
            SomeData = someData;
        }
    }

    public class TestSignal2 : Signal
    {
        public readonly int SomeData;

        public TestSignal2(int someData)
        {
            SomeData = someData;
        }
    }

    public class EventDispatcherTest
    {
        [Test]
        public void WhenCallbackIsSubscribedAndPerformCall_CallToTheCallback()
        {
            var eventDispatcher = new EventDispatcherServiceImpl();
            var callback1 = Substitute.For<SignalDelegate>();
            eventDispatcher.Subscribe<TestSignal>(callback1);

            var testSignal = new TestSignal("SomeData");
            eventDispatcher.Dispatch(testSignal);

            callback1.Received().Invoke(testSignal);
        }

        [Test]
        public void WhenMultipleCallbacksAreSubscribedAndPerformCall_CallToTheCallbacks()
        {
            var eventDispatcher = new EventDispatcherServiceImpl();
            var callback1 = Substitute.For<SignalDelegate>();
            var callback2 = Substitute.For<SignalDelegate>();
            eventDispatcher.Subscribe<TestSignal>(callback1);
            eventDispatcher.Subscribe<TestSignal>(callback2);

            var testSignal = new TestSignal("SomeData");
            eventDispatcher.Dispatch(testSignal);

            callback1.Received().Invoke(testSignal);
            callback2.Received().Invoke(testSignal);
        }

        [Test]
        public void WhenCallbacksIsUnsubscribedAndPerformCall_DoNotCallToTheCallback()
        {
            var eventDispatcher = new EventDispatcherServiceImpl();
            var callback1 = Substitute.For<SignalDelegate>();
            var callback2 = Substitute.For<SignalDelegate>();
            eventDispatcher.Subscribe<TestSignal>(callback1);
            eventDispatcher.Subscribe<TestSignal>(callback2);
            eventDispatcher.Unsubscribe<TestSignal>(callback1);

            var testSignal = new TestSignal("SomeData");
            eventDispatcher.Dispatch(testSignal);

            callback1.DidNotReceive();
            callback2.Received().Invoke(testSignal);
        }

        [Test]
        public void WhenDifferentSignalsAreRegisteredAndPerformCallWithOne_OnlyCallToTheAssociateCallbacksOfThisSignal()
        {
            var eventDispatcher = new EventDispatcherServiceImpl();
            var callback1 = Substitute.For<SignalDelegate>();
            var callback2 = Substitute.For<SignalDelegate>();
            eventDispatcher.Subscribe<TestSignal>(callback1);
            eventDispatcher.Subscribe<TestSignal2>(callback2);

            var testSignal2 = new TestSignal2(123);
            eventDispatcher.Dispatch(testSignal2);

            callback1.DidNotReceive();
            callback2.Received().Invoke(testSignal2);
        }
    }
}
