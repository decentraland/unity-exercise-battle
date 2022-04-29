using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private ArmyModelSO army1Model;
    [SerializeField] private ArmyModelSO army2Model;
    [SerializeField]private ArmyView army1View;
    [SerializeField]private ArmyView army2View;

    private ArmyPresenter army1Presenter;
    private ArmyPresenter army2Presenter;

    void Start()
    {
        startButton.onClick.AddListener(OnStart);

        army1Presenter = new ArmyPresenter(army1Model, army1View);
        army1View.BindPresenter(army1Presenter);

        army2Presenter = new ArmyPresenter(army2Model, army2View);
        army2View.BindPresenter(army2Presenter);
    }

    void OnStart()
    {
        SceneManager.LoadScene(1);
    }
}