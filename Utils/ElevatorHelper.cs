using ElevatorSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Utils
{
    public static class ElevatorHelper
    {
        public static int GetElevatorCapacityFromUser()
        {
            int capacity;
            while (true)
            {
                try
                {
                    capacity = Console.ReadLine() switch
                    {
                        string s when int.TryParse(s, out int cap) && cap > 0 => cap,
                        _ => throw new ArgumentException("Please enter a valid positive integer.")
                    };
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Logger.LogError("Invalid elevator capacity input.");
                    continue;
                }
                break;
            }
            return capacity;
        }
        public static int GetElevatorsCountFromUser()
        {
            int elevatorCount;
            while (true)
            {
                try
                {
                    elevatorCount = Console.ReadLine() switch
                    {
                        string s when int.TryParse(s, out int count) && count > 0 => count,
                        _ => throw new ArgumentException("Please enter a valid positive integer.")
                    };
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Logger.LogError("Invalid elevator count input.");
                    continue;
                }
                break;
            }

            return elevatorCount;
        }
        public static void ElevatorSetupInput(out int elevatorCount, out int elevatorCapacity)
        {
            Console.WriteLine("How many elevators are there ?");
            elevatorCount = GetElevatorsCountFromUser();
            Console.WriteLine("How many people can each elevator carry ?");
            elevatorCapacity = GetElevatorCapacityFromUser();
        }
        public static ElevatorSystem.Services.ElevatorSystem ElevatorSystemSetup(ref int elevatorCount, ref int elevatorCapacity)
        {
            ElevatorSystem.Services.ElevatorSystem system;
            while (true)
            {
                try
                {
                    system = new ElevatorSystem.Services.ElevatorSystem(elevatorCount: elevatorCount, capacity: elevatorCapacity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Please try again.\n\n");
                    ElevatorSetupInput(out elevatorCount, out elevatorCapacity);
                    Logger.LogError("Failed to create ElevatorSystem with the provided parameters.");
                    continue;
                }
                break;
            }

            return system;
        }
        public static int GetPassengerUnloadCount(int passengersUnloadedCount, int Occupants)
        {
            while (true)
            {
                try
                {
                    passengersUnloadedCount = Console.ReadLine() switch
                    {
                        string s when int.TryParse(s, out int count) && count > 0 && count <= Occupants => count,
                        _ => throw new ArgumentException($"Please enter a valid number of passengers to unload. You cannot unload more passengers than what is currently in the elevator ({Occupants})")
                    };
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    Logger.LogError("Invalid passenger unload count input.");
                    continue; // Prompt again for valid input
                }
            }

            return passengersUnloadedCount;
        }
    }
}
