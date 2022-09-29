using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using SkillBridge.Message;
using ProtoBuf;
using Services;

public class LoadingManager : MonoBehaviour
{
    public PlayerInputController playerInputController;
    public GameObject UITips;
    public GameObject UILoadingBar;
    public GameObject UILogin;

    public Slider progressBar;
    public Text progressText;

    private IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        playerInputController.enabled = false;

        UITips.SetActive(true);
        UILoadingBar.SetActive(false);
        UILogin.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        UILoadingBar.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        UITips.SetActive(false);

        yield return DataManager.Instance.LoadData();

        //Init basic services
        MapService.Instance.Init();
        UserService.Instance.Init();


        // Fake Loading Simulate
        for (float i = 70; i < 100;)
        {
            i += Random.Range(0.1f, 1.5f);
            progressBar.value = i;
            yield return new WaitForEndOfFrame();
        }

        UILoadingBar.SetActive(false);
        UILogin.SetActive(true);
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
