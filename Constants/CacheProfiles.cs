
using Microsoft.AspNetCore.Mvc;

public class CacheProfiles
{
    public const String Default10 = "Default10";
    public const String Default20 = "Default20";
    public static readonly CacheProfile Profile10 = new()
    {
        Duration = 10
    };
    public static readonly CacheProfile Profile20 = new()
    {
        Duration = 20
    };
}