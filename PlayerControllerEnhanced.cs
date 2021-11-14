using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerControllerEnhanced : MonoBehaviour {
    
    public float speed;
    Transform MainCamera;
    public float rotSpeed = 10f;

    private Dialog Dialog;
    public DialogManager DialogManager;
    public GameObject ActionKey;
    public GameObject ChoiceButtons;
    public Animator Animator;
    private StoryManager StoryManager;
    public Transform InitPos;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(){
        InitPos = GameObject.Find("InitPos").GetComponent<Transform>();
        this.transform.position = InitPos.position;

        MainCamera = GameObject.Find("MainCamera").transform;
        Vector3 CamPos = new Vector3(MainCamera.position.x, 0f, MainCamera.position.z);
        transform.LookAt(CamPos);
        StoryManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
    }

    private void Update() {

        if(DialogManager.isDisplayed()){
            Cursor.visible = false;
            return;
        }
        else if(ChoiceButtons.activeSelf){Cursor.visible = true; return;}
        else if(!DialogManager.isDisplayed()){Cursor.visible = true;}

        PlayerMovement();
        Interraction();
    }

    void PlayerMovement(){
        float horAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");

        Animator.SetFloat("Speed", Mathf.Abs(horAxis));
        Vector3 playerMovement = new Vector3(0f, 0f, horAxis) * speed * Time.deltaTime;
        transform.Rotate(Vector3.up, verAxis * Time.deltaTime * rotSpeed * 10, Space.Self);
        transform.Translate(playerMovement, Space.Self);
    }

    Collider Target;

    private void OnTriggerEnter(Collider obj) {
        if(obj.tag == "DialogArea"){
            Dialog = Target.GetComponent<Dialog>();

            Vector3 PlayerPos = new Vector3(this.transform.position.x, this.transform.position.y + 7f, this.transform.position.z);
            Vector3 CamPos = new Vector3(MainCamera.position.x, 0f, MainCamera.position.z);
            ActionKey.SetActive(true);
            ActionKey.transform.position = PlayerPos;
            ActionKey.transform.LookAt(ActionKey.transform.position - CamPos);
        }
        if(obj.tag == "Trigger"){
            Target = obj;

            Vector3 PlayerPos = new Vector3(this.transform.position.x, this.transform.position.y + 7f, this.transform.position.z);
            Vector3 CamPos = new Vector3(MainCamera.position.x, 0f, MainCamera.position.z);
            ActionKey.SetActive(true);
            ActionKey.transform.position = PlayerPos;
            ActionKey.transform.LookAt(ActionKey.transform.position - CamPos);
        }
    }

     private void OnTriggerExit(Collider obj) {
        if(obj.tag == "DialogArea"){
            Dialog = null;
            
            ActionKey.SetActive(false);
        }
    }

    private void Interraction(){
        if(Input.GetKeyDown("e")){
            if(Dialog != null){
                DialogManager.DisplayDialog(Dialog.GetDialog());
                Dialog = null;
                ActionKey.SetActive(false);
                
            }
            if(Dialog == null){
                StoryManager.validChoice();
                Target.gameObject.SetActive(false);
                ActionKey.SetActive(false);
            }
        }
    }
}