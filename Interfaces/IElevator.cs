using ElevatorSystem.Models;

namespace ElevatorSystem.Interfaces
{
    public interface IElevator
    {
        bool Move();
        bool AddRequest(int floor);
        int Id { get; }
        int CurrentFloor { get; }
        Direction Direction { get; }
        int Capacity { get; }
        int Occupants { get; }
    }
}
