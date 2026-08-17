using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private GameObject _endCanvas = null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(_endCanvas != null)
            _endCanvas.SetActive(true);
        }
    }
}
