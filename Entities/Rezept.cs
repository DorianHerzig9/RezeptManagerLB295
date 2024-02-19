using Rezeptmanager.Dtos;
using Rezeptmanager.Entities.Enums;
using System;

namespace Rezeptmanager.Entities
{
    public class Rezept
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Zutat> Zutaten { get; set; }
        public string Dauer { get; set; }
        public SchwierigkeitsGrad SchwierigkeitsGrad { get; set; }
    }

    public static class RezeptMapper
    {
        public static Rezept Map(RezeptDto dto)
        {
            return new Rezept
            {
                Dauer = dto.Dauer,
                Name = dto.Name,
                SchwierigkeitsGrad = dto.SchwierigkeitsGrad,
                Zutaten = dto.Zutaten
            };
        }
    }
}
