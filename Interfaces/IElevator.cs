using ElevatorSystem.Models;

namespace ElevatorSystem.Interfaces
{
    public interface IElevator
    {
        public abstract bool Move();
        bool AddRequest(int floor, int people);
        int Id { get; }
        bool IsAvailable { get; }
        int CurrentFloor { get; }
        Direction Direction { get; }
        int Capacity { get; }
        int Occupants { get; }
    }
}
