using System;
using UnityEngine;

public class BounceBall : MonoBehaviour
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
        if(isShooting)
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

        owner.AddMyBall(this);
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

        owner.AddMyBall(this);
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
        Debug.Log("입장~");
        // 충돌한 표면의 법선 벡터
        Vector2 normal = ballCollision.contacts[0].normal * -1;
        Debug.Log(normal);
        // 공의 현재 이동 벡터 (입사 벡터)
        Vector2 incomingVector = direction;
        Debug.Log(incomingVector);
        // 반사 벡터 계산
        Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);
        Debug.Log(reflectVector);

        reflectVector = reflectVector.normalized * Stat.CurrentBallStat.ballSpeed;

        // 반사 벡터로 공의 속도 변경
        rb2d.velocity = reflectVector;
        UpdateCrashCount();
    }

    public void PaddleBounce(Collision2D ballCollision, Paddle paddle)
    {
        // 패들의 중앙을 기준으로 공이 충돌한 위치 계산
        BoxCollider2D paddleCollider = paddle.GetComponent<BoxCollider2D>();
        Vector3 paddlePosition = paddle.transform.position;
        Vector3 contactPoint = ballCollision.GetContact(0).point;
        float paddleWidth = paddleCollider.bounds.size.x;

        // 충돌 지점의 상대적 위치 (-1: 왼쪽 끝, 0: 중앙, 1: 오른쪽 끝)
        float relativeHitPosition = (contactPoint.x - paddlePosition.x) / paddleWidth;

        // 충돌 위치에 따른 각도 계산
        float bounceAngle = relativeHitPosition * paddle.bounceAngleRange;

        // 반사 벡터 계산 (공의 속도를 유지하면서 각도만 변경)
        Vector2 direction = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
        direction = direction.normalized;

        // 공의 속도를 기존 속도 크기에 맞춰 반사
        rb2d.velocity = direction * Stat.CurrentBallStat.ballSpeed;
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
        Owner.RemoveMyBall(this);
    }

    public void UseItemBallPowerUp()
    {
        //TODO: PlayerType받아오기
        GameManager.Instance.BallPowerUp(PlayerType.Player1, 1);

        //TODO: 플레이어1, 플레이어2 볼 따로 적용 처리
        //공 크기 1.3배 키우기
        Vector3 currentScale = transform.localScale;
        currentScale.x *= 1.3f;
        currentScale.y *= 1.3f;
        transform.localScale = currentScale;
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