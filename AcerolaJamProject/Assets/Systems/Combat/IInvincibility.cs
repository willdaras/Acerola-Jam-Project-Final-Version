namespace Combat
{
    public interface IInvincibility
    {
        bool Invincible { get; }

        void BecomeInvincible();
        void BecomeInvincible(float time);
        void DisableInvincibility();
    }
}