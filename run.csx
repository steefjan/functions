using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // parse query parameter
    string timestamp = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "timestamp", true) == 0)
        .Value;

    DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    log.Info("Function call");

    if (timestamp == null)
    {
        // Get request body
        dynamic data = await req.Content.ReadAsAsync<object>();
        timestamp = data?.timestamp;
        long ePochTime = long.Parse(timestamp);
        dt = FromUnixTime(ePochTime);
        log.Info(dt.ToString());
    } 

    return dt == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, dt);
}

private static DateTime FromUnixTime(long unixTime)
{
    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    return epoch.AddSeconds(unixTime);
}