using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mant�n este objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados si ya existe
        }
    }
}

