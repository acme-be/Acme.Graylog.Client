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

### Errors
The errors when sending a message can be get in an event, this include the messageBody (compressed binary message) and the messageContent (json)

```
var client = new GrayLogHttpTlsClient(configuration);

client.SendErrorOccured += (sender, error) =>
    {
        Console.WriteLine($"Exception : {error.Exception} when sending message");
    };

await client.SendAsync($"Hello from {typeof(Program).Assembly.FullName}");
```

### Get the GELF data
If you need it, you can get the raw GELF data from an object if you want to store it anywhere else.

```
var dummy = new Dummy { FirstName = "Simon", LastName = "Baudart" };
var gelf = client.CreateGelfObject("This is a sample object", null, dummy);
Console.WriteLine(gelf);
```

### Installation

Nuget :

Install-Package Acme.Graylog.Client

https://www.nuget.org/packages/Acme.Graylog.Client/