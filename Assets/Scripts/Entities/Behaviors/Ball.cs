using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // 발사되는 로직 (Vector3 * Speed?)
    // 벽돌 충돌 감지
    // 패들 충돌 감지 
    // 속성: 파워, 속도, 

    [SerializeField] private LayerMask levelCollisionLayer;

    private bool isReady; //처음 공격 가능하게 하는 함수

    private Rigidbody2D rigidbody; 
    private SpriteRenderer spriteRenderer;

    private Vector2 direction;

    private float speed; //private float speed = GameManager.Instance.ballSpeed;
    private int Damage; //private int Damage = GameManager.Instance.ballDamage;
    //public Paddle paddle;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = direction * speed; //공의 속력
    }

    public void InitializeAttack(Vector2 direction) //초기 어택 설정
    {
        this.direction = direction;
        transform.up = this.direction;
        isReady = true;
    }

    //충돌시 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if(IsLayerMatched(levelCollisionLayer.value, collision.gameObject.layer)) //레이어마스크 비교하여 레벨일 경우
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>;
            if (healthSystem != null)
            {
                bool isAttackApplied = healthSystem.ChangeHealth(-attackData.power);
            }
        }
        */ 
    }
    private bool IsLayerMatched(int value, int layer)
    {
        return value == (value | 1 << layer);
    }



}