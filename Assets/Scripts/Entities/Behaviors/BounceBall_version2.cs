using System;
using UnityEngine;
using Random = UnityEngine.Random; // 모호한 참조 오류

public class BounceBallv2 : MonoBehaviour
{
    [SerializeField] private int crashCount = 0;
    [SerializeField] private bool isShooting = false;
    public Vector2 direction;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private BounceBallDestroy ballDestroy;

    //발사시, 무작위로 발사될 각도 값의 배열
    private float[] paddleRandomAngles = { -30, -45, -60, 60, 45, 30 };

    public Paddle Owner { get; private set; }
    public BounceBallStat Stat { get; private set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ballDestroy = GetComponent<BounceBallDestroy>();
        Stat = GetComponent<BounceBallStat>();
    }

    private void Start()
    {
        if (isShooting)
        {
            Shoot();
        }
    }

    private void Update()
    {
        FollowOwner();
    }

    private void FixedUpdate()
    {
        if (isShooting && rb2d.velocity != Vector2.zero)
        {
            direction = rb2d.velocity;
        }
    }

    private void FollowOwner()
    {
        if (Owner != null && isShooting == false)
        {
            Vector2 ownerPos = new Vector2(Owner.transform.position.x, Owner.transform.position.y + 0.3f);
            transform.position = ownerPos;
        }
    }

    public void SetInfo(Paddle owner)
    {
        isShooting = false;
        this.Owner = owner;
        if (owner.playerType == PlayerType.Player1)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;

        owner.OnShootEvent -= Shoot;
        owner.OnShootEvent += Shoot;

        Owner.Stat.OnItemEffectEvent -= TakeItemEffect;
        Owner.Stat.OnItemEffectEvent += TakeItemEffect;

        //owner.AddMyBall(this);
    }

    public void SetInfo(Paddle owner, Vector3 position)
    {
        this.Owner = owner;
        transform.position = position;
        if (owner.playerType == PlayerType.Player1)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;

        Owner.Stat.OnItemEffectEvent -= TakeItemEffect;
        Owner.Stat.OnItemEffectEvent += TakeItemEffect;

        //owner.AddMyBall(this);
        isShooting = true;
    }

    public void Shoot()
    {
        isShooting = true;
        float bounceAngle = paddleRandomAngles[UnityEngine.Random.Range(0, paddleRandomAngles.Length)]; //랜덤 값이 반영된다.
        Vector2 direction = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
        rb2d.velocity = direction.normalized * Stat.CurrentBallStat.ballSpeed;
        Owner.OnShootEvent -= Shoot;
        AudioManager.Instance.PlaySfx(AudioClipType.shoot);
    }

    public void BrickBounce(Collision2D ballCollision)
    {
        WallBounce(ballCollision);
    }

    public void WallBounce(Collision2D ballCollision, WallType wall = WallType.None)
    {
        // 여러 충돌 지점 중 가장 큰 충격을 받은 지점 선택
        ContactPoint2D primaryContact = ballCollision.contacts[0];
        float maxImpact = 0f;

        foreach (var contact in ballCollision.contacts)
        {
            float impact = contact.normalImpulse;
            if (impact > maxImpact)
            {
                primaryContact = contact;
                maxImpact = impact;
            }
        }

        // 충돌한 표면의 법선 벡터
        //Vector2 normal = ballCollision.contacts[0].normal * -1;
        // 선택된 충돌 지점의 법선 벡터 사용
        Vector2 normal = primaryContact.normal * -1;

        // 공의 현재 이동 벡터 (입사 벡터)
        Vector2 incomingVector = direction;
        // 반사 벡터 계산
        Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);

        reflectVector = reflectVector.normalized * Stat.CurrentBallStat.ballSpeed;

