using ElevatorSystem.Utils;
using ElevatorSystem.Models;


int elevatorCount = 0;
int elevatorCapacity = 0;
ElevatorSystem.Services.ElevatorSystem system = null;

Console.WriteLine("Welcome to the Elevator System\n");
Console.WriteLine("Press ESC to stop the simulation...\n\n");

ElevatorHelper.ElevatorSetupInput(out elevatorCount, out elevatorCapacity);

system = ElevatorHelper.ElevatorSystemSetup(ref elevatorCount, ref elevatorCapacity);

while (true)
{
    // allow the user to request an elevator
    Console.WriteLine("Press 'R' to request an elevator or 'S' to view all the elevators status:\n");
    var input = Console.ReadKey(true).Key;
    if (input == ConsoleKey.S)
    {
        system.ShowStatus();
        continue; // Continue to allow further requests
    }
    else if (input == ConsoleKey.R)
    {
        system.ShowStatus();
        Console.WriteLine("\nEnter the floor number you are on:");
        if (int.TryParse(Console.ReadLine(), out int floor) && floor >= 0)
        {
            Console.WriteLine("Enter the number of people requesting the elevator:");
            if (int.TryParse(Console.ReadLine(), out int peopleCount) && peopleCount > 0)
            {
                var elevatorTuple = system.RequestElevator(new PersonRequest(floor, peopleCount));
                var elevator = elevatorTuple.Item1;
                var result = elevatorTuple.Item2;
                if (!string.IsNullOrEmpty(result))
                {
                    Console.WriteLine(result);
                }

                if (elevator == null)
                {
                    Console.WriteLine("No elevator can accommodate your request at this time.");
                    Logger.LogInfo("No elevator available for the request.");
                }
                else
                {
                    Console.WriteLine($"Elevator {elevator.Id} has been requested to floor {floor} for {peopleCount} people.");
                    Logger.LogInfo($"Elevator {elevator.Id} requested to floor {floor} for {peopleCount} people.");

                    while (!elevator.Move())
                    {
                        // Continue moving the elevator until it reaches its destination
                        system.ShowStatus();
                        System.Threading.Thread.Sleep(1000); // Simulate time taken to move
                    }
                    elevator.LoadPassengers(peopleCount);
                    string peopleString = peopleCount > 1 ? "people" : "person";
                    Console.WriteLine($"Elevator '{elevator.Id}' has loaded {peopleCount} {peopleString}.");
                    Console.WriteLine($"Please select the destination floor for elevator '{elevator.Id}'");
                    Logger.LogInfo($"Elevator {elevator.Id} loaded {peopleCount} {peopleString}.");
                    int targetFloor;
                    while (true)
                    {
                        try
                        {
                            targetFloor = Console.ReadLine() switch
                            {
                                string s when int.TryParse(s, out int target) && target >= 0 && target != elevator.CurrentFloor => target,
                                _ => throw new ArgumentException("Please enter a valid floor number. It also cannot be the same as current floor.")
                            };
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Logger.LogError(ex.Message);
                            continue; // Prompt again for valid input
                        }
                        break;
                    }
                    elevator.AddRequest(targetFloor);
                    while (!elevator.Move())
                    {
                        // Continue moving the elevator until it reaches its destination
                        system.ShowStatus();
                        System.Threading.Thread.Sleep(1000); // Simulate time taken to move
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid number of people. Please try again.");
                Logger.LogError("Invalid number of people requested.");
            }
        }
        else
        {
            Console.WriteLine("Invalid floor number. Please try again.");
            Logger.LogError("Invalid floor number entered.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Press 'R' to request an elevator.");
        Logger.LogError("Invalid input received. Expected 'R' or 'S'");
    }
}