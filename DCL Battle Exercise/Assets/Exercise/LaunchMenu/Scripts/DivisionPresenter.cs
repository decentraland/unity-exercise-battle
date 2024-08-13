using Exercise.Models.Scripts;

namespace Exercise.LaunchMenu.Scripts
{
    public interface IDivisionPresenter
    {
        void UpdateCount(int count);
        void UpdateType(UnitType type);
    }

    public sealed class DivisionPresenter : IDivisionPresenter
    {
        private ArmyDivision model;
        private readonly IDivisionView view;

        public DivisionPresenter(ArmyDivision model, IDivisionView view)
        {
            this.model = model;
            this.view = view;
            this.view.UpdateWithModel(model);
        }

        public void UpdateCount(int count) => model.count = count;
        public void UpdateType(UnitType type) => model.type = type;
    }
}