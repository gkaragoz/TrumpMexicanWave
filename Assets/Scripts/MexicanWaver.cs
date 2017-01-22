using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MexicanWaver : MonoBehaviour {
	
	private GameObject _parent;

    public GameObject TileBackground;
    public GameObject[,] People;         //People list which is already instantiated to scene.
    public GameObject[] CulturePrefabs;  //Prefabs list from Resources assets folder.

    public GameObject X_StartPosition;
    public GameObject X_EndPosition;
    public GameObject Y_StartPosition;
    public GameObject Y_EndPosition;

    public int ColumnCount = 5;         //Defined as columns count for Mexican Wavers.
    public int RowCount = 4;            //Defined as rows count for Mexican Wavers.
    public float ColumnDistance;        //Offset columns between every 2 person.
    public float RowDistance;           //Offset rows between every 2 person.

    public float OnActiveTime = 1f;
    public float StandUpTime = 0.25f;   //It's the stand up animation time(seconds) for every column.
    public float SitDownTime = 0.25f;   //It's sit down animation time(seconds) for every column.
    public float Delay = 1.2f;          //Delay(seconds) for the go to next column.

    void Awake()
    {
        //INITIALIZING START - END POINTS REFERENCES.
        X_StartPosition = GameObject.Find("X_START_POSITION");
        X_EndPosition   = GameObject.Find("X_END_POSITION");
        Y_StartPosition = GameObject.Find("Y_START_POSITION");
        Y_EndPosition   = GameObject.Find("Y_END_POSITION");

        People = new GameObject [ColumnCount, RowCount];
    }

	public void ClearWave()
	{
		for(int i = 0; i < ColumnCount; i++)
		{
			for(int j = 0; j < RowCount; j++)
			{
				if( People[i,j] != null )
					Destroy( People[i,j] );
			}
		}

        People = new GameObject [ColumnCount, RowCount];

		Destroy(_parent);
	}

    public void InitWave(List<Culture> angryCultures, int maxAngries = 10)   //Instantiate every persons depends on ColumnCount and RowCount.
    {
        //Declare parent object for instantiating people to organize hierarchy.
        GameObject parentObject = new GameObject("People");
		_parent = parentObject;
        parentObject.transform.position = Vector3.zero;


        //Auto Calculate.
        //RowDistance = Mathf.Sqrt(Mathf.Pow(Y_StartPosition.transform.position.y - Y_EndPosition.transform.position.y, 2)) / RowCount;       //Calculate offsets between every persons on rows.
        ColumnDistance = Mathf.Sqrt(Mathf.Pow(X_StartPosition.transform.position.x - X_EndPosition.transform.position.x, 2)) / ColumnCount; //Calculate offsets between every persons on columns.

        #region Allignment
        float x_startPosOffset; //Offset for start position on X axis.
        float y_startPosOffset = 0; //Offset for start position on Y axis.

        //X - Y Maths.
        x_startPosOffset = ColumnDistance * 0.5f;
        y_startPosOffset = (RowCount - 1) * RowDistance * 0.5f;
        #endregion

        Vector3 v3 = new Vector3(X_StartPosition.transform.position.x + x_startPosOffset, X_EndPosition.transform.position.y - y_startPosOffset, 0); //Start position for Instantiate.

		//The amount of people spawned from angry cultures
		int spawnedAngries = 0;
		//The spawn index controls the frequency of spawns to ensure a varied layout of angries
		//Basically it stops all the angries from potentially spawning in the beginning
		int spawnIndex = 1000;
		//The size of the gap in between spawn indexes. You have to wait this much before spawning a new
		//angry
		int spawnGap = ColumnCount * RowCount / maxAngries;

		//Debug.Log("Starting generation. Spawn gap is: " + spawnGap );

        for (int ii = 0; ii < ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {
#region Culture Determination

				//Pick a random number to be later cast to a culture
				int culture = Random.Range(0, CulturePrefabs.Length);

				//The current index of the spawn
				spawnIndex = ii * RowCount + jj;

				if( angryCultures.Contains( (Culture) culture )  // If we're an angry culture...
						&& spawnedAngries < maxAngries // ...and we can still spawn angries...
						&& spawnIndex > spawnGap * spawnedAngries // ... and we haven't spawned an angry recently
				 )
				{
					//Spawn an angry
					spawnedAngries++;

					//Debug.Log("Spawned a dude. Index is at " + spawnIndex + ". spawnedAngries is at: " + spawnedAngries );
					//Debug.Log("spawnGap * spawnedAngries = " + spawnGap * spawnedAngries);
				}
				else
				{
					//If we've already spawned more than 'maxAngries' people from an angry culture
					//...then lets make sure the culture int represents a happy culture
					while( angryCultures.Contains((Culture)culture) )
					{
						culture++;
						if( culture >= CulturePrefabs.Length )
							culture = 0;
					}
				}
#endregion

                GameObject go = Instantiate(CulturePrefabs[culture], v3, Quaternion.identity) as GameObject;

				//Doing this check twice, we know.
				if(!angryCultures.Contains( (Culture) culture ) )
				{
					go.layer = 2;
				}

				//Let them know if they're angry
				var viewerController = go.GetComponent<ViewerController>();
				if( angryCultures.Contains( viewerController.ActiveCulture ) )
				   viewerController.HatesTrump = true;

                People[ii, jj] = go;
                go.transform.SetParent(parentObject.transform);                         //Set parent for organize hierarchy.

                float randomSeperatingOnX = Random.Range(-0.3f, 0.3f);
                v3 += new Vector3(randomSeperatingOnX, RowDistance, 0);             //Set offsets for every Row.

                // -0.3f // +0.3f
                ///////////////////////////////////////////////////////////////////////////////////////////
            }
            v3 = new Vector3(X_StartPosition.transform.position.x + x_startPosOffset, X_EndPosition.transform.position.y - y_startPosOffset, 0);     //Reset to start position;
            v3 += new Vector3(ColumnDistance * (ii + 1), 0, 0);     //Set offsets for every Column.
        }

        BackgroundTiler();
    }

    private void BackgroundTiler()
    {
        //3.38f
        float distance = X_StartPosition.transform.position.x - X_EndPosition.transform.position.x;
        float iterationCount = Mathf.Abs (distance / TileBackground.GetComponent<SpriteRenderer>().bounds.size.x);
        Vector3 startPos = X_StartPosition.transform.position;

        Vector3 v3 = startPos;

        for (int ii = 0; ii < iterationCount + 2; ii++)
        {
            GameObject tile = Instantiate(TileBackground, v3, Quaternion.identity) as GameObject;
            v3 = startPos + Vector3.right * ii * tile.GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    public List <int> GenerateUniqueRandomNumbers(int maxCount)
    {
        List <int> uniques = new List<int>();
        for (int ii = 0; ii < maxCount; ii++)
        {
            int rand = 0;
            bool success = false;
            do
            {
                rand = Random.Range(0, maxCount);

                if (uniques.Count <= 0 || !uniques.Contains(rand))
                {
                    if (uniques.Count <= 0)
                        break;

                    success = true;
                    break;
                }
            } while (!success);

            uniques.Add(rand);
        }
        return uniques;
    }
}
