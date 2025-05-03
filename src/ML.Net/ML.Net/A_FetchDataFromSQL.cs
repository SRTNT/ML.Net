using ML.Net.Domains;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Net;

internal class A_FetchDataFromSQL
{
    internal static List<Advertisement> FetchAdvertisements()
    {
        var advertisements = new List<Advertisement>();
        string connectionString =
            "Data Source=localhost,11433;" +
            "User ID=sa;" +
            "Initial Catalog=AdvertisementsDB;" +
            "Password=SaeedTNT220;" +
            "Connect Timeout=30;" +
            "Encrypt=False;" ;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"SELECT a.Area, a.BuildYear, a.Rooms, a.TotalPrice, a.Floor, a.Elevator, a.Parking, a.Storage, l.Name
                         FROM Advertisements a
                         INNER JOIN Locations l ON a.LocationId = l.Id";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var ad = new Advertisement
                    {
                        Area = (int)reader.GetInt32(0),
                        BuildYear = (int)reader.GetInt32(1),
                        Rooms = (int)reader.GetInt32(2),
                        TotalPrice = (long)reader.GetInt64(3),
                        Floor = (int)reader.GetInt32(4),
                        Elevator = reader.GetBoolean(5),
                        Parking = reader.GetBoolean(6),
                        Storage = reader.GetBoolean(7),
                        LocationName = reader.GetString(8)
                    };

                    advertisements.Add(ad);
                }
            }

            connection.Close();
        }

        return advertisements;
    }

}
