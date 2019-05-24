using UnityEngine;

public class DrillAttackInfo
{
    public GameObject attacker;
    public int damage;
    public Vector2 position;
    public Vector2 normal;

    public DrillAttackInfo(GameObject attacker, int damage, Vector2 position, Vector2 normal)
    {
        this.attacker = attacker;
        this.damage = damage;
        this.position = position;
        this.normal = normal;
    }
}

public class PlayerDrill : MonoBehaviour
{
    [SerializeField]
    private Transform drillTransform;

    [SerializeField]
    private float drillAttackDistance = 2.0f;

    [SerializeField]
    private int drillAttackDamage = 1;

    [SerializeField]
    private Cooldown drillAttackCooldown = new Cooldown(0.5f);

    private PlayerResources resources;
    private int noDrillMask;

    void Start()
    {
        resources = GetComponent<PlayerResources>();
        noDrillMask = ~LayerMask.GetMask("Player");
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        direction.y = Mathf.Clamp(direction.y, -1, 0);

        UpdateDrillRotation(direction);

        if(drillAttackCooldown.Check(Time.time) && Input.GetButton("Fire1"))
        {
            DoDrillAttack(direction);
        }
    }

    void UpdateDrillRotation(Vector2 direction)
    {
        // Set transform from mouse position.
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
