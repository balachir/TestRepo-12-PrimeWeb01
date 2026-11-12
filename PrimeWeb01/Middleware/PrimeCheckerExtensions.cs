namespace PrimeWeb01.Middleware;

public static class PrimeCheckerExtensions
{
    public static IApplicationBuilder UsePrimeChecker(this IApplicationBuilder builder)
        => builder.UsePrimeChecker(new PrimeCheckerOptions());

    public static IApplicationBuilder UsePrimeChecker(this IApplicationBuilder builder, string path)
        => builder.UsePrimeChecker(new PathString(path));

    public static IApplicationBuilder UsePrimeChecker(this IApplicationBuilder builder, PathString path)
        => builder.UsePrimeChecker(new PrimeCheckerOptions { Path = path });

    public static IApplicationBuilder UsePrimeChecker(
        this IApplicationBuilder builder, PrimeCheckerOptions options)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(options);
        return builder.UseMiddleware<PrimeCheckerMiddleware>(options);
    }
}
