using ElevatorSystem.Models;
using ElevatorSystem.Services;
using ElevatorSystem.Utils;
using System.Timers;

GeneralHelper.WriteLine("Welcome to the Elevator System\n");

int normalElevatorCount = ElevatorHelper.GetUserNumberInput("Enter the number of elevators:");
int elevatorCapacity = ElevatorHelper.GetUserNumberInput("Enter the capacity of each elevator:");
int floorCount = ElevatorHelper.GetUserNumberInput("Enter the number of floors in the building:");

ElevatorSystem.Services.ElevatorSystem system =
    ElevatorHelper.ElevatorSystemSetup(normalElevatorCount, elevatorCapacity);

List<Task> elevatorSystemTasks = new List<Task>();

System.Timers.Timer _timer = new System.Timers.Timer(3000); // 2 seconds
_timer.Elapsed += new ElapsedEventHandler((sender, e) => MoveElevators(sender, e, system)); ;
_timer.Start();

System.Timers.Timer _statusTimer = new System.Timers.Timer(250); // 2 seconds
_statusTimer.Elapsed += new ElapsedEventHandler((sender, e) => system.ShowStatus()); ;
_statusTimer.Start();

ConsoleKeyInfo cki;
var now = DateTime.Now;
var _fiveMinutesFromNow = now.AddMinutes(5);

while (now < _fiveMinutesFromNow)
{
    // Simulate a random request
    Random rand = new Random();
    int floor = rand.Next(0, floorCount + 5);
    int peopleCount = rand.Next(1, elevatorCapacity + 5);

    var elevatorTuple = system.RequestElevator(new PersonRequest(floor, peopleCount));
    if (elevatorTuple.Item1 == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        GeneralHelper.WriteLine(elevatorTuple.Item2);
        Console.ResetColor();
    }
    var nextRequestWaitTime = rand.Next(1000, 2000); // Random wait time between requests
    await Task.Delay(nextRequestWaitTime);
    now = DateTime.Now;
    Console.Clear();
    GeneralHelper.WriteLine("Welcome to the Elevator System\n");
    GeneralHelper.WriteLine($"Current Time: {now.ToShortTimeString()}");
    GeneralHelper.WriteLine($"Elevator Count: {normalElevatorCount}");
    GeneralHelper.WriteLine($"Elevator Capacity: {elevatorCapacity}");
    GeneralHelper.WriteLine($"Floor Count: {floorCount}");
}
// Wait for all elevator tasks to complete
await Task.WhenAll(elevatorSystemTasks);
_timer.Stop();


void MoveElevators(
    object? sender,
    ElapsedEventArgs e,
    ElevatorSystem.Services.ElevatorSystem system)
{
    elevatorSystemTasks.Add(system.Step());
}