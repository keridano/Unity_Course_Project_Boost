using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicBehavior : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int maxIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (currentIndex == 0)
        {
            if (objs.Length > 1)
                Destroy(objs[1]);

            DontDestroyOnLoad(objs[0]);
        }
        else if(currentIndex == maxIndex)
        {
            if (objs.Length > 1)
                Destroy(objs[0]);
        }
    }
}
