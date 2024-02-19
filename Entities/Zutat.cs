using Rezeptmanager.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rezeptmanager.Entities
{
    public class Zutat
    {
        public int Id { get; set; }

        public int Count { get; set; } 
        public string Name { get; set; }
    }
}
