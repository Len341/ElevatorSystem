using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Models
{
    public class PersonRequest
    {
        public int Floor { get; set; }
        public int PeopleCount { get; set; }
        public PersonRequest(int floor, int peopleCount)
        {
            Floor = floor;
            PeopleCount = peopleCount;
        }
    }
}
