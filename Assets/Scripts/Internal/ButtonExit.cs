using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });
    }
}
