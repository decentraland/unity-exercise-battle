using Exercise.Models.Scripts;

namespace Exercise.LaunchMenu.Scripts
{
    public interface IArmyPresenter
    {
        void UpdateWarriors(int warriors);
        void UpdateArchers(int archers);
        void UpdateStrategy(ArmyStrategy strategy);
    }

    public sealed class ArmyPresenter : IArmyPresenter
    {
        private readonly IArmyModel model;
        private readonly IArmyView view;

        public ArmyPresenter(IArmyModel model, IArmyView view)
        {
            this.model = model;
            this.view = view;
            this.view.UpdateWithModel(model);
        }

        public void UpdateWarriors(int warriors) => model.warriors = warriors;

        public void UpdateArchers(int archers) => model.archers = archers;

        public void UpdateStrategy(ArmyStrategy strategy) => model.strategy = strategy;
    }
}