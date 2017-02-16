using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class TestRestart : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
