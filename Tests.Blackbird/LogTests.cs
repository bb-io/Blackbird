using Apps.Blackbird.Actions;
using Apps.Blackbird.Models.Request.Birds;
using Apps.Blackbird.Models.Request.Flights;
using Newtonsoft.Json;

namespace Tests.Blackbird;
[TestClass]
public class LogTests : TestBase
{
    [TestMethod]
    public async Task BigLogsReturnsSuccess()
    {
        // Arrange
        var actions = new BirdActions(InvocationContext);

        // Act
        var result = await actions.GetBirdLogs(new BirdRequest { BirdId = "148890", NestId = "682" });

        // Assert
        Console.WriteLine(result.Log);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task FlightLogsReturnsSuccess()
    {
        // Arrange
        var actions = new FlightActions(InvocationContext);

        // Act
        var result = await actions.GetFlightLogs(new FlightRequest { BirdId = "148890", NestId = "682", FlightId = "e18b5841-3f1a-42d9-b7bf-b47200e6d761" });

        // Assert
        Console.WriteLine(result.Log);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task NotificationsReturnsSuccess()
    {
        // Arrange
        var actions = new AdminActions(InvocationContext);

        // Act
        var result = await actions.GetNotifications();

        // Assert
        Console.WriteLine(result.Log);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }
}
