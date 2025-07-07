# 🧩 WireMock C# Starter — Shift Left Mock API Testing with NUnit

This repository shows how to **shift left** your API or UI tests using **WireMock** for stubbing HTTP endpoints and **C# (.NET)** with **NUnit + RestSharp** to write and run tests — all **containerized** and ready for **local runs or CI/CD**.

---

## 🚀 Why this matters

✅ Test your application logic **before the real backend is ready**
✅ Catch integration bugs **early in the SDLC (shift left)**
✅ Mock real-world edge cases like `500` errors, `404`s, or timeouts
✅ Stabilize your pipelines with **repeatable, isolated tests**
✅ A practical starter for **SDETs, QA, and automation engineers**

---

## 📂 Folder structure

```
wiremock-csharp-starter/
 ├── mocks/
 │   ├── __mappings/
 │   │   ├── login-success.json
 │   │   ├── login-500.json
 │   └── docker-compose.yml
 ├── Tests/
 │   ├── LoginTests.cs
 ├── .github/
 │   └── workflows/
 │       └── ci.yml
 ├── README.md
```

---

## 🧩 How does it work?

* **WireMock**: Runs as a local mock HTTP server inside Docker.
* **Mappings**: JSON stubs under `_mappings/` tell WireMock what to return for given requests.
* **Tests**: `NUnit` tests send real HTTP requests using `RestSharp`.
* **CI/CD**: A GitHub Actions workflow spins up WireMock, runs tests, and verifies stubs — with **manual dispatch**.

---

## ✅ Example WireMock stub

**`mocks/_mappings/login-success.json`**

```json
{
  "request": {
    "method": "POST",
    "url": "/api/login",
    "bodyPatterns": [
      {
        "matchesJsonPath": "$[?(@.username == 'testuser')]"
      }
    ]
  },
  "response": {
    "status": 200,
    "body": "{ \"token\": \"mock123\" }",
    "headers": {
      "Content-Type": "application/json"
    }
  }
}
```

---

## ✅ Example NUnit test

**`Tests/LoginTests.cs`**

```csharp
[Test]
public void Login_Success_ShouldReturn200()
{
    var client = new RestClient("http://localhost:9876"); // Your WireMock port
    var request = new RestRequest("/api/login", Method.Post);
    request.AddJsonBody(new { username = "testuser", password = "pass" });

    var response = client.Execute(request);

    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    Assert.That(response.Content, Does.Contain("token"));
}
```

---

## ✅ Example `docker-compose.yml`

**`mocks/docker-compose.yml`**

```yaml
version: "3.8"

services:
  wiremock:
    image: wiremock/wiremock:3.3.1
    ports:
      - "9876:8080" # host:container
    volumes:
      - ./_mappings:/home/wiremock/mappings
```

---

## ✅ Example GitHub Actions workflow

**`.github/workflows/ci.yml`**

```yaml
name: WireMock Tests CI

on:
  workflow_dispatch:

jobs:
  run-tests:
    runs-on: ubuntu-latest

    services:
      wiremock:
        image: wiremock/wiremock:3.3.1
        ports:
          - 9876:8080
        volumes:
          - ./mocks/_mappings:/home/wiremock/mappings

    env:
      API_BASE_URL: http://localhost:9876

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '7.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release

      - name: Run tests
        run: dotnet test --configuration Release --logger trx
```

---

## ✅ Running locally

1️⃣ Clone this repo

```bash
git clone https://github.com/yourname/wiremock-csharp-starter.git
cd wiremock-csharp-starter
```

2️⃣ Start WireMock in Docker

```bash
docker-compose -f mocks/docker-compose.yml up -d
```

WireMock runs at [http://localhost:9876](http://localhost:9876)

3️⃣ Run the tests

```bash
dotnet restore
dotnet test
```

---

## ✅ Run in CI/CD

* Push your branch with `.github/workflows/ci.yml`
* Go to **Actions** → Select **WireMock Tests CI** → Click **Run workflow** → Tests will run with the mock server.

---

## ✅ Should you keep these mocks?

* ✅ For shift-left tests: Yes.
* ✅ When the real backend is ready: Keep the mocks for negative tests, chaos testing, or failover simulation.
* ✅ Good practice: Maintain separate `mock` folder — switch real URL or mock URL by config.

---

## ✅ Who’s this for?

* 📌 **SDETs / Automation Engineers** wanting realistic mocks
* 📌 Teams doing **early automation with CI/CD**
* 📌 Anyone new to **WireMock + C#**

---

## 💡 Good next steps

* 🔹 Add more stubs for `404`, `401`, `400` cases
* 🔹 Try dynamic response templating with `{{request.body.someField}}`
* 🔹 Integrate with your actual UI E2E flows (Playwright/Selenium)
* 🔹 Use same mocks in local + Docker + CI

---

## 📜 License

MIT — free for everyone to use & learn.

---

### 🔗 Made for the community — Happy Shift-Left Testing 🚀
