using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Esto es para que pueda hacerse override, para diferentes implementaciones
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start * new Vector2 (xDir, yDir);

		boxCollider.enabled = false; //para no chocarnos con nosotros mismos
		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) //para ver si chocamos con algo
		{
			StartCoroutine(SmoothMovement(end));
			return true;
		}
		return false; //no se pudo mover
	}

	protected IEnumerator SmoothMovement (Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while(sqrRemainingDistance > float.Epsilon)
		{
			Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition(newPosition):
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null; // esperar 1 frame para volver a evaluar la condicion del while
		}
	}

		// Para sabre si hay muros que pueda atacar el jugador
		protected virtual void AttemptMove <T> (int xDir, int yDir)where T : Component
		{
			RaycastHit2D hit;
			bool canMove = Move (xDir, yDir, out hit);

			if (hit.transform == null) //
			{
				return;
			}
		T hitComponent = hit.transform.GetComponent<T> ();

			//Es que el usuario esta blockeado y no puede interactuar con el
			if(!canMove && hitComponent != null)
			{
				OnCantMove(hitComponent);
			}

		protected abstract void OnCantMove <T> (T Component)where T : Component;

	}