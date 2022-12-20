namespace Etchings
{
    public abstract class CharPreMovementActivatedEffect : ActiveEtching
    {
        public bool DetectCharacterAboutToMove()
        {
            var enemy = ActiveEnemiesManager.current.CurrentEnemy;
            
            // Deactivated
            if (deactivationTurns > 0) return false;
            
            // GAMEOVER?
            if (enemy.StickNum >= StickManager.current.stickCount + 1) return false;
            
            if ((UsesUsedThisTurn < UsesPerTurn || design.Limitless) && TestCharAboutToMoveActivatedEffect())
            {
                StartCoroutine(ColorForActivate());
                Log.current.AddEvent(design.Title + " on S" + Stick.GetStickNumber() + " activates against E" + enemy.name +
                                     " on S" + enemy.StickNum);
            
                enemy.Stick.SetTempColour(design.Color);
                designInfo.Refresh();
                return true;
            }

            return false;
        }

        protected abstract bool TestCharAboutToMoveActivatedEffect();
    }
}
