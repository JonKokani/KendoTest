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
        NpgsqlConnection _conn;
        IWebHostEnvironment _webHostEnvironment;

        public DataRepository(IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
        {
            _conn = new NpgsqlConnection(_configuration.GetConnectionString("dc"));
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddData(tblData data)
        {
            try
            {
                _conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(@"INSERT INTO aakhari.t_data(
                c_name, c_date, c_gender, c_hobbies, c_cid, c_img)
                VALUES (@name, @date, @gender, @hobbies, @cid, @imgPath);", _conn);
                cmd.Parameters.AddWithValue("@name", data.name);
                cmd.Parameters.AddWithValue("@date", data.date);
                cmd.Parameters.AddWithValue("@gender", data.gender);
                cmd.Parameters.AddWithValue("@hobbies", string.Join(",",data.hobbies));
                cmd.Parameters.AddWithValue("@cid", data.cid);

                if (data.imgFile != null && data.imgFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqFile = Guid.NewGuid().ToString() + "_" + data.imgFile.FileName;
                    string uploadPath = Path.Combine(uploadFolder, uniqFile);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        data.imgFile.CopyTo(stream);
                    }
                    cmd.Parameters.AddWithValue("@imgPath", uniqFile);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@imgPath", DBNull.Value);
                }
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("GetAllCourse Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        public void UpdateData(tblData data)
        {
            try
            {
                _conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(@"UPDATE aakhari.t_data
                SET c_name=@name, c_date=@date, c_gender=@gender, c_hobbies=@hobbies, c_cid=@cid, c_img=@imgPath
                WHERE c_id=@id;", _conn);
                cmd.Parameters.AddWithValue("@id", data.id);
                cmd.Parameters.AddWithValue("@name", data.name);
                cmd.Parameters.AddWithValue("@date", data.date);
                cmd.Parameters.AddWithValue("@gender", data.gender);
                cmd.Parameters.AddWithValue("@hobbies", string.Join(",",data.hobbies));
                cmd.Parameters.AddWithValue("@cid", data.cid);

                if (data.imgFile != null && data.imgFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqFile = Guid.NewGuid().ToString() + "_" + data.imgFile.FileName;
                    string uploadPath = Path.Combine(uploadFolder, uniqFile);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        data.imgFile.CopyTo(stream);
                    }
                    cmd.Parameters.AddWithValue("@imgPath", uniqFile);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@imgPath", DBNull.Value);
                }
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("GetAllCourse Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }

        }

        public void DeleteData(int id)
        {
             try
            {
                _conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(@"DELETE FROM aakhari.t_data
	            WHERE c_id=@id;", _conn);
                cmd.Parameters.AddWithValue("@id",id);
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("DeleteData Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        public List<tblCourse> GetAllCourse()
        {
            List<tblCourse> data = new List<tblCourse>();
            try
            {
                _conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT c_cid, c_cname
	            FROM aakhari.t_course;", _conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tblCourse d = new tblCourse
                    {
                        cid = Convert.ToInt32(reader["c_cid"]),
                        cname = reader["c_cname"].ToString()
                    };
                    data.Add(d);
                }
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("GetAllCourse Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return data;
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

                System.Console.WriteLine("GetAllCourse Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return data;
        }

        public tblData GetOneData(int id)
        {
            tblData data = null;
            try
            {
                _conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT a.c_id, a.c_name, a.c_date, a.c_gender, a.c_hobbies, a.c_cid, a.c_img, b.c_cname
                    FROM aakhari.t_data a
                    JOIN aakhari.t_course b ON a.c_cid=b.c_cid WHERE c_id=@id;", _conn);
                cmd.Parameters.AddWithValue("@id",id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data = new tblData
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
                    
                }
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("GetOneData Error: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return data;
        }
        
    }
}