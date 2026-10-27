using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{  
    public GameObject[] itemPrefabs;
    private List<GameObject>[] itemPools;

    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    
    public float interval = 3f;
    private float timer = 0f;

    private void Awake()
    {
        itemPools = new List<GameObject>[itemPrefabs.Length];
        for (int i = 0; i < itemPools.Length; ++i)
        {
            itemPools[i] = new List<GameObject>();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            timer = 0f;
            int index = Random.Range(0, itemPrefabs.Length);
            if(itemPools[index].Count == 0)
            {
                GameObject newItemGo = Instantiate(itemPrefabs[index]);
                itemPools[index].Add(newItemGo);

                newItemGo.SetActive(false);
                IItem newItem = newItemGo.GetComponent<IItem>();
                if (newItem != null)
                {
                    newItem.OnUse.AddListener(() => itemPools[index].Add(newItemGo));
                    newItem.OnUse.AddListener(() => itemPools[index].Remove(newItemGo));
                }
            }
            

            Vector3 position = Vector3.zero;
            position.x = Random.Range(minX, maxX);
            position.z = Random.Range(minZ, maxZ);

            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                position = hit.position;
                position.y = 0.5f;

                GameObject newGo = itemPools[index][0];
                newGo.transform.position = position;
                newGo.SetActive(true);

                itemPools[index].RemoveAt(0);
                // Instantiate(prefab, position, Quaternion.identity);
            }
            else
            {
                Debug.Log("실패");
            }            
        }
    }

}
