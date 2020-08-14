using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class jointFollowAnim : MonoBehaviour
//{
//    [SerializeField] private HingeJoint2D hj;
//    [SerializeField] private Transform target;
//    [SerializeField] private bool invert;
//
//    void Update()
//    {
//        if (hj != null)
//        {
//                JointSpring js;
//                js = hj.spring;
//                js.targetPosition = target.transform.localEulerAngles.z;
//                if (js.targetPosition > 180)
//                    js.targetPosition = js.targetPosition - 360;
//                if (invert)
//                    js.targetPosition = js.targetPosition * -1;
//
//                js.targetPosition = Mathf.Clamp(js.targetPosition, hj.limits.min + 5, hj.limits.max - 5);
//
//                hj.spring = js;
//        }
//    }
//}
