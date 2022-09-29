using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;

public class PlayerInputController : MonoBehaviour {
    public Transform playerFollowCamera;
    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;

    public Character character;

    public float rotateSpeed = 2.0f;

    public float offsetAngle = 0;

    Vector3 direction = Vector3.zero;

    public int speed;

    public EntityController entityController;

    public bool onAir = false;

    // Use this for initialization
    void Start ()
    {
        state = SkillBridge.Message.CharacterState.Idle;
        if(this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;
        }
    }


    void FixedUpdate()
    {
        if (character == null)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 rawDirection = Vector3.zero;

        if (v > 0.01f && h > 0.01f)
        {
            rawDirection = this.transform.position - playerFollowCamera.position;
            direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
            direction = Quaternion.AngleAxis(45 + offsetAngle, Vector3.up) * direction;
        }
        else if (v > 0.01f && h < -0.01f)
        {
            rawDirection = this.transform.position - playerFollowCamera.position;
            direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
            direction = Quaternion.AngleAxis(-45 - offsetAngle, Vector3.up) * direction;
        }
        else if (v < -0.01f && h > 0.01f)
        {
            rawDirection = this.transform.position - playerFollowCamera.position;
            direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
            direction = Quaternion.AngleAxis(135 + offsetAngle, Vector3.up) * direction;
        }
        else if (v < -0.01f && h < -0.01f)
        {
            rawDirection = this.transform.position - playerFollowCamera.position;
            direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
            direction = Quaternion.AngleAxis(-135 - offsetAngle, Vector3.up) * direction;
        }
        else
        {
            if (v > 0.01f)
            {
                rawDirection = this.transform.position - playerFollowCamera.position;
                direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
                direction = Quaternion.AngleAxis(offsetAngle, Vector3.up) * direction;
            }
            else if (v < -0.01f)
            {
                rawDirection = this.transform.position - playerFollowCamera.position;
                direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
                direction = Quaternion.AngleAxis(180 + offsetAngle, Vector3.up) * direction;
            }
            else if (h > 0.01f)
            {
                rawDirection = this.transform.position - playerFollowCamera.position;
                direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
                direction = Quaternion.AngleAxis(90 + offsetAngle, Vector3.up) * direction;
            }
            else if (h < -0.01f)
            {
                rawDirection = this.transform.position - playerFollowCamera.position;
                direction = new Vector3(rawDirection.x, 0, rawDirection.z).normalized;
                direction = Quaternion.AngleAxis(-90 - offsetAngle, Vector3.up) * direction;
            }
        }

        if (Mathf.Abs(v) + Mathf.Abs(h) > 0.01f)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }

            this.rb.velocity = this.rb.velocity.y * Vector3.up + direction * (this.character.speed + 9.81f) / 100f;

            //将方向转换为四元数
            Quaternion quaDir = Quaternion.LookRotation(direction, Vector3.up);
            //平滑转动到目标方向
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, quaDir, Time.fixedDeltaTime * 10);

            //float angle = Vector3.Angle(character.direction, direction);
            //Vector3 normal = Vector3.Cross(character.direction, direction);//叉乘求出法线向量
            //angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up));  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
            //this.transform.rotation = Quaternion.Euler(0, angle, 0);

            character.SetDirection(GameObjectTool.WorldToLogic(this.transform.rotation.eulerAngles));
            //rb.transform.forward = this.transform.rotation.eulerAngles;
        }
        else
        {
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }
       
        //Debug.LogFormat("velocity {0}", this.rb.velocity.magnitude);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }

        if (Input.GetButtonDown("Run"))
        {
            //this.SendEntityEvent(EntityEvent.MoveFwd);
            transform.GetChild(0).GetComponent<Animator>().SetBool("Run", true);
        }

        if (Input.GetButtonUp("Run"))
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Run", false);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.visible = ! Cursor.visible;
        }
            
    }

    Vector3 lastPos;
    float lastSync = 0;
    private void LateUpdate()
    {
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 50)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;
    }


    void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);
    }
}
