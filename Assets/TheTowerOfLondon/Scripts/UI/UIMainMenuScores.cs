using Loaders;
using Saves;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIMenu
{
    public class UIMainMenuScores : MonoBehaviour
    {
        [SerializeField] private Button _btnBack;

        [SerializeField] private GameObject _panelGeneral;

        [SerializeField] private Transform _columnDifficulty;

        [SerializeField] private Transform _columnScore;

        [SerializeField] private Transform _columnBestDifficulty;

        [SerializeField] private Transform _columnBestScore;

        [SerializeField] private GameObject _prefabRow;

        private string[] _textsByDif = new string[3] { "Easy", "Normal", "Hard" };

        private void Awake()
        {
            SaveResultsStruct[] saveResultsStructs =  (SaveResultsStruct[]) GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.Results);

            int[] bestScores = new int[3];

            bestScores[0] = (int) GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.RecordEasy);

            bestScores[1] = (int)GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.RecordNormal);

            bestScores[2] = (int)GameService.Singleton.GetService<ISaveLoader>().Load(LoadType.RecordHard);

            for(int i = saveResultsStructs.Length - 1; i >= 0; i--)
            {
                Instantiate(_prefabRow, _columnDifficulty).GetComponent<TextMeshProUGUI>().SetText(saveResultsStructs[i].Difficulty.ToString());

                Instantiate(_prefabRow, _columnScore).GetComponent<TextMeshProUGUI>().SetText(saveResultsStructs[i].result.ToString());
            }

            for (int i = bestScores.Length-1; i >= 0; i--)
            {
                Instantiate(_prefabRow, _columnBestDifficulty).GetComponent<TextMeshProUGUI>().SetText(_textsByDif[i]);

                Instantiate(_prefabRow, _columnBestScore).GetComponent<TextMeshProUGUI>().SetText(bestScores[i].ToString());
            }
        }

        private void OnEnable()
        {
            _btnBack.onClick.AddListener(BackToGeneral);
        }

        private void OnDisable()
        {
            _btnBack.onClick.RemoveListener(BackToGeneral);
        }

        private void BackToGeneral()
        {
            _panelGeneral.SetActive(true);
            gameObject.SetActive(false);
        }

        
    }
}
