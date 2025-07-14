using ElevatorSystem.Interfaces;
using ElevatorSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Services
{
    public class GlassElevator : ElevatorBase, IGlassElevator
    {
        private readonly string _viewDescription = "A beautiful view of the surrounding ocean and mountains through a glass elevator...";
        public GlassElevator(int id, int capacity) : base(id, capacity)
        {
            if (capacity > 12)
                throw new ArgumentException("Glass elevator capacity must not exceed 12.");
            if (id < 1)
                throw new ArgumentException("Glass elevator ID must be a positive integer.");
        }

        public void PrintView()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            GeneralHelper.WriteLine(_viewDescription);
            Logger.LogInfo(_viewDescription);
            Console.ResetColor();
        }
        public override string ToString()
        {
            return $"Glass Elevator with ID '{Id}'";
        }
    }
}
