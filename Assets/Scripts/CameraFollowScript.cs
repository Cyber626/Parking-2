using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject followObject;
    [SerializeField] private Vector2 followObjectLocalOffset;
    [SerializeField] private bool doFollowRotation;


    private void FixedUpdate()
    {
        Vector2 relativeOffset = followObject.transform.rotation * followObjectLocalOffset;
        transform.SetPositionAndRotation(new Vector3(followObject.transform.position.x + relativeOffset.x, followObject.transform.position.y + relativeOffset.y, -10.0f), followObject.transform.rotation);
    }
}


//public class MyScript : MonoBehaviour
//{
//    public bool flag;
//    public int i = 1;
//}

//[CustomEditor(typeof(MyScript))]
//public class MyScriptEditor : Editor
//{
//    void OnInspectorGUI()
//    {
//        var myScript = target as MyScript;

//        myScript.flag = GUILayout.Toggle(myScript.flag, "Flag");

//        if (myScript.flag)
//            myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);

//    }
//}