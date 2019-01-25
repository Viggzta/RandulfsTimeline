using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using SFB;
using System.IO;

public class TimeLineCreator : MonoBehaviour
{
    struct Time
    {
        public int h;
        public int m;
        public float s;

        public Time(int h, int m, float s)
        {
            this.h = h;
            this.m = m;
            this.s = s;
        }
    }

    private int colorIndex;
    public Color[] epochColors;

    private string path;
    private Time startTime;
    private Time endTime;
    private GameObject previousEpoch;

    public Transform spawnPos;
    public Transform endPos;
    public Transform spawnPosEvents;
    public Transform spawnPosEpochs;
    public GameObject epochPrefab;
    public GameObject eventPrefab;

    private void Start()
    {
        colorIndex = 0;
        spawnPosEpochs = spawnPos.GetChild(0);
        spawnPosEvents = spawnPos.GetChild(1);
    }

    public void LoadLog()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        if (paths.Length != 1)
        {
            return; // No file selected
        }
        path = paths[0];

        List<string> info = new List<string>();
        StreamReader sr = new StreamReader(path);
        while (sr.EndOfStream == false)
        {
            info.Add(sr.ReadLine());
        }

        startTime = GetTimeOfEvent(info[0]);
        endTime = GetTimeOfEvent(info[info.Count - 1]);

        Debug.Log(endTime.h + " " + endTime.m + " " + endTime.s);

        previousEpoch = Instantiate(epochPrefab, spawnPosEpochs);
        previousEpoch.GetComponent<Epoch>().SetColor(GetNextEpochColor());
        RectTransform epoch1Transform = previousEpoch.GetComponent<RectTransform>();
        epoch1Transform.sizeDelta = new Vector2(TimelineLength(), epoch1Transform.sizeDelta.y);

        for (int i = 0; i < info.Count; i++)
        {
            Time occurance = GetTimeOfEvent(info[i]);
            string[] infoSplit = info[i].Split(';');

            if (infoSplit[0] == "Epoch")
            {
                // Add epoch
                AddEpoch(occurance);
            }
            else
            {
                // Add event
                GameObject tempEvent = Instantiate(eventPrefab, spawnPosEvents);
                tempEvent.transform.position = new Vector3(tempEvent.transform.position.x + TimelineLength() * GetTimePercent(occurance, endTime),
                    tempEvent.transform.position.y, tempEvent.transform.position.z);
                EventConstraints e = tempEvent.GetComponent<EventConstraints>();

                e.SetText(infoSplit[0], infoSplit[1], int.Parse(infoSplit[2]), int.Parse(infoSplit[3]), float.Parse(infoSplit[4]));
            }
        }
    }

    private void AddEpoch(Time occurance)
    {
        RectTransform prevEpochTransform = previousEpoch.GetComponent<RectTransform>();
        prevEpochTransform.sizeDelta = new Vector2(prevEpochTransform.sizeDelta.x - 
            TimelineLength() * (1 - GetTimePercent(occurance, endTime)), prevEpochTransform.sizeDelta.y); // Felet

        GameObject newEpoch = Instantiate(epochPrefab, spawnPosEpochs);
        newEpoch.GetComponent<Epoch>().SetColor(GetNextEpochColor());
        RectTransform epochTransform = newEpoch.GetComponent<RectTransform>();
        epochTransform.position = spawnPosEpochs.position + new Vector3(TimelineLength() * GetTimePercent(occurance, endTime), 0);
        epochTransform.sizeDelta = new Vector2(endPos.position.x - epochTransform.position.x, epochTransform.sizeDelta.y);

        previousEpoch = newEpoch;
    }

    private Color GetNextEpochColor()
    {
        Color returnColor = epochColors[colorIndex];
        colorIndex++;
        colorIndex %= epochColors.Length;

        return returnColor;
    }

    private float TimelineLength()
    {
        return endPos.position.x - spawnPos.position.x;
    }

    private float GetTimePercent(Time self, Time other)
    {
        decimal fractionSelf = (decimal)self.h + (decimal)(self.m / 60d) + (decimal)(self.s / 3600d);
        decimal fractionOther = (decimal)other.h + (decimal)(other.m / 60d) + (decimal)(other.s / 3600d);

        return (float)(fractionSelf / fractionOther);
    }

    private Time GetTimeOfEvent(string input)
    {
        string[] splitData = input.Split(';');

        Time t = new Time();
        t.h = int.Parse(splitData[2]);
        t.m = int.Parse(splitData[3]);
        t.s = float.Parse(splitData[4]);
        return t;
    }
}
