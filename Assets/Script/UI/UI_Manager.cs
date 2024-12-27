using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DrawColor
{
    public class UI_Manager : MonoBehaviour
    {
        public static UI_Manager instance;

        private bool gamePaused;
        private bool gameMuted;

        [SerializeField] private GameObject mainMenu;
        public GameObject finishMenu;

        [Header("Volume info")]
        [SerializeField] private UI_VolumeSlider[] slider;
        [SerializeField] private Image muteIcon;
        [SerializeField] private Image inGameMuteIcon;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            /*
            for (int i = 0; i < slider.Length; i++)
            {
                slider[i].SetupSlider();
            }
            */
            SwitchMenuTo(mainMenu);
        }

        public void SwitchSceneTo(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                // Chuyển sang Scene với tên đã nhập
                if (Application.CanStreamedLevelBeLoaded(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogError($"Scene '{sceneName}' không tồn tại hoặc không được thêm vào Build Settings.");
                }
            }
            else
            {
                Debug.LogWarning("Tên Scene không được nhập.");
            }
        }

        public void SwitchMenuTo(GameObject uiMenu)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            uiMenu.SetActive(true);

            //AudioManager.instance.PlaySFX(4);
        }

        public void MuteButton()
        {
            gameMuted = !gameMuted;

            if (gameMuted)
            {
                muteIcon.color = new Color(1, 1, 1, .5f);
                AudioListener.volume = 0;
            }
            else
            {
                muteIcon.color = Color.white;
                AudioListener.volume = 1;
            }
        }

        public void StartGameButton()
        {
            muteIcon = inGameMuteIcon;

            if (gameMuted)
                muteIcon.color = new Color(1, 1, 1, .5f);
        }

        public void PauseGameButton()
        {
            if (gamePaused)
            {
                Time.timeScale = 1;
                gamePaused = false;
            }
            else
            {
                Time.timeScale = 0;
                gamePaused = true;
            }
        }

        public void MainMenu()
        {
            // Chuyển đến scene tiếp theo
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
            SceneManager.LoadScene(nextSceneIndex);
        }

        // Quit Game function to exit the application
        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }

}
