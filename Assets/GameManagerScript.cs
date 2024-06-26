using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Purchasing;
using UnityEngine;
using static UnityEngine.UI.Image;
public class GameManagerScript : MonoBehaviour
{
	//
	public GameObject playerPreFab; 
	public GameObject boxPreFab;
	public GameObject noMoveBoxPreFab;
	public GameObject goalPreFab;
	public GameObject clearText;
	public GameObject particlePreFab;
    private AudioSource audioSource;

    int[,] map;
	int mapNum = 0;
	GameObject[,] field;
	List<GameObject> goalsField;

	//完了までの時間
	private float timeTaken = 0.5f;
	//経過時間
	private float timeErapsed;

	void PrintArray()
	{
		string debugText = "";
		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				debugText += map[y,x].ToString() + ",";
			}
			debugText += "\n";
		}
		Debug.Log(debugText);
	}
	Vector2Int GetPlayerIndex()
	{
		for (int y = 0; y < field.GetLength(0); y++)
		{
			for (int x = 0; x < field.GetLength(1); x++)
			{
				if (field[y,x] == null) {  continue; }
				if (field[y,x].tag == "Player")
				{
					return new Vector2Int(x,y);
				}
			}
		}
		return new Vector2Int(-1,-1);
	}
	bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo,int power = 1)
	{
		if (power < 0) {  return false; }
		if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
		if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
		if (field[moveTo.y, moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "NoMoveBox") { return false; }
		//
		if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
		{
			Vector2Int velocity = moveTo - moveFrom;

			bool success = MoveNumber(tag, moveTo, moveTo + velocity,power - 1);
			if (!success) { return false; }
		}

		Vector3 moveToPosition = new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
		field[moveFrom.y, moveFrom.x].GetComponent<move>().MoveTo(moveToPosition);

		//field[moveFrom.y,moveFrom.x].transform.position = new Vector3(moveTo.x - field.GetLength(1) / 2, -moveTo.y + field.GetLength(0) / 2, 0);
		field[moveTo.y,moveTo.x] = field[moveFrom.y,moveFrom.x];
		field[moveFrom.y,moveFrom.x] = null;
		SetParticle(moveFrom);
		return true;
	}
	bool IsCleared()
	{
		List<Vector2Int> goals = new List<Vector2Int>();
		//
		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				if (map[y, x] == 3)
				{
					goals.Add(new Vector2Int(x, y));
				}
			}
		}
		//
		for (int i = 0; i < goals.Count; i++)
		{
			GameObject f = field[goals[i].y, goals[i].x];
			if (f == null || f.tag != "Box")
			{
				return false;
			}
		}

		return true;
	}
	void Reset()
	{
		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				Destroy(field[y, x]);
			}
		}
		for(int i = 0;i < goalsField.Count;i++)
		{
			Destroy(goalsField[i]);
		}
		Start();
		clearText.SetActive(IsCleared());
	}
	// Start is called before the first frame update
	void Start()
	{
		Screen.SetResolution(1920, 1080, true);

        audioSource = this.GetComponent<AudioSource>();

        switch (mapNum)
		{
			default:
				mapNum = 0;
				map = new int[,] {
			{ 4,4,4,4,4,4,4,4,4,4,4},
			{ 4,0,0,4,0,0,0,0,3,3,4},
			{ 4,1,0,4,0,0,0,0,4,3,4},
			{ 4,0,0,4,0,2,2,0,0,0,4},
			{ 4,0,0,0,0,2,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,4,4,4,4,4,4,4,4,4,4},
			};
				break;
			case 0:
				map = new int[,] {
			{ 4,4,4,4,4,4,4,4,4,4,4},
			{ 4,0,0,4,0,0,0,0,3,3,4},
			{ 4,1,0,4,0,0,0,0,4,3,4},
			{ 4,0,0,4,0,2,2,0,0,0,4},
			{ 4,0,0,0,0,2,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,4,4,4,4,4,4,4,4,4,4},
			};
				break;
			case 1:
				map = new int[,] {
			{ 4,4,4,4,4,4,4,4,4,4,4},
			{ 4,3,0,0,2,3,0,0,0,0,4},
			{ 4,4,4,4,2,4,4,4,0,0,4},
			{ 4,0,0,0,3,0,0,0,0,0,4},
			{ 4,0,0,0,2,0,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,2,4},
			{ 4,1,0,0,0,0,0,0,2,2,4},
			{ 4,4,4,4,4,4,4,4,4,4,4},
			};
				break;
			case 2:
				map = new int[,] {
			{ 4,4,4,4,4,4,4,4,4,4,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,0,0,0,0,4,0,0,0,0,4},
			{ 4,0,0,0,2,3,2,4,0,0,4},
			{ 4,0,0,4,3,3,3,2,1,0,4},
			{ 4,0,0,0,2,3,2,4,0,0,4},
			{ 4,0,0,0,0,4,0,0,0,0,4},
			{ 4,0,0,0,0,0,0,0,0,0,4},
			{ 4,4,4,4,4,4,4,4,4,4,4},
			};
				break;
		}
		field = new GameObject[map.GetLength(0),map.GetLength(1)];
		goalsField = new List<GameObject>();

		for (int y = 0;y < map.GetLength(0);y++)
		{
			for(int x = 0;x < map.GetLength(1); x++)
			{
				if (map[y,x] == 1)
				{
					field[y,x] = Instantiate(
						playerPreFab,
						new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2 , 0),
						Quaternion.identity
						);
				}
				if (map[y, x] == 2)
				{
					field[y, x] = Instantiate(
						boxPreFab,
						new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
						Quaternion.identity
						);
				}
				if (map[y, x] == 3)
				{
					goalsField.Add(Instantiate(
						goalPreFab,
						new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0.01f),
						Quaternion.identity
						));
				}
				if (map[y, x] == 4)
				{
					field[y, x] = Instantiate(
						noMoveBoxPreFab,
						new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
						Quaternion.identity
						);
				}
			}
		}
		//PrintArray();
	}
	void SetParticle(Vector2Int playerPos)
	{
		for (int i = 0; i < 5; i++)
		{
			Instantiate(
							particlePreFab,
							new Vector3(playerPos.x - map.GetLength(1) / 2, -playerPos.y + map.GetLength(0) / 2, 0.0f),
							Quaternion.identity
							);
		}
	}
	void ClearParticle()
	{
		//時間経過を加算
		timeErapsed += Time.deltaTime;
		//経過時間が完了時間の何割かを算出
		float timeRate = timeErapsed / timeTaken;
		//完了時間を超えるようであれば実行時間相当に丸める
		if (timeRate > 1){timeRate = 1;}
		if(timeRate == 1)
		{
            timeErapsed = 0;
            Vector2Int particlePos = new Vector2Int(
                Random.Range(0, map.GetLength(1)),
                Random.Range(0, map.GetLength(0))
                );
            for (int i = 0; i < 3; i++)
            {
                SetParticle(particlePos);
            }
			audioSource.Play();
        }
	}
	// Update is called once per frame
	void Update()
	{
		if (!IsCleared())
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				Vector2Int playerIndex = GetPlayerIndex();
				MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
				clearText.SetActive(IsCleared());
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				Vector2Int playerIndex = GetPlayerIndex();
				MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
				clearText.SetActive(IsCleared());
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Vector2Int playerIndex = GetPlayerIndex();
				MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
				clearText.SetActive(IsCleared());
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Vector2Int playerIndex = GetPlayerIndex();
				MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
				clearText.SetActive(IsCleared());
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				mapNum++;
				Reset();
			}
			ClearParticle();

        }
		if (Input.GetKeyDown(KeyCode.R)) {
			Reset();
		}
	}
}
