using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiremockDemoSLT.Tests;

public class Config
{
    public static readonly string ApiBaseUrl =  Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:9876";

    public static bool IsMock => ApiBaseUrl.Contains("localhost");
}
