using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiremockDemoSLT.Tests
{
    public class BaseTest
    {
        protected RestClient? Client;

        [SetUp]
        public void Setup()
        {
            Client = new RestClient(Config.ApiBaseUrl);
        }
    }
}
