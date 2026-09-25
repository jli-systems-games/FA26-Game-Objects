using UnityEngine;

public class MuseumController : MonoBehaviour
{
    public GameObject galleryPanel;
    public GameObject world1Panel;
    public GameObject world2Panel;
    public GameObject world3Panel;

    private void Start()
    {
        ShowGallery();
    }

    public void ShowGallery()
    {
        galleryPanel.SetActive(true);
        world1Panel.SetActive(false);
        world2Panel.SetActive(false);
        world3Panel.SetActive(false);
    }

    public void OpenWorld1()
    {
        galleryPanel.SetActive(false);
        world1Panel.SetActive(true);
    }

    public void OpenWorld2()
    {
        galleryPanel.SetActive(false);
        world2Panel.SetActive(true);
    }

    public void OpenWorld3()
    {
        galleryPanel.SetActive(false);
        world3Panel.SetActive(true);
    }
}