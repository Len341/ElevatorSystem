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
    public class ElevatorBase : IElevator
    {
        private readonly Queue<(int, int)> _floorRequests = new();
        public int CurrentFloor { get; private set; }
        public Direction Direction { get; private set; } = Direction.Idle;
        public int Capacity { get; }
        public int Occupants { get; private set; }
        public bool IsAvailable => Direction == Direction.Idle && _floorRequests.Count == 0;
        public int Id { get; private set; }

        public ElevatorBase(int id, int capacity)
        {
            Capacity = capacity;
            Id = id;
        }

        public override string ToString()
        {
            return $"Base Elevator with ID '{Id}'";
        }

        public bool AddRequest(int floor, int people)
        {
            if (!_floorRequests.Any(z => z.Item1 == floor))
            {
                _floorRequests.Enqueue((floor, people));
                return true;
            }
            return false; // Request already exists
        }

        public void LoadPassengers(int count)
        {
            var _occupants = Occupants;
            Occupants = Math.Min(Capacity, Occupants + count);
            int passengerCountLoaded = Occupants - _occupants;
            Console.ForegroundColor = ConsoleColor.Green;
            GeneralHelper.WriteLine(
                $"{ToString()} loading '{passengerCountLoaded}' passenger(s) at floor {CurrentFloor}...");
            Console.ResetColor();
        }

        public void UnloadPassengers(int passengersUnloadedCount)
        {
            //if (passengersUnloadedCount > Occupants) Occupants = 0;
            //else Occupants -= passengersUnloadedCount;
            Occupants = 0; // Unload all passengers 
        }

        public virtual bool Move()
        {
            //return a bool determining if the elevator is still moving or destination reached

            if (_floorRequests.Count == 0)
            {
                Direction = Direction.Idle;
                return true;
            }
            var floorRequest = _floorRequests.Peek();
            var targetFloor = floorRequest.Item1;
            var peopleCount = floorRequest.Item2;
            if (this is IGlassElevator)
            {
                ((IGlassElevator)this).PrintView();
            }
            if (targetFloor > CurrentFloor)
            {
                CurrentFloor++;
                Direction = Direction.Up;
                Logger.LogInfo($"{ToString()} moving up to floor {CurrentFloor}...");
                return false; // Still moving
            }
            else if (targetFloor < CurrentFloor)
            {
                CurrentFloor--;
                Direction = Direction.Down;
                Logger.LogInfo($"{ToString()} moving down to floor {CurrentFloor}...");
                return false; // Still moving
            }
            else
            {
                // Reached the target floor, unload passengers
                _floorRequests.Dequeue();
                if (Occupants > 0)
                {
                    //passengersUnloadedCount = ElevatorHelper.GetPassengerUnloadCount(passengersUnloadedCount, Occupants);
                    int passengersToUnload = new Random().Next(1, Occupants + 1);
                    UnloadPassengers(passengersToUnload);
                    Console.ForegroundColor = ConsoleColor.Green;
                    GeneralHelper.WriteLine($"{ToString()} unloading passengers at floor {CurrentFloor}...");
                    Console.ResetColor();
                }
                else
                {
                    GeneralHelper.WriteLine($"{ToString()} arrived at floor {CurrentFloor} with no occupants.");
                }
                LoadPassengers(peopleCount);
                Direction = _floorRequests.Count > 0 ? Direction : Direction.Idle;
                return true; // Destination reached
            }
        }
    }
}
