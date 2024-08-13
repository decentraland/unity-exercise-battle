namespace Exercise.Battle.Scripts
{
    public sealed class Archer : UnitBase
    {
        public void Setup(Army army)
        {
            _army = army;
            _attackBehavior = GetComponent<AttackBehavior>();
        }
    }
}