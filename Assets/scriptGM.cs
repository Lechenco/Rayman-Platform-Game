using UnityEngine;
using UnityEngine.SceneManagement;

public class scriptGM : MonoBehaviour
{
    private bool isEnded = false;
    public void EndGame() {
        if (!isEnded) {
            isEnded = true;
            Invoke("Restart", 1f);
        }
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
