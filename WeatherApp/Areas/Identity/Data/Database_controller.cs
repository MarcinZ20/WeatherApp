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

        

        Migrate();

        

        var lista = new List<String>(){"756135",  "2988507", "5128581", "1850147", "3094802"};
        var miasta = new List<String>(){"Warsaw"};

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

        

        foreach(String miasto in lista){
            InsertData(miasto);
            InsertDataUser(miasto);
        }
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
                                Miasto varchar(100),
                                Data varchar (100),
                                Temp varchar(100),
                                CONSTRAINT pk PRIMARY KEY (Miasto, Data)
                                );

                                DROP TABLE IF EXISTS UserPanel ;
                                CREATE TABLE IF NOT EXISTS UserPanel (
                                miasto varchar (100) primary key,
                                temp varchar(100),
                                wind varchar (100),
                                pressure varchar(100),
                                humidity varchar (100),
                                sunrise varchar(100),
                                sunset varchar (100),
                                cloud_cover varchar(100),
                                rain_perc varchar (100),
                                description varchar(100)
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

            new SqliteCommand($"INSERT INTO PogodaHourly (Miasto, Data, Temp) VALUES ('{item.miasto}','{item.data}','{item.temp}')",
                _connection)
            .ExecuteNonQuery();

        }
    }

    public async static void InsertDataUser(String miasto)
    {
        WeatherWebApiClient weatherclient =
                        new WeatherWebApiClient("http://api.openweathermap.org/data/2.5/weather", "0b4f476a87344c92c0fa31a21cef1566", miasto);
        UserPanelModel model = await weatherclient.GetWeatherUserPanelAsync();
        

        new SqliteCommand($"INSERT INTO UserPanel (miasto, temp, wind, pressure, humidity, sunrise, sunset, cloud_cover, rain_perc, description) VALUES ('{model.miasto}','{model.temp}','{model.wind}','{model.pressure}','{model.humidity}','{model.sunrise}','{model.sunset}','{model.cloud_cover}','{model.rain_perc}','{model.description}')",
            _connection)
        .ExecuteNonQuery();

    }
    

    

    public static List<HourlyModel> ListHourly(String city)
    {
        using var command = new SqliteCommand("SELECT * FROM PogodaHourly", //WHERE Miasto = \"" + city + "\"",
         _connection);
        using var reader = command.ExecuteReader();
        var records = new List<HourlyModel>();

        while (reader.Read())
        {
            records.Add(new HourlyModel
            {
                miasto = (string)reader.GetValue(0),
                data = (string)reader.GetValue(1),
                temp = (string)reader.GetValue(2)  
            });
        }
        return records;
    }

    public static UserPanelModel ListUserPanel(String city)
    {
        using var command = new SqliteCommand("SELECT * FROM UserPanel WHERE miasto = \"" + city +"\"", _connection);
        using var reader = command.ExecuteReader();

        Console.WriteLine(reader.Read());
        var records = new UserPanelModel{
            miasto = (string)reader.GetValue(0),
            temp = (string)reader.GetValue(1),
            wind = (string)reader.GetValue(2), 
            pressure = (string)reader.GetValue(3),
            humidity = (string)reader.GetValue(4), 
            sunrise = (string)reader.GetValue(5),
            sunset = (string)reader.GetValue(6), 
            cloud_cover = (string)reader.GetValue(7),
            rain_perc = (string)reader.GetValue(8), 
            description = (string)reader.GetValue(9)
        };


        return records;
    }
}
