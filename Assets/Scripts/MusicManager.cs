using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantén este objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si ya existe
        }
    }
}

