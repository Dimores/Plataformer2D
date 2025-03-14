namespace Orby.Interfaces
{
    public interface IDamageable
    {
        public void Damage(int damageAmount);

        public int MaxHealth { get; set; }

        public int CurrentHealth { get; set; }
    }
}