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
            MessageBox.Show("�������˺�" , "��ʾ" , MessageBoxType.Information , "ȷ��");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("����������", "��ʾ", MessageBoxType.Information, "ȷ��");
            return;
        }
        if (string.IsNullOrEmpty(this.confirmPassword.text))
        {
            MessageBox.Show("��ȷ������", "��ʾ", MessageBoxType.Information, "ȷ��");
            return;
        }
        if (this.password.text != this.confirmPassword.text)
        {
            MessageBox.Show("������������벻һ��", "��ʾ", MessageBoxType.Information, "ȷ��");
            return;
        }

        UserService.Instance.SendSignUp(this.email.text, this.password.text);
    }



}
