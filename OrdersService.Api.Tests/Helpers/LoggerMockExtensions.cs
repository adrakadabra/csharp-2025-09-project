using Microsoft.Extensions.Logging;
using Moq;

namespace OrdersService.Api.Tests.Unit.Helpers;

public static class LoggerMockExtensions
{
    public static void VerifyLog<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel level,
        Func<string, bool> messagePredicate,
        Times times)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => messagePredicate(v.ToString() ?? string.Empty)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }
}