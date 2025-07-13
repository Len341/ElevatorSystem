using ElevatorSystem.Models;
using ElevatorSystem.Services;
using ElevatorSystem.Utils;
using System.Timers;

Console.WriteLine("Welcome to the Elevator System\n");

int elevatorCount = ElevatorHelper.GetUserNumberInput("Enter the number of elevators:");
int elevatorCapacity = ElevatorHelper.GetUserNumberInput("Enter the capacity of each elevator:");
int floorCount = ElevatorHelper.GetUserNumberInput("Enter the number of floors in the building:");

ElevatorSystem.Services.ElevatorSystem system =
    ElevatorHelper.ElevatorSystemSetup(elevatorCount, elevatorCapacity);

//instead of asking the user for input, we can simulate the elevator system
//for a certain number of iterations

List<Task> elevatorSystemTasks = new List<Task>();

System.Timers.Timer _timer = new System.Timers.Timer(2000); // 2 seconds
_timer.Elapsed += new ElapsedEventHandler((sender, e) => MoveElevators(sender, e, system)); ;
_timer.Start();

ConsoleKeyInfo cki;
var now = DateTime.Now;
var _fiveMinutesFromNow = now.AddMinutes(5);

while (now < _fiveMinutesFromNow)
{
    // Simulate a random request
    Random rand = new Random();
    int floor = rand.Next(0, floorCount + 1);
    int peopleCount = rand.Next(1, elevatorCapacity + 1);

    var elevatorTuple = system.RequestElevator(new PersonRequest(floor, peopleCount));


    var nextRequestWaitTime = rand.Next(1000, 5000); // Random wait time between requests
    await Task.Delay(nextRequestWaitTime);
    now = DateTime.Now;
}

void MoveElevators(
    object? sender,
    ElapsedEventArgs e,
    ElevatorSystem.Services.ElevatorSystem system)
{
    elevatorSystemTasks.Add(system.Step());
}

static void PrintLeft(string message)
{
    Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
    Console.WriteLine(message);
}

static void PrintRight(string message)
{
    Console.SetCursorPosition(Console.WindowWidth / 2, Console.GetCursorPosition().Top);
    Console.WriteLine(message);
}