using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public enum BallReflectType
{
    None,
    DetectAndRefelct, // 이동할때 물체 감지하여 방향 전환
    OnCollisionReflect, // 충돌 이벤트 발생했을때 방향 전환
    OnCollisionPhisics // 충돌 이벤트 발생했을때 물리적 으로 방향 전환
}

public class BounceBallReflect : MonoBehaviour
{
    public event Action<GameObject> OnReflectedEvent;

    public BallReflectType reflectType = BallReflectType.None;
    LayerMask includeRayLayerMask;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb2d;
    private BounceBall ball;
    private BounceBallMovement movement;
    private float layDistance = 1f;
    private float randBounceAngle = 10f;

    // 공의 충돌 기준 거리
    private float minReflectDistance = 0.005f;
    // 공 주변 collder 감지 범위
    private float colliderDetectRangeRatio = 1.5f;
    // 평행 임계값 (평행 각도 방지)
    private float parallelThreshold = 0.95f;
    // 직교 임계값 (수직 각도 방지)
    private float orthogonalThreshold = 0f;
    // 충돌이 일어나고 있는지 확인
    private bool isReflecting = false;

    private void Awake()
    {
        ball = GetComponent<BounceBall>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<BounceBallMovement>();
        rb2d = GetComponent<Rigidbody2D>();
        // 레이캐스트 대상
        includeRayLayerMask = Util.CombineLayersToMask(
            Defines.ELayerMask.Wall,
            Defines.ELayerMask.Brick,
            Defines.ELayerMask.Paddle1,
            Defines.ELayerMask.Paddle2
            );
    }

    private void Start()
    {
        //isPhisics true일 경우 Rigidbody2D 사용
        rb2d.constraints =
            reflectType != BallReflectType.OnCollisionPhisics ?
            RigidbodyConstraints2D.FreezeAll :
            RigidbodyConstraints2D.None;

        // DetectAndRefelct 이면 Collider event를 발생 시킬 이유가 없음.
        if (reflectType == BallReflectType.DetectAndRefelct)
            circleCollider.excludeLayers = includeRayLayerMask;
    }

    #region 물리엔진 사용
    public void BrickPhisicsBounce(Collision2D ballCollision, Collider2D brickCollder)
    {
        WallPhisicsBounce(ballCollision, brickCollder);
    }

    public void WallPhisicsBounce(Collision2D ballCollision, Collider2D wallCollder)
    {
        // 충돌한 표면의 법선 벡터
        Vector2 normal = ballCollision.contacts[0].normal * -1;
        // 공의 현재 이동 벡터 (입사 벡터)
        Vector2 incomingVector = movement.velocity;
        // 반사 벡터 계산
        Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);

        float dotProduct = Mathf.Abs(Vector2.Dot(incomingVector, reflectVector));
        if (dotProduct >= parallelThreshold || dotProduct <= orthogonalThreshold)
        {
            float additionalBounceAngle = UnityEngine.Random.Range(-randBounceAngle, randBounceAngle);
            reflectVector = Quaternion.Euler(0, 0, additionalBounceAngle) * reflectVector;
        }

        reflectVector = reflectVector.normalized * ball.Stat.CurrentBallStat.ballSpeed;

