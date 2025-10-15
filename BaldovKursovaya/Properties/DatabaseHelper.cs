using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KonditerskayaApp
{
	public static class DatabaseHelper
	{
		private static string connectionString =
			@"Data Source=DESKTOP-44H3EEA;Initial Catalog=kursovayaDB;Integrated Security=True;";

		public static DataTable GetData(string sql)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
				{
					DataTable dt = new DataTable();
					da.Fill(dt);
					return dt;
				}
			}
		}

		public static void ExecuteNonQuery(string sql)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand(sql, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static void ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand(sql, conn))
				{
					if (parameters != null)
					{
						foreach (var kv in parameters)
						{
							cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
						}
					}
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
