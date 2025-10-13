using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using CalculatorApp;
using CalculatorApp.Parsers;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/calculate", async (HttpRequest req, HttpResponse res) =>
{
    //Read the raw body as a string so I can parse it
    string body;
    using (var reader = new StreamReader(req.Body, Encoding.UTF8))
    {
        body = await reader.ReadToEndAsync();
    }

    if (string.IsNullOrWhiteSpace(body))
    {
        res.StatusCode = StatusCodes.Status400BadRequest;
        await res.WriteAsync("Empty request body.");
        return;
    }

    //Detect the file type either XML or JSON - Default to JSON if not set
    var contentType = req.ContentType?.ToLowerInvariant() ?? "application/json";
    Operation op;

    try

    {
        if (contentType.Contains("xml"))
        {
            op = InputPaser.ParseXml(body);
        }
        else
        {
            op = InputPaser.ParseJson(body);
        }
    }
    catch (Exception ex)
    {
        res.StatusCode = StatusCodes.Status400BadRequest;
        await res.WriteAsync($"Parsing error: {ex.Message}");
        return;
    }

    //Do the operations
    double result = 0;
    try
    {
        var maths = new Maths();
        result = maths.Calculate(op);
    }
    catch (DivideByZeroException)
    {
        res.StatusCode = StatusCodes.Status400BadRequest;
        await res.WriteAsync("Math error: Division by zero.");
    }
    catch (Exception ex)
    {
        res.StatusCode = StatusCodes.Status500InternalServerError;
        await res.WriteAsync($"Calculation Error: {ex.Message}");
    }

    //return XML or JSON if asked for
    var accept = req.Headers.Accept.ToString().ToLowerInvariant();
    var wantsJson = accept.Contains("application/json");

    if (wantsJson)
    {
        res.ContentType = "application/json";
        await res.WriteAsync($"{{\"Result\": {result.ToString(System.Globalization.CultureInfo.InvariantCulture)} }}");
    }
    else
    {
        res.ContentType = "application/xml; charset=utf-8";
        var xml = new XDocument(new XElement("Result", result.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        await res.WriteAsync(xml.ToString(SaveOptions.DisableFormatting));
    }
});

app.Run();