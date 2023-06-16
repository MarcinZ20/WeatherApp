using Microsoft.Data.Sqlite;
using WeatherApp.Models;
using WeatherApp.Controllers;

namespace WeatherApp.Areas.Identity.Data;

public class Database_controller
{
    private static SqliteConnection? _connection;

     
    public async static void Init()
    {
        _connection = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = "./WeatherAppData.db"
        }.ConnectionString);
        _connection.Open();

        Console.WriteLine("Fuck!");

        Migrate();

        Console.WriteLine("Fuck!");

        var lista = new List<String>(){"756135",  "2988507", "5128581", "1850147"};

        foreach (String miasto in lista)
        {
            WeatherWebApiClient weatherclient =
                        new WeatherWebApiClient("http://api.openweathermap.org/data/2.5/weather", "0b4f476a87344c92c0fa31a21cef1566", miasto);
            WeatherModel model = await weatherclient.GetWeatherAsync();
            weatherclient.GetWeatherHourlyAsync();
            new SqliteCommand($"INSERT INTO Frontpage (MiastoID,Temp,Feels,Pressure, Humidity) VALUES ('{miasto}', '{model.temp}','{model.feels_like}', '{model.pressure}', '{model.humidity}')",
                _connection)
            .ExecuteNonQuery();
        }

        InsertData("756135");    
    
    }

    public static void Migrate()
    {
        const string command = @"DROP TABLE IF EXISTS Frontpage ;
                                CREATE TABLE IF NOT EXISTS Frontpage (
                                MiastoID varchar(255) primary key,
                                Temp varchar(100),
                                Feels varchar(100),
                                Pressure varchar(100),
                                Humidity varchar(100)
                                );

                                DROP TABLE IF EXISTS Pogoda8dni ;
                                CREATE TABLE IF NOT EXISTS Pogoda8dni (
                                Data varchar(100) primary key,
                                Temp varchar(100)
                                
                                );
                                
                                DROP TABLE IF EXISTS PogodaHourly ;
                                CREATE TABLE IF NOT EXISTS PogodaHourly (
                                Data varchar (100) primary key,
                                Temp varchar(100)
                                );
                                
                                ";
        new SqliteCommand(command, _connection).ExecuteNonQuery();
    }

    public static List<FrontEndWeather> ListRecords()
    {
        using var command = new SqliteCommand("SELECT * FROM Frontpage", _connection);
        using var reader = command.ExecuteReader();
        var records = new List<FrontEndWeather>();
        while (reader.Read())
        {
            records.Add(new FrontEndWeather
            {
                MiastoID = (string)reader.GetValue(0),
                Temp = (string)reader.GetValue(1),
                Feels = (string)reader.GetValue(2),
                Pressure = (string)reader.GetValue(3),
                Humidity = (string)reader.GetValue(4),
            });
        }

        return records;
    }

    public async static void InsertData(String miasto)
    {
        WeatherWebApiClient weatherclient =
                        new WeatherWebApiClient("http://api.openweathermap.org/data/2.5/forecast", "0b4f476a87344c92c0fa31a21cef1566", miasto);
        List<HourlyModel> model = await weatherclient.GetWeatherHourlyAsync();
        foreach(HourlyModel item in  model){

            new SqliteCommand($"INSERT INTO PogodaHourly (Data, Temp) VALUES ('{item.data}','{item.temp}')",
                _connection)
            .ExecuteNonQuery();

        }
    }

    public static List<HourlyModel> ListHourly()
    {
        using var command = new SqliteCommand("SELECT * FROM PogodaHourly", _connection);
        using var reader = command.ExecuteReader();
        var records = new List<HourlyModel>();
        while (reader.Read())
        {
            records.Add(new HourlyModel
            {
                data = (string)reader.GetValue(0),
                temp = (string)reader.GetValue(1),    
            });
        }

        return records;
    }
}