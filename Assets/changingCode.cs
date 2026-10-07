using UnityEngine;

public class changingCode : MonoBehaviour
{
    public GameObject start;
    public GameObject front;
    public GameObject choose;
    public GameObject live;
    public GameObject die;

    void Start()
    {
        StartPanel();
    }

    public void StartPanel()
    {
        start.SetActive(true);
        front.SetActive(false);
        choose.SetActive(false);
        live.SetActive(false);
        die.SetActive(false);
    }

    public void FrontPanel()
    {
        start.SetActive(false);
        front.SetActive(true);
        choose.SetActive(false);
        live.SetActive(false);
        die.SetActive(false);
    }

    public void ChoosePanel()
    {
        start.SetActive(false);
        front.SetActive(false);
        choose.SetActive(true);
        live.SetActive(false);
        die.SetActive(false);
    }

    public void LivePanel()
    {
        start.SetActive(false);
        front.SetActive(false);
        choose.SetActive(false);
        live.SetActive(true);
        die.SetActive(false);
    }

    public void DiePanel()
    {
        start.SetActive(false);
        front.SetActive(false);
        choose.SetActive(false);
        live.SetActive(false);
        die.SetActive(true);
    }
}