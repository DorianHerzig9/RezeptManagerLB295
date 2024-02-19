using Rezeptmanager.Entities;
using Rezeptmanager.Entities.Enums;

namespace Rezeptmanager.Dtos
{
    public class RezeptDto
    {
        public string Name { get; set; }
        public List<Zutat> Zutaten { get; set; }
        public string Dauer { get; set; }
        public SchwierigkeitsGrad SchwierigkeitsGrad { get; set; }
    }
}
