using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


//****REFER TO CURRENT ORDERS SCRIPT FOR FULL COMMENTS*****

public class FinishedOrders: MonoBehaviour
{
    public List<string> FinishedOrderData = new List<string>();

    public string listInfo;

    public Text info;

    public void ReceieveData(string FinishedOrderStringPHPMany)
    {
        string newFinishedOrderStringPHPMany = fixJson(FinishedOrderStringPHPMany);

        Debug.Log(newFinishedOrderStringPHPMany);

        FinishedOrdersJSON[] FinishedOrdersObjectArray = JsonHelper.FromJson<FinishedOrdersJSON>(newFinishedOrderStringPHPMany);

        FinishedOrderData.Clear();
        listInfo = "";

        for (int i = 0; i < FinishedOrdersObjectArray.Length; i++)
        {
            Debug.Log("ONo:" + FinishedOrdersObjectArray[i].FinONo + ", Company:" + FinishedOrdersObjectArray[i].Company + ", Planned Start:" + FinishedOrdersObjectArray[i].Start + ", Planned End:" + FinishedOrdersObjectArray[i].End + ", State:" + FinishedOrdersObjectArray[i].State);
            
            FinishedOrderData.Add("Order Number: " + FinishedOrdersObjectArray[i].FinONo + ", Company Name: " + FinishedOrdersObjectArray[i].Company + ", Planned Start Time: " + FinishedOrdersObjectArray[i].Start + ", Planned End Time: " + FinishedOrdersObjectArray[i].End + ", Build State: " + FinishedOrdersObjectArray[i].State);
        }

        foreach (var listMember in FinishedOrderData)
        {
            listInfo += listMember.ToString() + "\n" + "\n";
        }

        info.text = listInfo;
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public void GetRequestPublic()
    {
        StartCoroutine(GetRequest("http://172.21.0.90/SQLData.php?Command=finishedOrders&numOrders=10"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    ReceieveData(webRequest.downloadHandler.text);
                    Debug.LogError("Finished Orders Success");

                    break;
            }
        }
    }
}
