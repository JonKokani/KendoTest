using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class tblData
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string gender { get; set; }
        public List<string> hobbies { get; set; }
        public int cid { get; set; }
        public string cname { get; set; }
        public IFormFile imgFile { get; set; }
        public string imgPath { get; set; } 
    }
}