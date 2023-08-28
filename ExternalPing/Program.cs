// See https://aka.ms/new-console-template for more information
using ExternalPing;
using System.Net.NetworkInformation;
using System.Text.Json;

string pingTarget = args[0];

PingResult pingResult = new PingResult();
pingResult.PingTime = DateTime.Now;
Ping ping = new Ping();
PingReply? pingReply;
try
{
    pingReply = ping.Send(pingTarget);
    pingResult.IsSuccess = pingReply.Status == IPStatus.Success;
    if (pingResult.IsSuccess)
    {
        pingResult.PingLatency = (int)pingReply.RoundtripTime;
    }
    else pingResult.Error = pingReply.Status.ToString();
}

catch (PingException ex)
{
    pingResult.IsSuccess = false;
    pingResult.PingLatency = 0;
    pingResult.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
}

string resultString = JsonSerializer.Serialize(pingResult);

Console.WriteLine(resultString);


