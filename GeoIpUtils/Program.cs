// See https://aka.ms/new-console-template for more information
using GeoIpUtils.IpToHex;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

IpToHexConvertor convertor = new();

Console.WriteLine("Populating IP address ranges converted to HEX for speeding up searches");

convertor.ConvertForCity();