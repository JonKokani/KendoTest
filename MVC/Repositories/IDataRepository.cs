using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Repositories
{
    public interface IDataRepository
    {
        public List<tblCourse> GetAllCourse();
        public List<tblData> GetAllData();
        public tblData GetOneData(int id);
        public void AddData(tblData data);
        public void UpdateData(tblData data);
        public void DeleteData(int id);
    }
}