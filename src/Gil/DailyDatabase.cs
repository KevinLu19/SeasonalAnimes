using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace SeasonalAnime.Gil;
internal class DailyDatabase 
{
    // Set connection string.
    private string _connection_string = "Data Source=mydatabase.db;Version=3;";

	public DailyDatabase()
    {
        // Check if database file exist.
        if (!System.IO.File.Exists("mydatabase.db"))
        {
            SQLiteConnection.CreateFile("mydatabase.db");

			using (SQLiteConnection connection = new SQLiteConnection(_connection_string))
			{
				connection.Open();
				Console.WriteLine("Database opened.");

				// Specify the table creation SQL query.
				string create_table = "CREATE TABLE IF NOT EXISTS MyTable (Id INTEGER PRIMARY KEY, Name TEXT)";

				using (SQLiteCommand command = new SQLiteCommand(create_table, connection))
				{
					// Execute query to create the table.
					command.ExecuteNonQuery();
				}

				Console.WriteLine("Table has been created.");

			}
		}
		else
		{
			Console.WriteLine("Database already exists.");
		}
    }

	private void InsertTable(DateTime snapshot_date)
	{
		int id = 1;

		var convert_date = snapshot_date.ToString();

		// open connection
		using (SQLiteConnection connection = new SQLiteConnection(_connection_string))
		{
			connection.Open();

			string insert = "INSERT INTO MyTable (Id, Name) VALUES (@Id, @Name)";

			using (SQLiteCommand command = new SQLiteCommand(insert, connection))
			{
				command.Parameters.AddWithValue("@Id", id);
				command.Parameters.AddWithValue("@Name", convert_date);

				command.ExecuteNonQuery();

				Console.WriteLine($"Added {convert_date} in ID {id}");
			}
		}
	}

	public void QueryTable(DateTime entered_date)
	{
		string sql_command = $"SELECT * FROM MyTable WHERE Name='{entered_date}';";

		// Open database connection
		using (SQLiteConnection connection = new SQLiteConnection(_connection_string))
		{
			connection.Open();

			// Create command with query
			using (SQLiteCommand command = new SQLiteCommand(sql_command, connection))
			{
				// Execute Query
				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					// stuff
				}
			}
		}
	}
}
