using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyMenu : MonoBehaviour
{
    public BattleSettings settings;
    public int armyId;
    public Slider warriorsCount;
    public Slider archersCount;
    public TMPro.TextMeshProUGUI warriorsCountLabel;
    public TMPro.TextMeshProUGUI archersCountLabel;
    public Toggle defensiveStrategyToggle;
    public Toggle basicStrategyToggle;

    private BattleSettings.ArmySettings armySettings;

    void Start()
    {
        if ( armyId == 1 )
            armySettings = settings.army1Settings;
        else
            armySettings = settings.army2Settings;

        warriorsCount.value = armySettings.warriorCount;
        archersCount.value = armySettings.archerCount;
        defensiveStrategyToggle.isOn = armySettings.armyStrategy == BattleSettings.ArmyStrategy.StrategyDefensive;
        basicStrategyToggle.isOn = armySettings.armyStrategy == BattleSettings.ArmyStrategy.StrategyBasic;
    }

    void Update()
    {
        warriorsCountLabel.text = warriorsCount.value.ToString();
        archersCountLabel.text = archersCount.value.ToString();

        warriorsCount.maxValue = 100;
        archersCount.maxValue = 100;

        armySettings.archerCount = (int)archersCount.value;
        armySettings.warriorCount = (int)warriorsCount.value;

        armySettings.armyStrategy = defensiveStrategyToggle.isOn ? BattleSettings.ArmyStrategy.StrategyDefensive : BattleSettings.ArmyStrategy.StrategyBasic;
    }
}