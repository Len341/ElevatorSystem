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
        public static int GetUserNumberInput(string prompt)
        {
            while (true)
            {
                try
                {
                    GeneralHelper.WriteLine(prompt);
                    _ = int.TryParse(Console.ReadLine() ?? string.Empty, out int res);
                    if (res <= 0)
                    {
                        throw new ArgumentException("Please enter a valid positive integer.");
                    }
                    return res;
                }
                catch (ArgumentException argEx)
                {
                    GeneralHelper.WriteLine($"An error occurred: {argEx.Message}");
                    Logger.LogError("Invalid user input for number.");
                    continue;
                }
            }
        }
        public static ElevatorSystem.Services.ElevatorSystem ElevatorSystemSetup(
            int elevatorCount, 
            int elevatorCapacity)
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
                    GeneralHelper.WriteLine($"An error occurred: {ex.Message}");
                    GeneralHelper.WriteLine("Please try again.\n\n");
                    elevatorCount = GetUserNumberInput("Enter the number of elevators:");
                    elevatorCapacity = GetUserNumberInput("Enter the capacity of each elevator:");
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
                    GeneralHelper.WriteLine(ex.Message);
                    Logger.LogError("Invalid passenger unload count input.");
                    continue; // Prompt again for valid input
                }
            }

            return passengersUnloadedCount;
        }
    }
}
