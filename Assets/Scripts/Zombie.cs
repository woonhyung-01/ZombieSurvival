using System;
using System.Data.SqlTypes;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 10f;
    public float attackDistance = 1f;

    public ParticleSystem bloodEffect;
    public Collider zombieCollider;
    public Renderer zombieRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;

    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;

    public float damage = 10f;
    public float attackInerval = 1f;
    private float lastAttackTime;
    private Status currentStatus;
    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDie = Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;

            Debug.Log(currentStatus);

            switch (currentStatus)
            {
                case Status.Idle:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    zombieAnimator.SetBool(IdHasTarget, true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    agent.isStopped = true;
                    zombieAnimator.SetTrigger(IdDie);
                    zombieCollider.enabled = false;
                    zombieAudioSource.PlayOneShot(deathClip);
                    
                    break;
            }
        }
    }

    public void Setup(ZombieData data)
    {
        startingHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;
        zombieRenderer.material.color = data.skinColor;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;
        
        

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas)) // 네비메쉬 위에 점이 있으면 rue이고 아니면 false
        {
            agent.Warp(hit.position);
        }

        zombieCollider.enabled = true;

        CurrentStatus = Status.Idle;
    }

    private void ODisable()
    {
        if(agent.isOnNavMesh)
        {
            agent.ResetPath(); //네비게이션 에이전트가 이동할 때 경로 계산 초기화.
        }
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
               break;
            case Status.Die:
                // UpdateDie();
                break;
            }
    }
    

    private void UpdateDie()
    {

    }


    private void UpdateAttack()
    {
        
         if(target == null || Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }
       
        if(Time.time >= lastAttackTime + attackInerval)
        {
            lastAttackTime = Time.time;

            IDamageable damageable = target.GetComponent<IDamageable>();
            var hitNormal = (transform.position - target.position).normalized;
            damageable.OnDamage(damage, target.position, hitNormal);
        }
        
    }

    private void UpdateTrace()
    {
         if(target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        if(target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }
     
        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer); //targetLayer를 전부 배열로 만들어서 리턴

        Transform closet = null;
        float closetDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; ++i)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
            }
        }
        

        return closet;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        zombieAudioSource.PlayOneShot(hitClip);
        bloodEffect.transform.position = hitPoint;
        bloodEffect.transform.forward = hitNormal;
        bloodEffect.Play();
    }

    public override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;
    }
}
