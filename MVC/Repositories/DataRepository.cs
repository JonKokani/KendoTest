using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using Npgsql;

namespace MVC.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly NpgsqlConnection _conn;
        public DataRepository(IConfiguration _configuration)
        {
            _conn = new NpgsqlConnection(_configuration.GetConnectionString("dc"));
        }
        public List<tblData> GetAllData()
        {
            List<tblData> data = new List<tblData>();
            try
            {
                _conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT a.c_id, a.c_name, a.c_date, a.c_gender, a.c_hobbies, a.c_cid, a.c_img, b.c_cname
                    FROM aakhari.t_data a
                    JOIN aakhari.t_course b ON a.c_cid=b.c_cid;", _conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tblData d = new tblData
                    {
                        id = Convert.ToInt32(reader["c_id"]),
                        name = reader["c_name"].ToString(),
                        date = Convert.ToDateTime(reader["c_date"].ToString()),
                        gender = reader["c_gender"].ToString(),
                        hobbies = reader["c_hobbies"].ToString().Split(",").ToList(),
                        cid = Convert.ToInt32(reader["c_cid"]),
                        cname = reader["c_cname"].ToString(),
                        imgPath = reader["c_img"].ToString()

                    };
                    data.Add(d);
                }

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("GetAllData Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return data;
        }
    }
}