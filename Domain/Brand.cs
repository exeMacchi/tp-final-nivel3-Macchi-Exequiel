using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Brand
    {
        // Propiedades
        public int ID { get; set; }
        public string Description { get; set; }

        public Brand(int id, string description)
        {
            ID = id;
            Description = description;
        }
    }
}
