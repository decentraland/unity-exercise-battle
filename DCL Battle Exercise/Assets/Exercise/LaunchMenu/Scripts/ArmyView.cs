using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IArmyView
{
    void UpdateWithModel(IArmyModel model);
}

public class ArmyView : MonoBehaviour, IArmyView
{
    [SerializeField] private Slider warriorsCount;
    [SerializeField] private TextMeshProUGUI warriorsLabel;
    [SerializeField] private Slider archersCount;
    [SerializeField] private TextMeshProUGUI archersLabel;
    [SerializeField] private TMP_Dropdown strategyDropdown;

    private EnumDropdownWrapper<ArmyStrategy> enumDropdown;
    private IArmyPresenter presenter = null;

    private void Awake()
    {
        warriorsCount.onValueChanged.AddListener(OnWarriorsCountChanged);
        archersCount.onValueChanged.AddListener(OnArchersCountChanged);
        enumDropdown = new EnumDropdownWrapper<ArmyStrategy>(strategyDropdown);
        enumDropdown.OnValueChanged += OnStrategyChanged;
    }

    public void BindPresenter(IArmyPresenter presenter)
    {
        this.presenter = presenter;
    }

    public void UpdateWithModel(IArmyModel model)
    {
        warriorsCount.SetValueWithoutNotify(model.warriors);
        warriorsLabel.text = model.warriors.ToString();
        archersCount.SetValueWithoutNotify(model.archers);
        archersLabel.text = model.archers.ToString();
        enumDropdown.SetValueWithoutNotify(model.strategy);
    }

    private void OnWarriorsCountChanged(float value)
    {
        presenter?.UpdateWarriors((int)value);
        warriorsLabel.text = ((int)value).ToString();
    }

    private void OnArchersCountChanged(float value)
    {
        presenter?.UpdateArchers((int)value);
        archersLabel.text = ((int)value).ToString();
    }

    private void OnStrategyChanged(ArmyStrategy strategy)
    {
        presenter?.UpdateStrategy(strategy);
    }

    private void OnDestroy()
    {
        enumDropdown.OnValueChanged -= OnStrategyChanged;
        enumDropdown?.Dispose();
    }
}