using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField]
    private int health = 10;

    private int startHealth;
    private float healthLerp;

    private SpriteRenderer spriteRenderer;
    private SpriteMask mask;
    private ParticleSystem.EmitParams p = new ParticleSystem.EmitParams();

    public ParticleSystem OnDrillParticleSystem { get; set; }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
        startHealth = health;
        healthLerp = startHealth;
    }

    void Update()
    {
        //healthLerp = Mathf.MoveTowards(healthLerp, health, Time.deltaTime * 10.0f);
        mask.alphaCutoff = 1 - Mathf.InverseLerp(0, startHealth, health);
    }

    void OnDrillAttack(DrillAttackInfo info)
    {
        health -= info.damage;

        /*Color colour = spriteRenderer.color;
        colour.a = Mathf.InverseLerp(0, startHealth, health);
        spriteRenderer.color = colour;*/

        p.position = new Vector3(0, 0, -1) + (Vector3)(info.position + info.normal * 0.01f);
        p.velocity = info.normal + Random.insideUnitCircle * 1.5f;
        OnDrillParticleSystem.Emit(p, Random.Range(1, 2));

        if (health <= 0)
        {
            SendMessage("OnTileBreak", info);
            Destroy(gameObject);
        }
    }
}
