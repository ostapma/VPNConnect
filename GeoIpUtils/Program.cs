// See https://aka.ms/new-console-template for more information
using GeoIpUtils.IpToHex;
using Serilog;

string dbFile = "C:\\Work\\Winsconnect\\VpnConnect\\geoip.db";

string conn = $"Data Source={dbFile};";

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

IpToHexConvertor convertor = new(conn);

Console.WriteLine("Warning! To speed up data updates db flushing is disabled that may corrupt database if processing has been interrupted");
Console.WriteLine($"Please backup database file {dbFile} before the start");
Console.WriteLine("Press any key if you are ready");
Console.ReadKey();

Console.WriteLine("Populating cities IP address ranges converted to HEX for speeding up searches");

convertor.ConvertForCity();

Console.WriteLine("Populating ASN IP address ranges converted to HEX for speeding up searches");

convertor.ConvertForAsn();