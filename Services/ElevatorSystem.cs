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
        private static readonly object _lock = new();
        public ElevatorSystem(int elevatorCount, int capacity)
        {
            if (elevatorCount > 100)
                throw new ArgumentException("Elevator count must not exceed 100.");
            if (capacity > 20)
                throw new ArgumentException("Elevator capacity must not exceed 20.");

            var ids = Enumerable.Range(1, elevatorCount).Select(i => i).ToList();
            _elevators = ids.Select(id => new ElevatorBase(id, capacity)).Cast<IElevator>().ToList();

            //add two old elevators and one glass elevator
            _elevators.Add(new OldElevator(101, capacity));
            _elevators.Add(new OldElevator(102, capacity));
            _elevators.Add(new GlassElevator(103, capacity));
        }

        public Tuple<IElevator?, string> RequestElevator(PersonRequest request)
        {
            GeneralHelper.WriteLine($"Requesting elevator for {request.PeopleCount} people on floor {request.Floor}.");
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var elevator in _elevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor)))
            {
                if (elevator.Occupants + request.PeopleCount > elevator.Capacity && elevator.IsAvailable)
                {
                    sb.AppendLine($"{elevator.ToString()} is the {GeneralHelper.ToOrdinal(i)} closest but cannot accommodate the request of {request.PeopleCount} people. Current Occupants: {elevator.Occupants}, Capacity: {elevator.Capacity}");
                    Logger.LogInfo($"{elevator.ToString()} is the {GeneralHelper.ToOrdinal(i)} closest but cannot accommodate the request of {request.PeopleCount} people. Current Occupants: {elevator.Occupants}, Capacity: {elevator.Capacity}");
                }
                else if (!elevator.IsAvailable)
                {
                    sb.AppendLine($"{elevator.ToString()} is the {GeneralHelper.ToOrdinal(i)} closest but is not available.");
                    Logger.LogInfo($"{elevator.ToString()} is the {GeneralHelper.ToOrdinal(i)} closest but is not available.");
                }
                i++;
            }

            var elevatorsWithCapacityAndAvailability = _elevators
                .Where(e => e.Occupants + request.PeopleCount <= e.Capacity && e.IsAvailable)
                .OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor));

            IElevator nearestAvailableElevator = null;

            if (elevatorsWithCapacityAndAvailability != null && elevatorsWithCapacityAndAvailability.Any())
            {
                foreach (var elevator in elevatorsWithCapacityAndAvailability)
                {
                    if (elevator.AddRequest(request.Floor, request.PeopleCount))
                    {
                        nearestAvailableElevator = elevator;
                        break;
                    }
                }
            }
            return Tuple.Create(nearestAvailableElevator, sb.ToString());
        }

        public async Task<bool> Step()
        {
            var tasks = _elevators.Select(async elevator =>
            {
                bool idle = await Task.Run(() => elevator.Move());
                return !idle;
            });

            var results = await Task.WhenAll(tasks);
            return results.Any(moving => moving);
        }

        public void ShowStatus()
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                StringBuilder sb = new StringBuilder("\n*********************************************\n");
                foreach (var e in _elevators)
                {
                    sb.AppendLine($"{e.ToString()} '{e.Id}' at Floor {e.CurrentFloor}, Direction: {e.Direction}, Occupants: {e.Occupants}/{e.Capacity}");
                }
                sb.AppendLine("*********************************************\n");
                GeneralHelper.WriteLine($"{sb.ToString()}");
                Console.ResetColor();
            }
        }
    }
}
