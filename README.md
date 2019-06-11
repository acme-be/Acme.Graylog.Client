# Acme.GrayLog.Client
Very simple client for Graylog without a lot of external dependencies

## Usage
See the project Acme.Graylog.Client.Tests for a sample usage.

```
var configurationContent = File.ReadAllText("C:\\TMP\\Acme\\graylog-sample.json");
var configuration = JsonConvert.DeserializeObject<GraylogConfiguration>(configurationContent);

var client = new GrayLogHttpTlsClient(configuration);
await client.SendAsync($"Hello from {typeof(Program).Assembly.FullName}");
```

### Installation

Nuget :

Install-Package Acme.Graylog.Client

https://www.nuget.org/packages/Acme.Graylog.Client/