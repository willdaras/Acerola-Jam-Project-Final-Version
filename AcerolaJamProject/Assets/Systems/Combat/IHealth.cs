namespace Combat
{
    public interface IHealth : IDamageable, IInvincibility
    {
        int Health { get; }
    }
}