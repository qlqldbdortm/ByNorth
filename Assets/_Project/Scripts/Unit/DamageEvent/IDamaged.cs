namespace ByNorth.Unit.DamageEvent
{
    public interface IDamaged
    {
        public void OnDamaged(Unit unit, int damage, DamageType damageType);
    }
}