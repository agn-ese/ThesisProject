using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    private string filePath;
    private int currentRowID = 0;
    public static SaveFile instance;
    private bool nuovo = true;
    private void Awake()
    {
        // Assicurati che ci sia una sola istanza di DataManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantieni l'oggetto anche dopo il cambio di scena
            InitializeFile();
        }
        else
        {
            Destroy(gameObject); // Elimina altre istanze
        }
    }

    void InitializeFile()
    {
        filePath = Application.persistentDataPath + "/resultsNoHelp.csv";

        if (!File.Exists(filePath))
        {
            string header = "Player; Version; Task1Time1;Task2Time1;Task3Time1; Task4Time1; Level1TotalTime; Task1Time2;Task2Time2; Task3Time2; Task4Time2; Level2TotalTime; Task1Time3;Task2Time3; Task3Time3; Task4Time3; Task5Time3; Task6Time3;Task7Time3; Level3TotalTime; \n";
            File.WriteAllText(filePath, header);
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length > 1) 
        {
            string lastLine = lines[lines.Length - 1];
            string[] values = lastLine.Split(';');
            if (int.TryParse(values[0], out int parsedID))
            {
                currentRowID = parsedID;
            }
            if (nuovo)
            {
                nuovo = false;
            }
        }
    }

    public void SavePlayerData(string appVersion, List<float> taskTimes1, List<float> taskTimes2, List<float> taskTimes3, float totalLevel1Time, float totalLevel2Time, float totalLevel3Time)
    {
        if (!nuovo)
        {
            currentRowID += 1; 
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(currentRowID.ToString() + ";"); 
        sb.Append(appVersion + ";");


        // Add task times
        if (taskTimes1 != null)
        {
            foreach (var taskTime in taskTimes1)
            {
                sb.Append(taskTime.ToString() + ";");
            }
        }
        sb.Append(totalLevel1Time.ToString() + ";");

        if (taskTimes2 != null)
        {
            foreach (var taskTime in taskTimes2)
            {
                sb.Append(taskTime.ToString() + ";");
            }
        }
        sb.Append(totalLevel2Time.ToString() + ";");

        if (taskTimes3 != null)
        {
            foreach (var taskTime in taskTimes3)
            {
                sb.Append(taskTime.ToString() + ";");
            }
        }
        sb.Append(totalLevel3Time.ToString() + "\n"); // Ensure a newline is added

        File.AppendAllText(filePath, sb.ToString());
    }

}
