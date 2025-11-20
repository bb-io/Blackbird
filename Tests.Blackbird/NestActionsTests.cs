using Newtonsoft.Json;
using Apps.Blackbird.Actions;
using Apps.Blackbird.Models.Request.Nests;

namespace Tests.Blackbird;

[TestClass]
public class NestActionsTests : TestBase
{
    [TestMethod]
    public async Task ListNests_ReturnsNests()
    {
		// Arrange
		var actions = new NestActions(InvocationContext);

        // Act
        var result = await actions.ListNests();

        // Assert
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetNest_ReturnsNest()
    {
        // Arrange
        var actions = new NestActions(InvocationContext);
        var request = new NestRequest { NestId = "687" };

        // Act
        var result = await actions.GetNest(request);

        // Assert
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsNotNull(result);
    }
}
