using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen = null;
    public void TakeDamage()
    {
        Debug.Log("Player dead");
        if(_gameOverScreen != null)
        {
            _gameOverScreen.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
