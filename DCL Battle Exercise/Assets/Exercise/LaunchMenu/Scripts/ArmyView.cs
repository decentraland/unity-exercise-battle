using Exercise.Battle.Scripts;
using Exercise.Models.Scripts;
using Exercise.Utils;
using TMPro;
using UnityEngine;

namespace Exercise.LaunchMenu.Scripts
{
    public interface IArmyView
    {
        void UpdateWithModel(IArmyModel model);
    }

    public sealed class ArmyView : MonoBehaviour, IArmyView
    {
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TMP_Dropdown strategyDropdown;
        [SerializeField] private DivisionView divisionViewPrefab;
        [SerializeField] private GameObject divisionViewParent;
        private EnumDropdownWrapper<ArmyStrategy> enumDropdown;
        private IArmyPresenter presenter = null;

        private void Awake()
        {
            enumDropdown = new EnumDropdownWrapper<ArmyStrategy>(strategyDropdown);
            enumDropdown.OnValueChanged += OnStrategyChanged;
        }

        public void BindPresenter(IArmyPresenter presenter) => this.presenter = presenter;


        public void UpdateWithModel(IArmyModel model)
        {
            titleLabel.text = model.name;
            enumDropdown.SetValueWithoutNotify(model.strategy);
            foreach (var division in model.divisions)
            {
                var divisionView = Instantiate(divisionViewPrefab, divisionViewParent.transform, true);
                divisionView.gameObject.SetActive(true);
                var divisionPresenter = new DivisionPresenter(division, divisionView);
                divisionView.BindPresenter(divisionPresenter);
            }
        }

        private void OnStrategyChanged(ArmyStrategy strategy) => presenter?.UpdateStrategy(strategy);

        private void OnDestroy()
        {
            enumDropdown.OnValueChanged -= OnStrategyChanged;
            enumDropdown?.Dispose();
        }
    }
}