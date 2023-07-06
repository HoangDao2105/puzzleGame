using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelManager : MonoBehaviour
{
     public int MaxLevel;
    public static LevelManager Instance { get; private set; }
     public int CurLevelIndex
    {
        get { return PlayerPrefs.GetInt("CUR_LEVEL_INDEX", 1); }
        private set
        {
            if (value > MaxLevel)
            {
                PlayerPrefs.SetInt("CUR_LEVEL_INDEX", 1);
            }
            else 
            {
                PlayerPrefs.SetInt("CUR_LEVEL_INDEX", value);
            }
        }
    }
    
    private GameObject NextLevel;
    public Level CurLevel;

    private GameObject CurLevelPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);    
        }

        if (!PlayerPrefs.HasKey("CUR_LEVEL_INDEX"))
        {
            PlayerPrefs.SetInt("CUR_LEVEL_INDEX", 1);
        }
    }

    private void Start()
    {
        CurLevelIndex =  PlayerPrefs.GetInt("CUR_LEVEL_INDEX");
        LoadCurLevel();
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(CR_LoadLevelAsync(level));
    }

    IEnumerator CR_LoadLevelAsync(int level)
    {
        if (NextLevel == null && CurLevel == null)
        {
            AsyncOperationHandle<GameObject> loadWithIResourceLocations =
                Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/LevelPrefabs/Level" + (level) + ".prefab");
            yield return loadWithIResourceLocations;
            Debug.Log("Loaded "+loadWithIResourceLocations.Result);
            CurLevel = Instantiate(loadWithIResourceLocations.Result,transform).GetComponent<Level>();
            CurLevelPrefab = loadWithIResourceLocations.Result;
        }
        else
        {
            //Debug.Log("Load by pre " + CurLevel);
            if (CurLevel)
            {
                Addressables.ReleaseInstance(CurLevel.gameObject);
                Destroy(CurLevel.gameObject);
            }

            yield return new WaitWhile(() => NextLevel == null);
            CurLevel = Instantiate(NextLevel,transform).GetComponent<Level>();
            CurLevelPrefab = NextLevel;
        }
        
        PreLoadNextLevel();
    }

    public void LoadCurLevel()
    {
        LoadLevel(CurLevelIndex);
    }
    

    //Game lose and reset
    public void RestartLevel()
    {

        if (CurLevel)
        {
            Destroy(CurLevel.gameObject);
        }
        CurLevel = Instantiate(CurLevelPrefab,transform).GetComponent<Level>();
        Debug.Log("Reloaded "+CurLevel.name);
    }
    

    public void PreLoadNextLevel()
    {
        NextLevel = null;
        if (CurLevelIndex == MaxLevel)
        {
            AsyncOperationHandle<GameObject> loadWithIResourceLocations =
                Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/LevelPrefabs/Level1.prefab");
            loadWithIResourceLocations.Completed += LoadWithIResourceLocations_Completed;
        }
        else
        {
            AsyncOperationHandle<GameObject> loadWithIResourceLocations =
                Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/LevelPrefabs/Level" + (CurLevelIndex + 1) + ".prefab");
            loadWithIResourceLocations.Completed += LoadWithIResourceLocations_Completed;
        }
    }

    public AsyncOperationHandle<GameObject> LoadLastLevel()
    {
        NextLevel = null;
        if (CurLevelIndex > MaxLevel)
        {
            CurLevelIndex = 1;
        }
        //    Debug.Log("Load Last level " + CurLevelIndex);

        AsyncOperationHandle<GameObject> loadWithIResourceLocations =
            Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/LevelPrefabs/Level" + (CurLevelIndex)  + ".prefab");
        loadWithIResourceLocations.Completed += LoadWithIResourceLocations_Completed;
        return loadWithIResourceLocations;
    }

    public void LevelUp()
    {
        CurLevelIndex++;
       Debug.Log(PlayerPrefs.GetInt("CUR_LEVEL_INDEX"));
    }
    private void LoadWithIResourceLocations_Completed(AsyncOperationHandle<GameObject> obj)
    {
        //    Debug.Log("Loadded " + obj.Result.name);
        NextLevel = obj.Result;
    }

    public void RestartGame()
    {
        Debug.Log("Restart game");
        CurLevelIndex = 1;
        LoadCurLevel();
    }
}
