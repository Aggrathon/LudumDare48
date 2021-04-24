using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    public int width = 21;
    public int height = 21;

    public List<PrefabProbability> prefabsSolid;
    public List<PrefabProbability> prefabsWallLeft;
    public List<PrefabProbability> prefabsWallRight;
    public List<PrefabProbability> prefabsWallLeftRight;
    public List<PrefabProbability> prefabsWallTop;
    public List<PrefabProbability> prefabsWallLeftTop;
    public List<PrefabProbability> prefabsWallRightTop;
    public List<PrefabProbability> prefabsWallLeftRightTop;
    public List<PrefabProbability> prefabsWallBottom;
    public List<PrefabProbability> prefabsWallLeftBottom;
    public List<PrefabProbability> prefabsWallRightBottom;
    public List<PrefabProbability> prefabsWallLeftRightBottom;
    public List<PrefabProbability> prefabsWallTopBottom;
    public List<PrefabProbability> prefabsWallLeftTopBottom;
    public List<PrefabProbability> prefabsWallRightTopBottom;
    public List<PrefabProbability> prefabsWallLeftRightTopBottom;
    public List<PrefabProbability> prefabsSpace;
    public List<PrefabProbability> prefabsSpaceLeft;
    public List<PrefabProbability> prefabsSpaceRight;
    public List<PrefabProbability> prefabsSpaceLeftRight;
    public List<PrefabProbability> prefabsSpaceTop;
    public List<PrefabProbability> prefabsSpaceLeftTop;
    public List<PrefabProbability> prefabsSpaceRightTop;
    public List<PrefabProbability> prefabsSpaceLeftRightTop;
    public List<PrefabProbability> prefabsSpaceBottom;
    public List<PrefabProbability> prefabsSpaceLeftBottom;
    public List<PrefabProbability> prefabsSpaceRightBottom;
    public List<PrefabProbability> prefabsSpaceLeftRightBottom;
    public List<PrefabProbability> prefabsSpaceTopBottom;
    public List<PrefabProbability> prefabsSpaceLeftTopBottom;
    public List<PrefabProbability> prefabsSpaceRightTopBottom;
    public List<PrefabProbability> prefabsSpaceLeftRightTopBottom;

    private List<List<PrefabProbability>> mazePrefabs;
    protected List<List<PrefabProbability>> MazePrefabs
    {
        get
        {
            if (mazePrefabs == null || mazePrefabs.Count != 32)
            {
                mazePrefabs = new List<List<PrefabProbability>>
                {
                    prefabsSolid,
                    prefabsWallLeft,
                    prefabsWallRight,
                    prefabsWallLeftRight,
                    prefabsWallTop,
                    prefabsWallLeftTop,
                    prefabsWallRightTop,
                    prefabsWallLeftRightTop,
                    prefabsWallBottom,
                    prefabsWallLeftBottom,
                    prefabsWallRightBottom,
                    prefabsWallLeftRightBottom,
                    prefabsWallTopBottom,
                    prefabsWallLeftTopBottom,
                    prefabsWallRightTopBottom,
                    prefabsWallLeftRightTopBottom,
                    prefabsSpace,
                    prefabsSpaceLeft,
                    prefabsSpaceRight,
                    prefabsSpaceLeftRight,
                    prefabsSpaceTop,
                    prefabsSpaceLeftTop,
                    prefabsSpaceRightTop,
                    prefabsSpaceLeftRightTop,
                    prefabsSpaceBottom,
                    prefabsSpaceLeftBottom,
                    prefabsSpaceRightBottom,
                    prefabsSpaceLeftRightBottom,
                    prefabsSpaceTopBottom,
                    prefabsSpaceLeftTopBottom,
                    prefabsSpaceRightTopBottom,
                    prefabsSpaceLeftRightTopBottom
                };
            }
            return mazePrefabs;
        }
    }

    [System.Serializable]
    public struct PrefabProbability
    {
        public float prob;
        public GameObject prefab;
    }


    private void Start()
    {
        NormalisePrefabProbabilities();
    }


    [ContextMenu("Normalise Prefab Probabilities")]
    void NormalisePrefabProbabilities()
    {
        foreach (var list in MazePrefabs)
        {
            float sum = 0f;
            foreach (var pp in list)
            {
                sum += pp.prob;
            }
            if (sum > 0f)
            {
                sum = 1f / sum;
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = new PrefabProbability { prob = list[i].prob * sum, prefab = list[i].prefab };
                }
            }
        }
    }


    [ContextMenu("Generate Maze")]
    void GenerateDefaultMaze()
    {
        NormalisePrefabProbabilities();
        MazeGenerator gen = new MazeGenerator(width, height);
        int midX = width / 2;
        int midY = height / 2;
        gen.OpenSpace(midX - 1, midX + 1, midY - 1, midY + 1);
        gen.Generate();
        SpawnMaze(gen);
    }

    [ContextMenu("Clear Maze")]
    void ClearMaze()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
    }

    void SpawnMaze(MazeGenerator gen)
    {
        float offsetX = -width / 2;
        float offsetY = -height / 2;
        var mp = MazePrefabs;
        foreach ((int x, int y, MazeGenerator.CellShape cell) in gen.GetCells())
        {
            int i = (int)cell;
            var prefabs = mp[i];
            if (prefabs.Count > 0)
            {
                Vector3 pos = new Vector3(x + offsetX, 0f, y + offsetY);
                if (prefabs.Count == 1)
                {
                    if (prefabs[0].prefab)
                        Instantiate(prefabs[0].prefab, pos, Quaternion.identity, transform);
                }
                else
                {
                    float rnd = Random.value;
                    foreach (var pp in prefabs)
                    {
                        if (rnd > pp.prob)
                        {
                            rnd -= pp.prob;
                        }
                        else
                        {
                            if (pp.prefab)
                                Instantiate(pp.prefab, pos, Quaternion.identity, transform);
                            break;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.isStatic = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 0, height));
    }


}
