using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using DG.Tweening;

public class UILogin : MonoBehaviour
{
    public InputField email;
    public InputField password;
    public Button buttonLogin;
    public GameObject playerFollowCamera;
    public Canvas startCanvas;
    public GameObject player;

    private void Start()
    {
       UserService.Instance.OnLogin = this.OnLogin;
    }


    void OnLogin(SkillBridge.Message.Result result, string msg)
    {
        if(result == SkillBridge.Message.Result.Failed)
            MessageBox.Show(string.Format("{0}", msg));
        else
        {
            //SceneManager.Instance.LoadScene("MainRoom");
            //startCanvas.transform.Find("Bg").DOMove(new Vector3(1920 / 2f, 1080*2, 0), 1f);

            Transform[] parent = startCanvas.GetComponentsInChildren<Transform>();
            foreach (var child in parent)
            {
                child.GetComponent<Image>()?.DOColor(new Color(0, 0, 0, 0), 1f);
                child.GetComponent<Text>()?.DOColor(new Color(0, 0, 0, 0), 1f);
                child.GetComponent<RawImage>()?.DOColor(new Color(0, 0, 0, 0), 1f);
            }

            player.transform.GetChild(0).GetComponent<Animator>().SetTrigger("StandUp");
            playerFollowCamera.SetActive(true);
            player.GetComponent<PlayerInputController>().enabled = true;

            UserService.Instance.SendGameEnter(0);
        }
    }


    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.email.text))
        {
            MessageBox.Show("用户名不能为空", "提示", MessageBoxType.Information, "确认");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("密码不能为空", "提示", MessageBoxType.Information, "确认");
            return;
        }

        UserService.Instance.SendLogin(this.email.text, this.password.text);
    }

}
