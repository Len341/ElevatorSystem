using ElevatorSystem.Models;
using ElevatorSystem.Services;

namespace ElevatorSystem.Tests
{
    public class ElevatorTests
    {
        private ElevatorBase _elevator;
        private GlassElevator _glassElevator;
        private OldElevator _oldElevator;
        int capacity = 5;
        int id = 1;
        [SetUp]
        public void Setup()
        {
            _elevator = new ElevatorBase(capacity: capacity, id: id);
            _glassElevator = new GlassElevator(capacity: capacity, id: id);
            _oldElevator = new OldElevator(capacity: capacity, id: id);
        }

        [Test]
        public void Constructor_InitializesCorrectly()
        {
            Assert.That(_elevator.Id, Is.EqualTo(id));
            Assert.That(_elevator.CurrentFloor, Is.EqualTo(0));
            Assert.That(_elevator.Occupants, Is.EqualTo(0));
            Assert.That(_elevator.Direction, Is.EqualTo(Direction.Idle));
        }

        [Test]
        public void AddPeople_DoesNotExceedCapacity()
        {
            _elevator.LoadPassengers(capacity + 1);
            Assert.That(_elevator.Occupants, Is.EqualTo(capacity));
        }

        [Test]
        public void AddPeople_SucceedsWhenUnderLimit()
        {
            _elevator.LoadPassengers(capacity - 1);
            Assert.That(_elevator.Occupants, Is.EqualTo(capacity - 1));
        }

        [Test]
        public void MoveToFloor_UpdatesCurrentFloorAndDirection()
        {
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(TextWriter.Null);
                int initialFloor = _elevator.CurrentFloor;
                int targetFloorTop = 7;
                int targetFloorBottom = 4;
                _elevator.AddRequest(targetFloorTop, 1);
                while (!_elevator.Move())
                {
                    Assert.That(_elevator.CurrentFloor, Is.EqualTo(initialFloor + 1));
                    Assert.That(_elevator.Direction, Is.EqualTo(Direction.Up));
                    initialFloor = _elevator.CurrentFloor;
                }

                _elevator.AddRequest(targetFloorBottom, 1);
                initialFloor = _elevator.CurrentFloor;
                while (!_elevator.Move())
                {
                    Assert.That(_elevator.CurrentFloor, Is.EqualTo(initialFloor - 1));
                    Assert.That(_elevator.Direction, Is.EqualTo(Direction.Down));
                    initialFloor = _elevator.CurrentFloor;
                }
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Test]
        public void UnloadPassengers_UnloadMoreThanExists_DoesNotThrowError()
        {
            Assert.DoesNotThrow(() => _elevator.UnloadPassengers(1));
        }
        [Test]
        public void OldElevator_ThrowsException_WhenCapacityExceedsLimit()
        {
            Assert.Throws<ArgumentException>(() => new OldElevator(id: 1, capacity: 11));
        }
        [Test]
        public void GlassElevator_ThrowsException_WhenCapacityExceedsLimit()
        {
            Assert.Throws<ArgumentException>(() => new GlassElevator(id: 1, capacity: 13));
        }
        [Test]
        public void OldElevator_ThrowsException_WhenIdIsLessThanOne()
        {
            Assert.Throws<ArgumentException>(() => new OldElevator(id: 0, capacity: 5));
        }
        [Test]
        public void GlassElevator_ThrowsException_WhenIdIsLessThanOne()
        {
            Assert.Throws<ArgumentException>(() => new GlassElevator(id: 0, capacity: 5));
        }
        [Test]
        public void OldElevator_Move_SimulatesTimeTakenToMove()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _oldElevator.Move();
            stopwatch.Stop();
            Assert.That(stopwatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(2000));
        }
        [Test]
        public void GlassElevator_PrintView_PrintsCorrectly()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                _glassElevator.PrintView();
                var output = sw.ToString().Trim();
                Assert.That(output, Is.EqualTo("A beautiful view of the surrounding ocean and mountains through a glass elevator..."));
            }
        }
        [Test]
        public void ElevatorBase_ToString_ReturnsCorrectFormat()
        {
            Assert.That(_elevator.ToString(), Is.EqualTo($"Base Elevator with ID '{id}'"));
            Assert.That(_glassElevator.ToString(), Is.EqualTo($"Glass Elevator with ID '{id}'"));
            Assert.That(_oldElevator.ToString(), Is.EqualTo($"Old Elevator with ID '{id}'"));
        }
        [Test]
        public void ElevatorBase_AddRequest_AddsRequestSuccessfully()
        {
            Assert.IsTrue(_elevator.AddRequest(3, 2));
            Assert.IsFalse(_elevator.AddRequest(3, 2)); // Duplicate request should not be added
            Assert.IsTrue(_elevator.AddRequest(5, 1)); // New request should be added
        }
        [Test]
        public void ElevatorBase_IsAvailable_ReturnsTrue_WhenIdleAndNoRequests()
        {
            Assert.IsTrue(_elevator.IsAvailable);
            _elevator.AddRequest(3, 2);
            Assert.IsFalse(_elevator.IsAvailable); // Should be false when there are requests
            while (!_elevator.Move())
            {

            } // Move the elevator all the way to process the request
            Assert.IsTrue(_elevator.IsAvailable); // Should be true again after processing
        }
        [Test]
        public void ElevatorBase_UnloadPassengers_UnloadsAllPassengers()
        {
            _elevator.LoadPassengers(3);
            Assert.That(_elevator.Occupants, Is.EqualTo(3));
            _elevator.UnloadPassengers(3);
            Assert.That(_elevator.Occupants, Is.EqualTo(0)); // All passengers should be unloaded
        }
        [Test]
        public void ElevatorBase_LoadPassengers_IncreasesOccupantsCorrectly()
        {
            _elevator.LoadPassengers(2);
            Assert.That(_elevator.Occupants, Is.EqualTo(2));
            _elevator.LoadPassengers(3);
            Assert.That(_elevator.Occupants, Is.EqualTo(5)); // Should not exceed capacity
            _elevator.LoadPassengers(1);
            Assert.That(_elevator.Occupants, Is.EqualTo(5)); // Still should not exceed capacity
        }
    }
}