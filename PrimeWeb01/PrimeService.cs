namespace PrimeWeb01;

public class PrimeService
{
    public bool IsPrime(int candidate)
    {
        if (candidate < 2)
        {
            return false;
        }

        for (int divisor = 2; divisor <= candidate / divisor; divisor++)
        {
            if (candidate % divisor == 0)
            {
                return false;
            }
        }

        return true;
    }
}
