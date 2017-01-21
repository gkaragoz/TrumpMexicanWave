using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class MexicanWaver : MonoBehaviour {
    public GameObject [] CulturePrefabs; //Prefabs list from Resources assets folder.

    public GameObject X_StartPosition;
    public GameObject X_EndPosition;
    public GameObject Y_StartPosition;
    public GameObject Y_EndPosition;

    public int ColumnCount = 5;         //Defined as columns count for Mexican Wavers.
    public int RowCount = 4;            //Defined as rows count for Mexican Wavers.
    public float ColumnDistance;        //Offset columns between every 2 person.
    public float RowDistance;           //Offset rows between every 2 person.

    public float StandUpTime = 0.25f;   //It's the stand up animation time(seconds) for every column.
    public float SitDownTime = 0.25f;   //It's sit down animation time(seconds) for every column.
    public float Delay = 1f;            //Delay(seconds) for the go to next column.

    void Start()
    {
        InitPersons();
    }

    public void InitPersons()   //Instantiate every persons depends on ColumnCount and RowCount.
    {
        ColumnDistance = Vector3.Distance(X_StartPosition.transform.position, X_EndPosition.transform.position) / ColumnCount;  //Calculate offsets between every persons on columns.
        RowDistance = Vector3.Distance(Y_StartPosition.transform.position, Y_EndPosition.transform.position) / RowCount;        //Calculate offsets between every persons on rows.

        #region Allignment
        float x_startPosOffset; //Offset for start position on X axis.
        float y_startPosOffset = 0; //Offset for start position on Y axis.

        //X - Y
        x_startPosOffset = ColumnDistance / 2;
        y_startPosOffset = (RowCount - 1) * RowDistance / 2;
        #endregion

        Vector3 v3 = new Vector3(X_StartPosition.transform.position.x + x_startPosOffset, X_EndPosition.transform.position.y - y_startPosOffset, 0); //Start position for Instantiate.

        for (int ii = 1; ii <= ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {
                GameObject go = Instantiate(CulturePrefabs[Random.Range(0, CulturePrefabs.Length)], v3, Quaternion.identity) as GameObject;
                go.transform.SetParent(X_StartPosition.transform);
                v3 += new Vector3(0, RowDistance, 0);
            }
            v3 = new Vector3(X_StartPosition.transform.position.x + x_startPosOffset, X_EndPosition.transform.position.y - y_startPosOffset, 0);     //Reset to start position;
            v3 += new Vector3(ColumnDistance * ii, 0, 0);
        }
    }

    public void iTween_StandUp()
    {

    }
}
