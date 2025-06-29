namespace Dapr.DurableTask.Worker.Shims.Tests;

public class JsonDataConverterShimTests
{
    [Fact]
    public void Constructor_WithNullConverter_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new JsonDataConverterShim(null));
    }

    [Fact]
    public void Serialize_ForwardsCallToInnerConverter()
    {
        // Arrange
        var testObject = new { Name = "Test", Value = 123 };
        var expectedResult = "{\"Name\":\"Test\",\"Value\":123}";
            
        var mockConverter = new Mock<DataConverter>();
        mockConverter
            .Setup(c => c.Serialize(testObject))
            .Returns(expectedResult);
            
        var shim = new JsonDataConverterShim(mockConverter.Object);
            
        // Act
        var result = shim.Serialize(testObject);
            
        // Assert
        Assert.Equal(expectedResult, result);
        mockConverter.Verify(c => c.Serialize(testObject), Times.Once);
    }
        
    [Fact]
    public void SerializeWithFormatting_ForwardsCallToInnerConverter_IgnoresFormattingParameter()
    {
        // Arrange
        var testObject = new { Name = "Test", Value = 123 };
        var expectedResult = "{\"Name\":\"Test\",\"Value\":123}";
            
        var mockConverter = new Mock<DataConverter>();
        mockConverter
            .Setup(c => c.Serialize(testObject))
            .Returns(expectedResult);
            
        var shim = new JsonDataConverterShim(mockConverter.Object);
            
        // Act
        var result = shim.Serialize(testObject, true);
            
        // Assert
        Assert.Equal(expectedResult, result);
        mockConverter.Verify(c => c.Serialize(testObject), Times.Once);
    }
        
    [Fact]
    public void Deserialize_ForwardsCallToInnerConverter()
    {
        // Arrange
        var jsonData = "{\"Name\":\"Test\",\"Value\":123}";
        var expectedObject = new TestClass { Name = "Test", Value = 123 };
            
        var mockConverter = new Mock<DataConverter>();
        mockConverter
            .Setup(c => c.Deserialize(jsonData, typeof(TestClass)))
            .Returns(expectedObject);
            
        var shim = new JsonDataConverterShim(mockConverter.Object);
            
        // Act
        var result = shim.Deserialize(jsonData, typeof(TestClass));
            
        // Assert
        Assert.Same(expectedObject, result);
        mockConverter.Verify(c => c.Deserialize(jsonData, typeof(TestClass)), Times.Once);
    }
        
    [Fact]
    public void Deserialize_WithNullData_ForwardsCallToInnerConverter()
    {
        // Arrange
        string jsonData = null;
        TestClass? expectedObject = null;
            
        var mockConverter = new Mock<DataConverter>();
        mockConverter
            .Setup(c => c.Deserialize(jsonData, typeof(TestClass)))
            .Returns(expectedObject);
            
        var shim = new JsonDataConverterShim(mockConverter.Object);
            
        // Act
        var result = shim.Deserialize(jsonData, typeof(TestClass));
            
        // Assert
        Assert.Null(result);
        mockConverter.Verify(c => c.Deserialize(jsonData, typeof(TestClass)), Times.Once);
    }
        
    class TestClass
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}