namespace PrimeWeb01.UnitTests;

public class PrimeService_IsPrimeShould
{
    [Theory]
    [InlineData(int.MinValue, false)]
    [InlineData(-7, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(5, true)]
    [InlineData(6, false)]
    [InlineData(9, false)]
    [InlineData(25, false)]
    [InlineData(49, false)]
    [InlineData(97, true)]
    [InlineData(2147395600, false)]
    [InlineData(int.MaxValue - 1, false)]
    [InlineData(int.MaxValue, true)]
    public void ClassifyInteger(int candidate, bool expected)
    {
        var service = new PrimeService();

        Assert.Equal(expected, service.IsPrime(candidate));
    }
}
