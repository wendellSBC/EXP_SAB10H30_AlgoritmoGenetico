using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class BaseEnemy : MonoBehaviour {

	[System.Serializable]
	public class Brain
	{
		public DNA dna;
		public BaseEnemy mainBody;

		public float rayRange = 1;
		public int maxGeneNumber = 360;
		public int geneQuantity = 3;
		public float timeAlive = 0;

		public bool _Spike;
		public bool _Danger;

		public bool isAlive;

		public void Initialize(BaseEnemy be)
		{
			mainBody = be;
			dna = new DNA(360,3);
			Physics2D.queriesStartInColliders = false;

			if (Random.Range(0,100) < 11)
			{
				dna.Mutate();
			}
			mainBody.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360f));

			dna.SetGeneValues();
		}

		public void Move()
		{
			float direction = 0;

			if (_Spike)
			{
				direction = dna.GetGene(0);
				if (dna.GetGene(2) > 180)
				{
					mainBody.rb.velocity = Vector2.zero;
				}
			}
			else if (_Danger)
			{
				direction = dna.GetGene(1);
				if (dna.GetGene(2) > 180)
				{
					mainBody.rb.velocity = Vector2.zero;
				}
			}

			mainBody.transform.rotation *= Quaternion.Euler(0,0,direction);

			mainBody.rb.AddForce(mainBody.transform.up.normalized * mainBody.speed * Time.deltaTime,ForceMode2D.Force);

		}

		public void FireRaycasts()
		{
			_Spike = false;
			_Danger = false;

			Vector2 diagonalRight = (mainBody.transform.up + mainBody.transform.right) / 2;
			Vector2 diagonalLeft = (mainBody.transform.up + -mainBody.transform.right) / 2;

			Debug.DrawRay(mainBody.transform.position,diagonalRight,Color.red);
			Debug.DrawRay(mainBody.transform.position,diagonalLeft,Color.blue);
			Debug.DrawRay(mainBody.transform.position,mainBody.transform.up,Color.green);

			RaycastHit2D hit = Physics2D.Raycast(mainBody.transform.position,diagonalRight,rayRange);

			if (hit)			
				CompareTags(hit);

			hit = Physics2D.Raycast(mainBody.transform.position,diagonalLeft,rayRange);
			if (hit)
				CompareTags(hit);
				
			hit = Physics2D.Raycast(mainBody.transform.position,mainBody.transform.up,rayRange);
			if (hit)
				CompareTags(hit);
			


		}
		public void ExecuteMovement()
		{
			FireRaycasts();
			Move();
		}

		public void CompareTags(RaycastHit2D hit)
		{
			if (hit.transform.CompareTag("Spike"))
			{
				_Spike = true;
			}
			else if (hit.transform.CompareTag("Danger"))
			{
				_Danger = true;
			}
		}
	}

	public bool useBrain = true;
	public Brain brain;

    public float maxVelocityMagnitude = 5;
    public float speed = 60;
    public bool quickBounce = true;
    public bool randomizeBounceDirection = true;
    public bool limitVelocity = true;
    Rigidbody2D rb;

    public virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
		
    }

    void Update() {

		if (useBrain)
		{
			brain.ExecuteMovement();
		}
		else
		{
			Move();			
		}

        if (limitVelocity)
            LimitVelocity();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1);
        if (hit)
            if (hit.collider.CompareTag("Danger"))
                TurnRight();

    }

    protected void LimitVelocity() {

        if (Mathf.Abs(rb.velocity.x) > maxVelocityMagnitude)
            rb.velocity = new Vector2(maxVelocityMagnitude * Mathf.Sign(rb.velocity.x), rb.velocity.y);


        if (Mathf.Abs(rb.velocity.y) > maxVelocityMagnitude)
            rb.velocity = new Vector2(rb.velocity.x, maxVelocityMagnitude * Mathf.Sign(rb.velocity.y));
    }

    protected virtual void Move() {
        rb.AddForce(transform.up.normalized * speed * Time.deltaTime, ForceMode2D.Force);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {

		if (collision.transform.CompareTag("Spike")){
			DestroyEnemy();
		}
		else if (collision.transform.CompareTag("Danger"))
		{
			DestroyEnemy();
		}

        transform.up = -collision.contacts[0].point;


        if (quickBounce)
            rb.AddForce(collision.relativeVelocity, ForceMode2D.Impulse);

        if (randomizeBounceDirection)
            transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-60f, 60f));

    }

    public virtual void DestroyEnemy() {
        // Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }


    public void TurnRight() {
        rb.velocity = Vector2.zero;
        transform.rotation *= Quaternion.Euler(0, 0, -90);

    }

}
