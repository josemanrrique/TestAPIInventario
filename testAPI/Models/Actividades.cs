using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIsystem.Models
{
    public class Actividades
    {
        [Key] 
        public int Id { get; set; }
        
        [Required]
        public int PrductoID { get; set; }
       
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public TypeOperation Operation { get; set; }

        [Required]
        public string field { get; set; }

        [Required]
        public string Before { get; set; }
        
        [Required]
        public string After { get; set; }
    }
}