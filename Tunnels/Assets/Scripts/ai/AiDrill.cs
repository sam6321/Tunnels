using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDrill : MonoBehaviour
{
    [SerializeField]
    private Transform drillTransform;

    [SerializeField]
    private float drillAttackDistance = 2.0f;

    [SerializeField]
    private int drillAttackDamage = 1;

    [SerializeField]
    private Cooldown drillAttackCooldown = new Cooldown(0.4f);
    private float baseCooldown;

    private PlayerResources resources;
    private int noDrillMask;
    private bool runOnce = true;

    private AiMovement aiMovement;
    
    private int sidewaysCount = 0;
    private int forceDownCount = 0;
    private float lastYPos = 0;

    void Start()
    {
        resources = GetComponent<PlayerResources>();
        noDrillMask = ~LayerMask.GetMask("Player");
        baseCooldown = drillAttackCooldown.Frequency;
        aiMovement = GetComponent<AiMovement>();
    }

    void Update()
    {
        Vector2 pos;
        TileNode next = aiMovement.NextNode();
        if(next == null)
        {
            pos = new Vector2(aiMovement.transform.position.x, aiMovement.transform.position.y - 1.0f);
        }
        else
        {
            var size = aiMovement.tileSize;
            pos = new Vector2((float)next.x, -(float)next.y) * size;
        }

        if(Mathf.Abs(transform.position.y - lastYPos) > 0.01f || forceDownCount > 100)
        {
            sidewaysCount = 0;
            forceDownCount = 0;
        }
        else
        {
            sidewaysCount++;
        }

        lastYPos = transform.position.y;

        if(sidewaysCount > 100)
        {
            forceDownCount++;
            pos = new Vector2(aiMovement.transform.position.x, aiMovement.transform.position.y - 1.0f);
        }

        Vector2 direction = (pos - (Vector2)transform.position).normalized;
        direction.y = Mathf.Clamp(direction.y, -1, 0);

        drillAttackCooldown.Frequency = (1.0f / resources.DrillSpeed) * baseCooldown;

        UpdateDrillRotation(direction);

        if(drillAttackCooldown.Check(Time.time))
        {
            DoDrillAttack(direction);
        }
    }

    void UpdateDrillRotation(Vector2 direction)
    {
        drillTransform.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
    }

    void DoDrillAttack(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, drillAttackDistance, noDrillMask);
        if(hit.collider)
        {
            hit.collider.gameObject.SendMessage("OnDrillAttack", new DrillAttackInfo(gameObject, drillAttackDamage, hit.point, hit.normal), SendMessageOptions.DontRequireReceiver);
        }
        int oil = resources.GetResourceCount(ResourceType.Oil);
        resources.SetResourceCount(ResourceType.Oil, Mathf.Max(oil - 1, 0));
    }
}
