using Microsoft.EntityFrameworkCore;
using System;

namespace ListaDeCompra.Models
{
    public class Produtos
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Secret { get; set; }
    }  
}
