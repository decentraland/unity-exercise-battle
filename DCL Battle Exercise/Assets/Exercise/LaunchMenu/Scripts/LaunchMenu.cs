using Exercise.Models.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Exercise.LaunchMenu.Scripts
{
    public sealed class LaunchMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private ArmyModelSo[] armyModels;
        [SerializeField] private ArmyView armyViewPrefab;
        [SerializeField] private GameObject armyViewParent;

        private ArmyView armyViews;

        private void Start()
        {
            startButton.onClick.AddListener(OnStart);

            foreach (var armyModel in armyModels)
            {
                var armyView = Instantiate(armyViewPrefab, armyViewParent.transform, true);
                armyView.gameObject.SetActive(true);
                var armyPresenter = new ArmyPresenter(armyModel, armyView);
                armyView.BindPresenter(armyPresenter);
            }
        }

        private static void OnStart() => SceneManager.LoadScene(1);
    }
}