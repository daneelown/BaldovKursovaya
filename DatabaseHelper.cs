using System;
using System.Data;
using System.Data.SqlClient;

namespace KonditerskayaApp
{
	public static class DatabaseHelper
	{
		private static string connectionString =
			@"Data Source=.\SQLEXPRESS;Initial Catalog=kursovayaDB;Integrated Security=True;";

		public static DataTable GetData(string query)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					SqlDataAdapter adapter = new SqlDataAdapter(cmd);
					DataTable dt = new DataTable();
					adapter.Fill(dt);
					return dt;
				}
			}
		}

		public static void ExecuteQuery(string query)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
