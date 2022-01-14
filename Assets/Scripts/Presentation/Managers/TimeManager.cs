using UnityEngine;

namespace Presentation.Managers
{
    public class TimeManager : MonoBehaviour
    {
        public void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
        }
    }
}