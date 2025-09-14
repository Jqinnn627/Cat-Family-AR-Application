using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMenu : MonoBehaviour
{
    public AudioSource audio;
    public GameObject infoDialogPanel;
    // Start is called before the first frame update
    public void playButton()
    {
        audio.Play();
    }

    public void showDialog() {
        infoDialogPanel.SetActive(true);
    }

    public void closeDialog()
    {
        infoDialogPanel.SetActive(false);
    }

}
