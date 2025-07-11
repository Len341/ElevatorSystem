using ElevatorSystem.Models;

namespace ElevatorSystem.Interfaces
{
    public interface IElevator
    {
        bool Move();
        void AddRequest(int floor);
        int Id { get; }
        int CurrentFloor { get; }
        Direction Direction { get; }
        int Capacity { get; }
        int Occupants { get; }
    }
}
