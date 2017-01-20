using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class MexicanWaver : MonoBehaviour {

    /*public class Culture
    {
        public string CultureName;  //Mexican Waver type as culture.
        public GameObject Prefab;   //Mexican Waver prefab. It depends on appearance for every different culture.

        public Culture(string CultureName, GameObject Prefab)
        {
            this.CultureName = CultureName;
            this.Prefab = Prefab;
        }
    }*/

    //public List <Culture> Cultures = new List<Culture>();

    public GameObject [] CulturePrefabs; //Prefabs list from Resources assets folder.

    public GameObject StartPosition;
    public GameObject EndPosition;

    public int ColumnCount = 5;         //Defined as columns count for Mexican Wavers.
    public int RowCount = 4;            //Defined as rows count for Mexican Wavers.
    public float ColumnDistance;        //Offset columns between every 2 person.
    public float RowDistance;           //Offset rows between every 2 person.

    public float StandUpTime = 0.25f;   //It's the stand up animation time(seconds) for every column.
    public float SitDownTime = 0.25f;   //It's sit down animation time(seconds) for every column.
    public float Delay = 1f;            //Delay(seconds) for the go to next column.

    void Awake()
    {
    }

    void Start()
    {
        InitPersons();
    }

    public void InitPersons()   //Instantiate every persons depends on ColumnCount and RowCount.
    {
        RowDistance = (Mathf.Abs(StartPosition.transform.position.x) + Mathf.Abs(EndPosition.transform.position.x)) / RowCount; //Calculate offsets between every person.
        Vector3 v3 = new Vector3(StartPosition.transform.position.x, StartPosition.transform.position.y - 2.25f, 0);                    //Start position for Instantiate.

        for (int ii = 0; ii <= ColumnCount; ii++)
        {
            for (int jj = 0; jj < RowCount; jj++)
            {
                Instantiate(CulturePrefabs[Random.Range(0, CulturePrefabs.Length)], v3, Quaternion.identity);
                v3 += new Vector3(0, ColumnDistance, 0);
            }
            v3 = new Vector3(StartPosition.transform.position.x, StartPosition.transform.position.y - 2.25f, 0); //Reset to start position;
            v3 += new Vector3(RowDistance * ii, 0, 0);
        }
    }

    public void iTween_StandUp()
    {

    }

    public int DirCount(DirectoryInfo d)
    {
        int i = 0;
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("jpg"))
                i++;
        }
        return i;
    }
}
