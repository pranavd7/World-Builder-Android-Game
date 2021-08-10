using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropEventHandler : MonoBehaviour
{
    [SerializeField] Button sliderButton;

    // Start is called before the first frame update
    void Start()
    {
        DragDrop.outsideEvent += HideDrawer;
    }

    void HideDrawer(object sender, System.EventArgs e)
    {
        sliderButton.onClick.Invoke();
    }
}
