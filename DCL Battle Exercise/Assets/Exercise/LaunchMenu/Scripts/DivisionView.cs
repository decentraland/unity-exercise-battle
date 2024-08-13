using Exercise.Models.Scripts;
using Exercise.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Exercise.LaunchMenu.Scripts
{
    public interface IDivisionView
    {
        void UpdateWithModel(ArmyDivision model);
    }

    public sealed class DivisionView : MonoBehaviour, IDivisionView
    {
        [SerializeField] private TextMeshProUGUI divisionLabel;
        [SerializeField] private Slider divisionCount;
        [SerializeField] private TMP_Dropdown typeDropdown;

        private EnumDropdownWrapper<UnitType> enumDropdown;
        private IDivisionPresenter presenter = null;

        private void Awake()
        {
            divisionCount.onValueChanged.AddListener(OnDivisionCountChanged);
            enumDropdown = new EnumDropdownWrapper<UnitType>(typeDropdown);
            enumDropdown.OnValueChanged += OnTypeChanged;
        }

        public void BindPresenter(IDivisionPresenter presenter) => this.presenter = presenter;


        public void UpdateWithModel(ArmyDivision model)
        {
            enumDropdown.SetValueWithoutNotify(model.type);
            divisionCount.SetValueWithoutNotify(model.count);
            divisionLabel.text = (model.count).ToString();
        }

        private void OnDivisionCountChanged(float value)
        {
            presenter?.UpdateCount((int)value);
            divisionLabel.text = ((int)value).ToString();
        }

        private void OnTypeChanged(UnitType type) => presenter?.UpdateType(type);

        private void OnDestroy()
        {
            enumDropdown.OnValueChanged -= OnTypeChanged;
            enumDropdown?.Dispose();
        }
    }
}