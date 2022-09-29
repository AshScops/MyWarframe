using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UINameBar : MonoBehaviour
{
    public Text avatarText;
    public Character character;
    public new GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        if(this.character != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateInfo();

        if (camera == null)
            camera = GameObject.Find("PlayerFollowCamera");
        else
            this.transform.forward = (this.transform.position - camera.transform.position).normalized;

    }

    private void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name;
            if(name != this.avatarText.text)
            {
                this.avatarText.text = name;
            }
        }
    }
}
