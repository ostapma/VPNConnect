// See https://aka.ms/new-console-template for more information
using GeoIpUtils.IpToHex;
using Serilog;

string conn = "Data Source=C:\\Work\\Winsconnect\\Winsconnect\\geoip.db";

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

IpToHexConvertor convertor = new(conn);

Console.WriteLine("Populating IP address ranges converted to HEX for speeding up searches");

convertor.ConvertForCity();