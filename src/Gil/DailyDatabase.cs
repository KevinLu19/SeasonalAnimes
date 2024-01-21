using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace SeasonalAnime.Gil;
internal class DailyDatabase
{
    private SQLiteConnection _sqlite_conn;

    private const string _database = @"SeasonalAnime\src\Gil\Daily.sqlite";
    private const string _table = "entered_datetime";
	private string _conn_string = $"Data Source={_database};Version=3";

	public DailyDatabase()
    {
        /*
         Only Create new database when database don't exist. If it exist, dont create a new database.
         */
        
        
        // Create database if it doesn't exist.
        if (!System.IO.File.Exists(_database))
        {
            SQLiteConnection.CreateFile(_database);
            Console.WriteLine($"{_database} has been created.");
        }

        // Create Table if it doesn't exist.
        using (SQLiteConnection connection = new SQLiteConnection(_conn_string))
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {_table}";
                command.ExecuteNonQuery();
                Console.WriteLine($"{_table} has been created.");
            }
        }
    }

    public void InsertData(DateTime entered_datetime)
    {
		using (SQLiteConnection connection = new SQLiteConnection(_conn_string))
        {
            connection.Open();

            using (SQLiteCommand insert = new SQLiteCommand($"INSERT INTO {_table} VALUES ('{entered_datetime}')", connection))
            {
                insert.ExecuteNonQuery();
                Console.WriteLine($"{entered_datetime} has been added to table.");
            }
        }
	    	
    }

    public void QueryData(DateTime data)
    {
		using (SQLiteConnection connection = new SQLiteConnection(_conn_string))
        {
            connection.Open();

			using (SQLiteCommand queryCmd = new SQLiteCommand($"SELECT {data} FROM {_table};", connection))
			using (SQLiteDataReader reader = queryCmd.ExecuteReader())
			{
				while (reader.Read())
				{
					Console.WriteLine($"User ID: {reader["Id"]}, Name: {reader["Name"]}");
				}
			}
		}
	}
}
