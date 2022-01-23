using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIsystem.Models
{
    public class Productos 
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string Descripcion { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }
        
        public bool Existencia { get; set; }

        public bool isDeleted { get; set; }

    }
}
