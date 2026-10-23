using UnityEngine;

//class안 생성자임.  CreateAssetMenu = Asset에 GunData를 생성함\ +를 누르면 ScriptableObject가 맨 아래에 생김.
                    // GunData 누르면 각 총마다 데이터를 달아줄 수 있음.
[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject //이 오브젝트가 하나 만들어 질 때마다 각 객체의 데이터가 됨
{
    public AudioClip shotClip;
    public AudioClip reloadClip;


    public float damage = 25f;
    public int startAmmoRemain = 80;
    public int magCapacity = 25;
    public float timeBetFire = 0.12f;
    public float reloadTime = 1.8f;
}