        // 반사 벡터로 공의 속도 변경
        rb2d.velocity = reflectVector;
        UpdateCrashCount();
    }

    public void PaddleBounce(Collision2D ballCollision, Paddle paddle)
    {
        // movementInfluence: 패들의 움직임이 반사각에 미치는 영향의 강도
        float movementInfluence = 0.2f;

        // 패들의 충돌 범위와 위치 계산 (BoxCollider2D의 bounds를 사용하여 실제 크기 계산)
        BoxCollider2D paddleCollider = paddle.GetComponent<BoxCollider2D>();
        float paddleWidth = paddleCollider.bounds.size.x; // 패들의 실제 너비 사용
        Vector3 paddlePosition = paddle.transform.position;
        Vector3 contactPoint = ballCollision.GetContact(0).point;

        // 공의 반지름을 고려한 충돌 지점의 상대적 위치 계산
        float ballRadius = ballCollision.collider.bounds.extents.x; // 공의 반경 계산
        float relativeHitPosition = (contactPoint.x - paddlePosition.x) / (paddleWidth + ballRadius);
        relativeHitPosition = Mathf.Clamp(relativeHitPosition, -1f, 1f); // 범위를 -1에서 1 사이로 제한

        // 충돌 위치에 따른 반사각 계산
        float maxBounceAngle = paddle.bounceAngleRange; // 설정된 최대 반사각
        float minBounceAngle = 10f; // 최소 반사각 설정
        float bounceAngle = relativeHitPosition * maxBounceAngle;

        // 너무 작은 각도 보정 -> 공이 패들에서 스쳐지나가며 멈추지 않도록 최소 각도 설정
        if (Mathf.Abs(bounceAngle) < minBounceAngle)
        {
            bounceAngle = Mathf.Sign(bounceAngle) * minBounceAngle;
        }

        // 반사각에 약간의 무작위성 추가

        float randomFactor = Random.Range(-2f, 2f); // 2도 내외로 무작위성 추가
        bounceAngle += randomFactor;

        bounceAngle = Mathf.Clamp(bounceAngle, -maxBounceAngle, maxBounceAngle); // 최대/최소 반사각 제한

        // 반사 벡터 계산 (공의 속도를 유지하면서 각도만 변경)
        Vector2 direction = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
        direction = direction.normalized;

        Rigidbody2D paddleRb = paddle.GetComponent<Rigidbody2D>();


        // 패들의 움직임에 따른 반사각 조정 -> movementInfluence 값이 클수록 패들의 속도가 공의 반사각에 영향을 크게 미침
        float maxPaddleSpeedEffect = 0.3f; // 패들 속도가 반사각에 미치는 최대 한계값 설정
        float paddleSpeedEffect = Mathf.Clamp(paddleRb.velocity.x * movementInfluence, -maxPaddleSpeedEffect, maxPaddleSpeedEffect);
        direction.x += paddleSpeedEffect;
        direction = direction.normalized;



        // 공의 속도를 일정 범위 내에서 유지 (최소 및 최대 속도 적용)
        float currentSpeed = rb2d.velocity.magnitude;
        float minSpeed = 5f; // 최소 속도
        float maxSpeed = 20f; // 최대 속도
        float finalSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        rb2d.velocity = direction * finalSpeed; // 최종 계산된 속도를 적용

        // 충돌 횟수 업데이트 (중복 기록 방지 로직 추가)
        UpdateCrashCount();
    }


    public void UpdateCrashCount()
    {
        crashCount += 1;
        Stat.IncreaseSpeed();
    }

    public void DestroyBall()
    {
        Owner.Stat.OnItemEffectEvent -= TakeItemEffect;
        ballDestroy?.DestroyBall();
        //Owner.RemoveMyBall(this);
    }

    private void TakeItemEffect(Item item)
    {
        // 아이템 효과 적용하기
        Stat.ApplyItemEffect(item);
    }

    public void GenerateBall(int count)
    {
        // 공 생성
        GameManager.Instance.BallGenerate(Owner, transform.position);
    }
}