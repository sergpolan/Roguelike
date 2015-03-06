using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

		public static GameManager instance = null;
		public BoardManager boardScript;
		private int level = 3; //vamos a probar el nivel 3 porque es donde aparecen los enemigos



		void Awake ()
		{
				if (instance == null) {
						instance = this;
				} else if (instance != this) {
						Destroy (gameObject);
				}
				DontDestroyOnLoad (gameObject); //para que no se borre entre escenas
				boardScript = GetComponent<BoardManager> ();
				InitGame ();
		}

		void InitGame ()
		{
				boardScript.SetupScene (level);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
