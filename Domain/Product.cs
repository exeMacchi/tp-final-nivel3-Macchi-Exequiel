using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Product
    {
        // Propiedades
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        // Constructor
        public Product()
        {
            Brand = new Brand();
            Category = new Category();
        }
    }
}
