using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public delegate Vector3 MovementFn();

    public enum Behaviour
    {
        Calm,
        Enraged,
    }

    public float DEFAULT_SPEED = 5f;
    public int N_ANIM_SPRITES = 2;
    public Character player;

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    private int baseSpriteIdx = 0;

    public Rigidbody2D rigidBody;

    private Vector3 movement;
    private float speed;
    private Behaviour behaviour;
    private MovementFn movementFn;

    private Coroutine setDelayedCalmBehaviour;

    IEnumerator AnimateSprites()
    {
        var iSprite = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            this.spriteRenderer.sprite = this.sprites[this.baseSpriteIdx + iSprite];
            iSprite = (iSprite + 1) % N_ANIM_SPRITES;
        }
    }

    IEnumerator SetDelayedCalmBehaviour()
    {
        yield return new WaitForSeconds(10f);
        SetBehaviour(Behaviour.Calm);
        setDelayedCalmBehaviour = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetBehaviour(Behaviour.Calm);
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        StartCoroutine(AnimateSprites());
    }

    // Update is called once per frame
    void Update()
    {
        this.movement = this.movementFn();
        this.movement.Normalize();
        this.rigidBody.velocity = speed * this.movement;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Character>();
        if (player)
        {
            SetBehaviour(Behaviour.Enraged);
        }

        var wall = collision.gameObject.GetComponent<Wall>();
        if (wall)
        {
            switch (wall.orientation)
            {
                case Wall.Orientation.North:
                    this.movement = GenerateRandomMovement(0, 1, -1, -1);
                    break;
                case Wall.Orientation.East:
                    this.movement = GenerateRandomMovement(1, 0, -1, -1);
                    break;
                case Wall.Orientation.South:
                    this.movement = GenerateRandomMovement(1, 1, 0, -1);
                    break;
                case Wall.Orientation.West:
                    this.movement = GenerateRandomMovement(1, 1, -1, 0);
                    break;
            }
        }
    }

    private void SetBehaviour(Behaviour behaviour)
    {
        this.behaviour = behaviour;
        switch (behaviour)
        {
            case Behaviour.Calm:
                this.movementFn = () => (this.player.transform.position - this.transform.position);
                this.speed = DEFAULT_SPEED;
                this.baseSpriteIdx = 0;
                break;
            case Behaviour.Enraged:
                this.movement = GenerateRandomMovement();
                this.movementFn = () => this.movement;
                this.speed = DEFAULT_SPEED * 2.5f;
                this.baseSpriteIdx = 2;
                if (setDelayedCalmBehaviour != null)
                {
                    StopCoroutine(setDelayedCalmBehaviour);
                }
                this.setDelayedCalmBehaviour = StartCoroutine(SetDelayedCalmBehaviour());
                break;
        }
    }

    private Vector3 GenerateRandomMovement(float topConstraint = 1f, float rightContraint = 1f, float bottomConstraint = -1f, float leftConstraint = -1f)
    {
        return new Vector3(Random.Range(leftConstraint, rightContraint), Random.Range(bottomConstraint, topConstraint), 0);
    }
}
