namespace Etchings
{
    public abstract class CharPreMovementActivatedEffect : Etching
    {
        public bool DetectCharacterAboutToMove()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;
            
            // Deactivated
            if (deactivationTurns > 0) return false;
            
            // GAMEOVER?
            if (enemy.StickNum >= PlankMap.current.stickCount + 1) return false;
            
            if ((UsesUsedThisTurn < UsesPerTurn || design.Limitless) && TestCharAboutToMoveActivatedEffect())
            {
                StartCoroutine(ColorForActivate());
                Log.current.AddEvent(design.Title + " on S" + Plank.GetPlankNum() + " activates against E" + enemy.name +
                                     " on S" + enemy.StickNum);
            
                enemy.Plank.SetTempColour(design.Color);
                etchingDisplay.UpdateDisplay();
                return true;
            }

            return false;
        }

        protected abstract bool TestCharAboutToMoveActivatedEffect();
    }
}
