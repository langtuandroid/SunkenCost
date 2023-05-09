namespace Enemies
{
    public class EnemyMove
    {
        public MovementType MovementType { get; private set; }
        public int Magnitude { get; private set; }

        public EnemyMove(MovementType movementType, int magnitude)
        {
            MovementType = movementType;
            Magnitude = magnitude;
        }

        public void IncreaseMagnitude(int amountToIncrease)
        {
            if (Magnitude == 0) return;

            if (Magnitude > 0) Magnitude += amountToIncrease;
            else Magnitude -= amountToIncrease;
        }
    }
}