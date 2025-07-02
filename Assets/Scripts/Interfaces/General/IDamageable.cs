namespace Orby.Interfaces.General
{
    public interface IDamageable
    {
        public void Damage(int damageAmount);

        public int MaxHealth { get; set; }

        public int CurrentHealth { get; set; }
    }
}