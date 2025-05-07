using UnityEngine;
using UnityEngine.UI;

public class Tabormenuoperator : MonoBehaviour
{
public Image[] tabImages;// Array of tab images
public GameObject[] pages;// Array of pages to be activated

    void Start()
    {
        ActivateTab(0);// Activate the first tab by default
    }

    public void ActivateTab(int tabNo)// Activate the tab based on the index passed
    {
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);// Deactivate all pages
            tabImages[i].color = Color.grey;// Change tab image color to grey
        }
        pages[tabNo].SetActive(true);// Activate the selected page
        tabImages[tabNo].color = Color.white;// Change the selected tab image color to white
    }
}
