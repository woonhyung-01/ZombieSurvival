using System.Collections;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State //열거형. 안의 값 데이터들을 한번에 선언 가능
    {
        Ready,
        Empty,
        Reloading,
    }

    public State CurrentState {get; private set;}

    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;
    public GunData gunData;
    
    private float fireDistance = 50f;
    private int ammoRemain;
    private int magAmmo;
    private float lastFireTime;

    public UIManager uiManager;
    

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }
    public void AddAmmo(int add)
    {
        ammoRemain += add;
        uiManager.SetAmmoText(magAmmo, ammoRemain);
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        uiManager.SetAmmoText(magAmmo, ammoRemain);

        CurrentState = State.Ready;
        lastFireTime = 0f;
    }

    public void Fire()
    {
        if(CurrentState == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            shoot();
        }
    }

    public LayerMask targetLayer;

    private void shoot()
    {
        Vector3 hitPosition;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
         // fireDistance까지만 검사한다. targetLayer를 넘기면 일부 충돌 레이어들만 검사. 없으면 전부 검사
        if(Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
        {
            hitPosition = hit.point;
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        StartCoroutine(CoShotEffect(hitPosition));

        magAmmo--;
        uiManager.SetAmmoText(magAmmo, ammoRemain);

        if (magAmmo == 0)
        {
            CurrentState = State.Empty;
        }
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        //yield return(일드 리턴) new WaitForSeconds(n초 후) = 일시정지 후 호출한 쪽으로 넘어간 후 다시 돌아옴
        yield return new WaitForSeconds(0.03f); 
        
        bulletLineRenderer.enabled = false;
    }


    public bool Reload()
    {
        if (CurrentState == State.Reloading || magAmmo >= gunData.magCapacity || ammoRemain <= 0)
        {
            return false;
        }
        
        StartCoroutine(CoReloadRoutine());
        return true;
    }
    private IEnumerator CoReloadRoutine()
    {
        CurrentState = State.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
        yield return new WaitForSeconds(gunData.reloadTime - 0.5f);

        int ammoToFill = Mathf.Min(gunData.magCapacity - magAmmo, ammoRemain);

        if(ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        uiManager.SetAmmoText(magAmmo, ammoRemain);

        CurrentState = State.Ready;
    }
}
