using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class Module
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public string Commentary { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Days: {2}, Commentary: {3}, Price: {4}", Id, Name, Days, Commentary, Price);
        }
    }
}
