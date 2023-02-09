using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Banking_tests
{
    public class MockedLogger<T>
    {
        public MockedLogger()
        {
            MockLog = new Mock<ILogger<T>>();
            MockLog.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception });

                    LoggedMessages.Add((logLevel, logMessage));
                }));
        }

    //    public MockedLoggerFactory() { 

    //        MockFactory_ = new Mock<ILoggerFactory>();
    //        MockFactory_.Setup(x=>x.CreateLogger<T>()).Returns()

    //}

        public Mock<ILogger<T>> MockLog { get; }
        public Mock<ILoggerFactory> MockFactory_ { get; }

        public List<(LogLevel Level, string Message)> LoggedMessages { get; } = new List<(LogLevel Level, string Message)>();
    }
}
