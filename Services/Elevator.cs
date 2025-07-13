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
    public class Elevator : IElevator
    {
        private readonly Queue<int> _floorRequests = new();
        public int CurrentFloor { get; private set; }
        public Direction Direction { get; private set; } = Direction.Idle;
        public int Capacity { get; }
        public int Occupants { get; private set; }

        public int Id { get; private set; }

        public Elevator(int id, int capacity)
        {
            Capacity = capacity;
            Id = id;
        }

        public bool AddRequest(int floor)
        {
            if (!_floorRequests.Contains(floor))
            {
                _floorRequests.Enqueue(floor);
                return true;
            }
            return false; // Request already exists
        }

        public void LoadPassengers(int count)
        {
            Occupants = Math.Min(Capacity, Occupants + count);
        }

        public void UnloadPassengers(int passengersUnloadedCount)
        {
            if (passengersUnloadedCount > Occupants) Occupants = 0;
            else Occupants -= passengersUnloadedCount;
        }

        public bool Move()
        {
            //return a bool dtermining if the elevator is still moving or destination reached
            if (_floorRequests.Count == 0)
            {
                Direction = Direction.Idle;
                return true;
            }

            int targetFloor = _floorRequests.Peek();

            if (targetFloor > CurrentFloor)
            {
                CurrentFloor++;
                Direction = Direction.Up;
                Console.WriteLine($"Elevator '{Id}' moving up to floor {CurrentFloor}...");
                Logger.LogInfo($"Elevator {Id} moving up to floor {CurrentFloor}...");
                return false; // Still moving
            }
            else if (targetFloor < CurrentFloor)
            {
                CurrentFloor--;
                Direction = Direction.Down;
                Console.WriteLine($"Elevator '{Id}' moving down to floor {CurrentFloor}...");
                Logger.LogInfo($"Elevator {Id} moving down to floor {CurrentFloor}...");
                return false; // Still moving
            }
            else
            {
                // Reached the target floor, unload passengers
                _floorRequests.Dequeue();
                if (Occupants > 0)
                {
                    int passengersUnloadedCount = 0;
                    Console.WriteLine($"How many passengers are exiting the elevator?\n");
                    passengersUnloadedCount = ElevatorHelper.GetPassengerUnloadCount(passengersUnloadedCount, Occupants);
                    UnloadPassengers(passengersUnloadedCount);
                    Console.WriteLine($"Elevator '{Id}' unloading '{passengersUnloadedCount}' passenger(s) at floor {CurrentFloor}...");
                }
                else
                {
                    Console.WriteLine($"Elevator '{Id}' arrived at floor {CurrentFloor} with no occupants.");
                }
                Direction = _floorRequests.Count > 0 ? Direction : Direction.Idle;
                return true; // Destination reached
            }
        }
    }
}
