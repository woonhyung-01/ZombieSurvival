using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uIManager;
    public Zombie zombiePrefab;
    public ZombieData[] zombieDatas;
    public Transform[] spawnPoints; //정적 배열

    private readonly List<Zombie> zombies = new List<Zombie>(); //<이거>의 List 컬렉션
    private readonly List<Zombie> zombiePool = new List<Zombie>(); //동적 배열

    private int wave = 0;


    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameController");
        gameManager = findGo.GetComponent<GameManager>();
        uIManager = gameManager.uIManager;
    }

    private void Update()
    {
        if (zombies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        wave++;

        int count = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < count; ++i)
        {
            CreateZombie();
        }
        uIManager.SetWaveText(wave, zombies.Count);
    }
    private void CreateZombie()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        //instantiate가 붙어있는 오브젝트 생성
        // Zombie zombie = Instantiate(zombiePrefab, point.position, point.rotation);

        Zombie zombie;
        if(zombiePool.Count == 0)
        {
            zombie = Instantiate(zombiePrefab);
            zombie.gameObject.SetActive(false);
            zombiePool.Add(zombie);

            zombie.OnDead.AddListener(() => zombies.Remove(zombie)); //AddListener < event함수. 에드리스너에 반환형이 없는 메소드 참조
            zombie.OnDead.AddListener(() => uIManager.SetWaveText(wave, zombies.Count));
            zombie.OnDead.AddListener(() => StartCoroutine(CoReturnToPoolZombie(zombie)));
            zombie.OnDead.AddListener(() => gameManager.AddScore(50));
        }

        zombie = zombiePool[0];
        zombie.Setup(data);
        zombie.transform.position = point.position;
        zombie.transform.rotation = point.rotation;
        zombie.gameObject.SetActive(true);

        zombies.Add(zombie);
        zombiePool.Remove(zombie);

        
        
    }
    private IEnumerator CoReturnToPoolZombie(Zombie zombie)
    {
        yield return new WaitForSeconds(5f);
        zombie.gameObject.SetActive(false);
        zombiePool.Add(zombie);
    }

    public void OnEndGame()
    {
        for (int i = zombies.Count - 1; i >= 0; --i)
        {
            zombies[i].Die();
        }
    }
}
