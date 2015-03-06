using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
	[Serializable]
	public class Count
	{
		public int maximum;
		public int minimum;

		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject[] floorTiles; 
	public GameObject[] wallTiles; 
	public GameObject[] foodTiles; 
	public GameObject[] enemyTiles; 
	public GameObject[] outerWallTiles; 

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3> ();

	void InitialiseList()
	{
		gridPositions.Clear ();
		for (int x = 1; x < columns -1; x++) {
			for (int y = 1; y < rows -1; y++)
			{
				gridPositions.Add(new Vector3(x,y, 0f));
			}
		}
	}

	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < columns +1; x++) {
			for (int y = -1; y < rows +1; y++)
			{
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
				if(x == -1 || x == columns || y == -1 || y == rows)
				{
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				/* Quaternion.identity es para crearlo sin rotaccion
				 * y as GameObject es para castearlo
				 */
				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject; 
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex); // para no crear dos elementos en el mismo sitio, indica que la posicion esta ocupada
		return randomPosition;
	}

	void LayaoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1); // numero de elementos a spawnear, como el numero de muros en un nivel
		for (int i = 0; i < objectCount; i++) 
		{
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity); //crea el elemento tileChoice en una posicion random sin rotacion
		}
	}

	public void SetupScene(int level)
	{
		BoardSetup ();
		InitialiseList ();
		LayaoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayaoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f); //1 enemy a nivel 2, 2 enemigos al nivel 4, 3 a nivel 8
		LayaoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity); //siempre la crearemos arriba a la derecha 
	}

}
