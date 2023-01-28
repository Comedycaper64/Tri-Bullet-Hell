using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTest : MonoBehaviour
{
    private float lifetime;
    private float growthRate;
    private int degreesPerSegment;
    private float radialScale;
    private float curAngle = 0;
    LineRenderer lineRenderer;

    private float colliderOffset;
    [SerializeField] private CircleCollider2D exteriorCollider;
    [SerializeField] private CircleCollider2D interiorCollider;
    public bool exteriorCollision;
    public bool interiorCollision;

    private void Awake()
    {
        exteriorCollision = false;
        interiorCollision = false;
        degreesPerSegment = 5;
        colliderOffset = 0.25f;
        radialScale = 0.1f;
        lifetime = 5f;
        growthRate = 1f;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (360 / degreesPerSegment + 2);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = Color.red;
        lineRenderer.sortingLayerName = "Player";
        lineRenderer.sortingOrder = -1;
    }

    public void SetLifetime(float lifetime)
    {
        this.lifetime = lifetime;
    }

    public void SetGrowthRate(float growthRate)
    {
        this.growthRate = growthRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        exteriorCollider.radius = radialScale + colliderOffset;
        interiorCollider.radius = radialScale - colliderOffset;
        Destroy(gameObject, lifetime);
    }

    void CreatePoints()
    {
        float x, y;
        for (int i = 0; i < 360 / degreesPerSegment + 2; i++)
        {
            x = Mathf.Sin(curAngle * Mathf.Deg2Rad);
            y = Mathf.Cos(curAngle * Mathf.Deg2Rad);
            lineRenderer.SetPosition(i, transform.position + (new Vector3(x, y, 0) * radialScale));
            curAngle += degreesPerSegment;
        }
    }

    void ExpandPulse()
    {
        exteriorCollider.radius += (growthRate * Time.deltaTime);
        interiorCollider.radius += (growthRate * Time.deltaTime);
        radialScale += (growthRate * Time.deltaTime);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CreatePoints();
        ExpandPulse();
    }
}
