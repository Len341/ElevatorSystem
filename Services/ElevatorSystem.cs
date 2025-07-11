using ElevatorSystem.Interfaces;
using ElevatorSystem.Models;
using ElevatorSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Services
{
    public class ElevatorSystem
    {
        private readonly List<IElevator> _elevators;
        public ElevatorSystem(int elevatorCount, int capacity)
        {
            if (elevatorCount > 100)
                throw new ArgumentException("Elevator count must not exceed 100.");
            if (capacity > 20)
                throw new ArgumentException("Elevator capacity must not exceed 20.");

            var ids = Enumerable.Range(1, elevatorCount).Select(i => i).ToList();
            _elevators = ids.Select(id => new Elevator(id, capacity)).Cast<IElevator>().ToList();
        }

        public Tuple<Elevator?, string> RequestElevator(PersonRequest request)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var elevator in _elevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor)))
            {
                if (elevator.Occupants + request.PeopleCount > elevator.Capacity)
                {
                    sb.AppendLine($"Elevator '{elevator.Id}' is the {GeneralHelper.ToOrdinal(i)} closest but cannot accommodate the request of {request.PeopleCount} people. Current Occupants: {elevator.Occupants}, Capacity: {elevator.Capacity}\n");
                    Logger.LogInfo($"Elevator '{elevator.Id}' is the {GeneralHelper.ToOrdinal(i)} closest but cannot accommodate the request of {request.PeopleCount} people. Current Occupants: {elevator.Occupants}, Capacity: {elevator.Capacity}");
                }
                i++;
            }

            var nearestElevatorWithCapacity = _elevators
                .Where(e => e.Occupants + request.PeopleCount <= e.Capacity)
                .OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor))
                .FirstOrDefault();

            if (nearestElevatorWithCapacity != null)
            {
                nearestElevatorWithCapacity.AddRequest(request.Floor);
            }
            return Tuple.Create((Elevator?)nearestElevatorWithCapacity, sb.ToString());
        }

        public void Step()
        {
            foreach (var elevator in _elevators)
            {
                elevator.Move();
            }
        }

        public void ShowStatus()
        {
            Console.WriteLine("*********************************************************");
            foreach (var e in _elevators)
            {
                Console.WriteLine($"Elevator '{e.Id}' at Floor {e.CurrentFloor}, Direction: {e.Direction}, Occupants: {e.Occupants}/{e.Capacity}");
            }
            Console.WriteLine("*********************************************************\n");
        }
    }
}
