using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Services
{
    public class OldElevator : ElevatorBase
    {
        public OldElevator(int id, int capacity) : base(id, capacity)
        {
            if (capacity > 10)
                throw new ArgumentException("Old elevator capacity must not exceed 10.");
            if (id < 1)
                throw new ArgumentException("Old elevator ID must be a positive integer.");
        }
        public override bool Move()
        {
            Thread.Sleep(2000);// Simulate time taken to move
            return base.Move();
        }
        public override string ToString()
        {
            return "Old Elevator";
        }
    }
}
