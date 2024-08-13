using Exercise.Models.Scripts;

namespace Exercise.LaunchMenu.Scripts
{
    public interface IArmyPresenter
    {
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

        public void UpdateStrategy(ArmyStrategy strategy) => model.strategy = strategy;
    }
}