using System;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BounceBall : MonoBehaviour
{
    [SerializeField] private int crashCount = 0;
    [SerializeField] private bool isShooting = false;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private BounceBallDestroy ballDestroy;
    private BounceBallReflect reflect;
    private BounceBallMovement movement;

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
        reflect = GetComponent<BounceBallReflect>();
        movement = GetComponent<BounceBallMovement>();
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
        if (Owner != null && isShooting == false)
            FollowOwner();
    }

    private void OnDestroy()
    {
        Owner.OnShootEvent -= Shoot;
        Owner.Stat.OnItemEffectEvent -= TakeItemEffect;
    }

    private void FollowOwner()
    {
        Vector2 ownerPos = new Vector2(Owner.transform.position.x, Owner.transform.position.y + 0.3f);
        transform.position = ownerPos;
    }

    public void SetInfo(Paddle owner)
    {
        isShooting = false;
        this.Owner = owner;
        if (owner.playerType == PlayerType.Player1)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;

        owner.OnShootEvent += Shoot;
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
        float bounceAngle = paddleRandomAngles[UnityEngine.Random.Range(0, paddleRandomAngles.Length)]; //랜덤 값이 반영된다.
        Vector3 direction = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
        direction = direction.normalized * Stat.CurrentBallStat.ballSpeed;
        movement.Move(direction);
        Owner.OnShootEvent -= Shoot;
        AudioManager.Instance.PlaySfx(AudioClipType.shoot);
        isShooting = true;
    }

    public void BrickBounce(Collision2D ballCollision, Collider2D brickCollider)
    {
        if (reflect.reflectType == BallReflectType.OnCollisionReflect)
            reflect.BrickReflectBounce(ballCollision, brickCollider);
        else if (reflect.reflectType == BallReflectType.OnCollisionPhisics)
            reflect.BrickPhisicsBounce(ballCollision, brickCollider);
        else if (reflect.reflectType == BallReflectType.OnCollisionPhisicsV2)
            reflect.BrickPhisicsBounceV2(ballCollision, brickCollider);
    }

    public void WallBounce(Collision2D ballCollision, Collider2D wallCollider)
    {
        if (reflect.reflectType == BallReflectType.OnCollisionReflect)
            reflect.WallReflectBounce(ballCollision, wallCollider);
        else if (reflect.reflectType == BallReflectType.OnCollisionPhisics)
            reflect.WallPhisicsBounce(ballCollision, wallCollider);
        else if (reflect.reflectType == BallReflectType.OnCollisionPhisicsV2)
            reflect.WallPhisicsBounceV2(ballCollision, wallCollider);
    }
    public void PaddleBounce(Collision2D ballCollision, Paddle paddle)
    {
        if (reflect.reflectType == BallReflectType.OnCollisionReflect)
            reflect.PaddleReflectBounce(ballCollision, paddle);
        else if (reflect.reflectType == BallReflectType.OnCollisionPhisics)
            reflect.PaddlePhisicsBounce(ballCollision, paddle);
        else if (reflect.reflectType == BallReflectType.OnCollisionPhisicsV2)
            reflect.PaddlePhisicsBounceV2(ballCollision, paddle);
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

    public void Reflected(GameObject go)
    {
        UpdateCrashCount();
        if (go.layer == (int)Defines.ELayerMask.Wall)
        {
            Wall wall = go.GetComponent<Wall>();
            AudioManager.Instance.PlaySfx(AudioClipType.wall_bounce);
            if (wall.wallType == WallType.Bottom)
            {
                DestroyBall();
            }
        }
        else if (go.layer == (int)Defines.ELayerMask.Paddle1 || go.layer == (int)Defines.ELayerMask.Paddle2)
        {
            AudioManager.Instance.PlaySfx(AudioClipType.paddle_bounce);
        }
        else if (go.layer == (int)Defines.ELayerMask.Brick)
        {
            Brick brick = go.GetComponent<Brick>();
            brick.OnDamaged(this, Stat.CurrentBallStat.ballPower);
            AudioManager.Instance.PlaySfx(AudioClipType.brick_bounce);
        }
    }
}