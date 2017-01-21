using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MexicanWaver : MonoBehaviour {
    public GameObject[,] People;         //People list which is already instantiated to scene.
    public GameObject[] CulturePrefabs;  //Prefabs list from Resources assets folder.

    public GameObject X_StartPosition;
    public GameObject X_EndPosition;
    public GameObject Y_StartPosition;
    public GameObject Y_EndPosition;

    public float distance;

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
        distance = Random.Range(0.5f, 1f);
        //INITIALIZING START - END POINTS REFERENCES.
        X_StartPosition = GameObject.Find("X_START_POSITION");
        X_EndPosition   = GameObject.Find("X_END_POSITION");
        Y_StartPosition = GameObject.Find("Y_START_POSITION");
        Y_EndPosition   = GameObject.Find("Y_END_POSITION");

        People = new GameObject [ColumnCount, RowCount];
    }

    void MakeRandomCulture(GameObject culturePrefab, int count)
    {
        for (int ii = 0; ii < ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {

            }
        }
    }

    void Start()
    {
        InitPersons();
        GenerateUniqueRandomNumbers(ColumnCount);
    }
     
    public void InitPersons()   //Instantiate every persons depends on ColumnCount and RowCount.
    {
        //Declare parent object for instantiating people to organize hierarchy.
        GameObject parentObject = new GameObject("People");
        parentObject.transform.position = Vector3.zero;

        ColumnDistance = Mathf.Sqrt(Mathf.Pow(X_StartPosition.transform.position.x - X_EndPosition.transform.position.x, 2)) / ColumnCount; //Calculate offsets between every persons on columns.
        RowDistance = Mathf.Sqrt(Mathf.Pow(Y_StartPosition.transform.position.y - Y_EndPosition.transform.position.y, 2)) / RowCount;       //Calculate offsets between every persons on rows.

        #region Allignment
        float x_startPosOffset; //Offset for start position on X axis.
        float y_startPosOffset = 0; //Offset for start position on Y axis.

        //X - Y Maths.
        x_startPosOffset = ColumnDistance / 2;
        y_startPosOffset = (RowCount - 1) * RowDistance / 2;
        #endregion

        Vector3 v3 = new Vector3(X_StartPosition.transform.position.x + x_startPosOffset, X_EndPosition.transform.position.y - y_startPosOffset, 0); //Start position for Instantiate.

        for (int ii = 0; ii < ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {
                GameObject go = Instantiate(CulturePrefabs[Random.Range(0,CulturePrefabs.Length)], v3, Quaternion.identity) as GameObject;
                People[ii, jj] = go;
                go.transform.SetParent(parentObject.transform);     //Set parent for organize hierarchy.
                v3 += new Vector3(0, RowDistance, 0);               //Set offsets for every Row.
            }
            v3 = new Vector3(X_StartPosition.transform.position.x + x_startPosOffset, X_EndPosition.transform.position.y - y_startPosOffset, 0);     //Reset to start position;
            v3 += new Vector3(ColumnDistance * (ii + 1), 0, 0);     //Set offsets for every Column.
        }
    }

    public IEnumerator iTween_StandUp()
    {
        for (int ii = 0; ii < ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {
                LeanTween.moveY(
                    People[ii, jj], 
                    People[ii, jj].transform.position.y + distance, 
                    Random.Range(0.5f, 1.0f));
            }
            yield return new WaitForSeconds(Delay);
        }
    }

    public IEnumerator iTween_SitDown()
    {
        yield return new WaitForSeconds(OnActiveTime);

        for (int ii = 0; ii < ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {
                LeanTween.moveY(
                    People[ii, jj],
                    People[ii, jj].transform.position.y - distance,
                    Random.Range(0.5f, 1.0f));
            }
            yield return new WaitForSeconds(Delay);
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