        OnReflectedEvent?.Invoke(wallCollder.gameObject);
        // 반사 벡터로 공의 속도 변경
        movement.Move(reflectVector);
    }

    public void PaddlePhisicsBounce(Collision2D ballCollision, Paddle paddle)
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
        direction = direction.normalized * ball.Stat.CurrentBallStat.ballSpeed;
        OnReflectedEvent?.Invoke(paddle.gameObject);
        // 공의 속도를 기존 속도 크기에 맞춰 반사
        movement.Move(direction);
    }
    #endregion

    #region 이동벡터 사용
    public void BrickReflectBounce(Collision2D ballCollision, Collider2D wallCollder)
    {
        WallReflectBounce(ballCollision, wallCollder);
    }
    public void WallReflectBounce(Collision2D ballCollision, Collider2D wallCollder)
    {
        Vector3 hitNormal = ballCollision.contacts[0].normal;
        Vector3 nextDirection = Vector3.Reflect(movement.direction, hitNormal);
        float dotProduct = Mathf.Abs(Vector3.Dot(nextDirection, hitNormal));
        if (dotProduct >= parallelThreshold || dotProduct <= orthogonalThreshold)
        {
            float additionalBounceAngle = UnityEngine.Random.Range(-randBounceAngle, randBounceAngle);
            nextDirection = Quaternion.Euler(0, 0, additionalBounceAngle) * nextDirection;
        }
        nextDirection = nextDirection.normalized;
        OnReflectedEvent?.Invoke(wallCollder.gameObject);
        movement.Move(nextDirection);
    }
    public void PaddleReflectBounce(Collision2D ballCollision, Paddle paddle)
    {
        BoxCollider2D paddleCollider = paddle.GetComponent<BoxCollider2D>();
        Vector3 paddlePosition = paddle.transform.position;
        Vector3 contactPoint = ballCollision.GetContact(0).point;
        float paddleWidth = paddleCollider.bounds.size.x;

        // 충돌 지점의 상대적 위치 (-1: 왼쪽 끝, 0: 중앙, 1: 오른쪽 끝)
        float relativeHitPosition = (contactPoint.x - paddlePosition.x) / paddleWidth;
        float additionalBounceAngle = UnityEngine.Random.Range(-randBounceAngle, randBounceAngle);
        // 충돌 위치에 따른 각도 계산
        float bounceAngle = (relativeHitPosition * paddle.bounceAngleRange) + additionalBounceAngle;

        // 반사 벡터 계산 (공의 속도를 유지하면서 각도만 변경)
        Vector2 direction = new Vector2(Mathf.Sin(bounceAngle * Mathf.Deg2Rad), Mathf.Cos(bounceAngle * Mathf.Deg2Rad));
        direction = direction.normalized;
        OnReflectedEvent?.Invoke(paddle.gameObject);
        movement.Move(direction);
    }
    #endregion

    #region 직접충돌 여부 확인해서 이동
    public Vector3 ReflectDirection(Vector3 prevDirection)
    {
        Collider2D contactCollider = GetContactCollider();

        if (contactCollider != null)
        {
            Vector2 pos = transform.position;
            Vector3 hitNormal = (contactCollider.ClosestPoint(transform.position) - pos).normalized;
            Vector3 nextDirection = Vector3.Reflect(prevDirection, hitNormal);

            // 공의 방향이 벽이나 벽돌과 평행에 가까운지 확인
            float dotProduct = Mathf.Abs(Vector3.Dot(nextDirection, hitNormal));
            if (dotProduct >= parallelThreshold || dotProduct <= orthogonalThreshold)
            {
                // 평행에 가깝거나 직교에 가까울수록 추가적인 반사 각도를 적용 (예: -10도~10도)
                float additionalBounceAngle = UnityEngine.Random.Range(-randBounceAngle, randBounceAngle);
                nextDirection = Quaternion.Euler(0, 0, additionalBounceAngle) * nextDirection;
                nextDirection = nextDirection.normalized;
                Debug.Log($"ball direction : {prevDirection} -> {nextDirection} (dot:{dotProduct})");
            }
            else
            {
                nextDirection = nextDirection.normalized;
                Debug.Log($"ball direction : {prevDirection} -> {nextDirection}");
            }
            OnReflectedEvent?.Invoke(contactCollider.gameObject);
            return nextDirection;
        }

        return prevDirection;
    }

    private Collider2D GetContactCollider()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius * 1.5f, includeRayLayerMask);
        float contactDistance = float.MaxValue;
        Collider2D contactCollider = null;
        foreach (Collider2D collider in colliders)
        {
            Box box = collider.GetComponent<Box>();
            
            Vector3 hitPostion = collider.ClosestPoint(transform.position);
            if (!box.AllowBoxContact(hitPostion)) continue;

            float dist = RayTargetDistance(transform.position, hitPostion, circleCollider.radius);
            if (dist < contactDistance && dist <= minReflectDistance)
            {
                contactDistance = dist;
                contactCollider = collider;
            }
        }

        return contactCollider;
    }

    private float RayTargetDistance(Vector3 pos, Vector3 targetPos, float radius)
    {
        Vector3 dir = (targetPos - pos).normalized;
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, layDistance, includeRayLayerMask);
        if (hit)
        {
            // Ray가 맞은 지점
            Vector2 hitPoint = hit.point;
            // 맞은 표면의 법선 벡터
            Vector2 hitNormal = hit.normal;
            // 충돌 지점까지의 거리
            float distanceFromBallCenterToHit = Vector2.Distance(pos, hitPoint);
            // 구체의 닿는 면 (구체의 중심에서 반지름만큼 떨어진 위치)
            float contactDistance = distanceFromBallCenterToHit - radius;

            return contactDistance;
        }

        return float.MaxValue;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (movement == null || circleCollider == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, movement.direction * layDistance);
        Gizmos.DrawWireSphere(transform.position, circleCollider.radius * colliderDetectRangeRatio);
    }
}
