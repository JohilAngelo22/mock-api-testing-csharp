using NUnit.Framework;
using RestSharp;
using System.Net;

namespace WiremockDemoSLT.Tests;
[TestFixture]
public class LoginTests : BaseTest
{
    [Test]
    public void Login_Success_ShouldReturn200()
    {
        var request = new RestRequest("/api/login", Method.Post);
        request.AddJsonBody(new {username = "testuser"});

        var response =  Client!.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content, Does.Contain("token"));
    }

    [Test]
    public void Login_Unauthorized_ShouldReturn401()
    {
        var request = new RestRequest("/api/login", Method.Post);
        request.AddJsonBody(new { username = "baduser" });

        var response = Client!.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public void Login_Forbidden_ShouldReturn403()
    {
        var request = new RestRequest("/api/login", Method.Post);
        request.AddJsonBody(new { username = "blockedUser", password = "pass" });

        var response = Client!.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public void Login_InternalServerError_ShouldReturn500()
    {
        if (!Config.IsMock) 
            Assert.Ignore("500 forced test only runs on mock.");

        var request = new RestRequest("/api/login", Method.Post);
        request.AddJsonBody(new { username = "cause500", password = "pass" });

        var response = Client!.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public void Login_BadRequest_ShouldReturn400()
    {
        var request = new RestRequest("/api/login", Method.Post);
        request.AddJsonBody(new { password = "pass" }); // No username!

        var response = Client!.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public void Login_UnknownPath_ShouldReturn404()
    {
        var request = new RestRequest("/api/unknown-login", Method.Post);
        request.AddJsonBody(new { username = "any", password = "any" });

        var response = Client!.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}

