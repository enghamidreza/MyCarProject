using CarProject2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CarProject2.REPOSITORIES
{
    public class DriverRepository : IDriverRepository
    {
        private readonly string _test; // redonly yani hamoon time meghdar dadeh mishe 
        // farghesh ba const ine ke const taghirr napazireh va hamoon aval ye meghdari behesh dadi hamoon mimoneh 
        public DriverRepository(IConfiguration configuration)
        {
            _test = configuration.GetConnectionString("DefaultConnection");
        }

        // baraye jologiri az add shodan id rekrari dar database index gozashtam 
        // va dar repositori ham try()_catch() mizaram va dakhel catch add error dar sqlserver mizaram  

        public int AddDriver(Driver driver)
        {

            // baraye jologiri az add shodan id rekrari dar database index gozashtam 
            // va dar repositori ham try()_catch() mizaram va dakhel catch add error dar sqlserver mizaram  
            try
            {
                using var cn = new SqlConnection(_test);
                using var cmd = new SqlCommand("dbo.sp_InsertDriver", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FullName", driver.FullName);
                cmd.Parameters.AddWithValue("@NationalCode", driver.NationalCode);
                cmd.Parameters.AddWithValue("@Phone", (object?)driver.Phone ?? DBNull.Value);

                var pOut = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pOut);

                cn.Open();
                cmd.ExecuteNonQuery(); //kar ejra besheh

                return (int)pOut.Value;// SCOPE_IDENTITY();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)   // va dar repositori ham try()_catch() mizaram va dakhel catch add error dar sqlserver mizaram  
            //2627 VA 2601 HAMOON PAYAM ERROR DAR DATABASE BA MAFHOOM PRIMARY KEY YA DUPLICAT HASTESH 
            {
                // 2627 & 2601 = Violation of UNIQUE constraint
                throw new Exception("NationalCodeExists");
            }
        }

        //public int AddDriver(Driver driver)
        //{

        //    using var cn = new SqlConnection(_test);//using >>miad connection ro a database open va badesh close mikoneh
        //    //// agar faghat (((var cn = new SqlConnection(_test);))) benivisam bayad akhar code cn.Close(); va cn.Dispose(); bezaram 
        //    using var cmd = new SqlCommand("dbo.sp_InsertDriver", cn) { CommandType = CommandType.StoredProcedure };
        //    cmd.Parameters.AddWithValue("@FullName", driver.FullName);
        //    cmd.Parameters.AddWithValue("@NationalCode", driver.NationalCode);
        //    cmd.Parameters.AddWithValue("@Phone", (object?)driver.Phone ?? DBNull.Value);
        //    var pOut = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //    cmd.Parameters.Add(pOut);
        //    cn.Open();
        //    cmd.ExecuteNonQuery();// kar ejra besheh

        //    return (int)pOut.Value; // SCOPE_IDENTITY();
        //}

        public void DeleteDriver(int id)
        {
            using var cn = new SqlConnection(_test);
            using var cmd = new SqlCommand("dbo.sp_DeleteDriver", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Driver> GetAlllDrivers()
        {
            var list = new List<Driver>();
            using var cn = new SqlConnection(_test);
            using var cmd = new SqlCommand("dbo.sp_GetAllDrivers", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new Driver
                {
                    id = rd.GetInt32(rd.GetOrdinal("Id")),
                    FullName = rd["FullName"].ToString(),
                    NationalCode = rd["NationalCode"].ToString(),
                    Phone = rd["Phone"] as string
                });
            }
            return list;
        }

        public Driver GetDriverById(int id)
        {
            using var cn = new SqlConnection(_test);
            using var cmd = new SqlCommand("dbo.sp_GetDriverById", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            using var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                return new Driver
                {
                    id = rd.GetInt32(rd.GetOrdinal("Id")),
                    FullName = rd["FullName"].ToString(),
                    NationalCode = rd["NationalCode"].ToString(),
                    Phone = rd["Phone"] as string
                };
            }
            return null;
        }

        public List<Driver> GetSearchDrivers(string driver)
        {
            var Drivers = new List<Driver>();

            using var cn = new SqlConnection(_test);
            using var cmd = new SqlCommand("dbo.sp_SearchDrivers", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", driver); //@Name AZ KHODEH SP HASTESH VA driver HAM AZ CODE yani argoman Edit
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Drivers.Add(new Driver
                {
                    id = (int)reader["Id"],
                    FullName = reader["FullName"].ToString(),
                    NationalCode = reader["NationalCode"].ToString(),
                    Phone = reader["Phone"].ToString()
                });
            }
            return Drivers;
        }

        public void UpdateDriver(Driver driver)
        {
            using var cn = new SqlConnection(_test);
            using var cmd = new SqlCommand("dbo.sp_Updatedriver", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", driver.id);
            cmd.Parameters.AddWithValue("@FullName", driver.FullName);
            cmd.Parameters.AddWithValue("@NationalCode", driver.NationalCode);
            cmd.Parameters.AddWithValue("@Phone", (object?)driver.Phone ?? DBNull.Value);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
