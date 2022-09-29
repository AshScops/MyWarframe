using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UISignUp : MonoBehaviour
{
    public InputField email;
    public InputField password;
    public InputField confirmPassword;
    public Button buttonSignUp;

    void Start()
    {
        UserService.Instance.OnSignUp = this.OnSignUp;

    }

    void OnSignUp(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("{0}", msg));


    }


    void Update()
    {

    }

    public void OnClickSignUp()
    {
        if (string.IsNullOrEmpty(this.email.text))
        {
            MessageBox.Show("请输入账号" , "提示" , MessageBoxType.Information , "确认");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码", "提示", MessageBoxType.Information, "确认");
            return;
        }
        if (string.IsNullOrEmpty(this.confirmPassword.text))
        {
            MessageBox.Show("请确认密码", "提示", MessageBoxType.Information, "确认");
            return;
        }
        if (this.password.text != this.confirmPassword.text)
        {
            MessageBox.Show("两次输入的密码不一致", "提示", MessageBoxType.Information, "确认");
            return;
        }

        UserService.Instance.SendSignUp(this.email.text, this.password.text);
    }



}
