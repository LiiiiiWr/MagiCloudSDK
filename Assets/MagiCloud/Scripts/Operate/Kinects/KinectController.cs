﻿using UnityEngine;
using MagiCloud.Core;
using MagiCloud.Core.Events;
using MagiCloud.Kinect;
using MagiCloud.Core.MInput;
using MagiCloud.Features;
using System.Collections.Generic;
using System;

namespace MagiCloud.Operate
{
    /// <summary>
    /// Kinect控制端
    /// 1、支持单双手操作
    /// 2、同时兼容鼠标操作
    ///     1）当识别到人物手时，鼠标禁止
    ///     2）当未识别到人的手时，鼠标开启
    /// 
    /// 思路：
    /// 1、当手势未识别到手时（双手没有一只手激活时，鼠标控制端启动）
    /// 2、当手势识别时，开启手势端。
    /// 3、当手势端有一个未开启的时候，需要禁用相应的操作，但是事件是否需要考虑待定。
    /// 4、比如握拳时，消失。在显示时，这时候需要做什么处理。
    /// 5、单双手的缩放，也需要优化。
    /// 
    /// </summary>
    public class KinectController : MonoBehaviour, IHandController
    {
        private bool isEnable = false;

        public bool IsEnable
        {
            get {
                return isEnable;
            }
            set {

                if (isEnable == value) return;
                isEnable = value;

                if(isEnable)
                {
                    behaviour = new MBehaviour(ExecutionPriority.Highest, -900);
                    behaviour.OnUpdate_MBehaviour(OnKinectUpdate);

                    //注册手势启动/停止事件
                    EventHandStart.AddListener(HandStart);
                    EventHandStop.AddListener(HandStop);
                }
                else
                {
                    behaviour.OnExcuteDestroy();

                    //移除手势启动/停止事件
                    EventHandStart.RemoveListener(HandStart);
                    EventHandStop.RemoveListener(HandStop);
                }

            }
        }

        /// <summary>
        /// 手操作相关数据
        /// </summary>
        [Serializable]
        public class HandOperate
        {
            /// <summary>
            /// 手图标
            /// </summary>
            public HandIcon handIcon;
            /// <summary>
            /// 手势操作端
            /// </summary>
            /// <value>The operate.</value>
            public MOperate Operate { get; set; }
            /// <summary>
            /// 针对手对物体的操作
            /// </summary>
            /// <value>The operate object.</value>
            public IOperateObject OperateObject { get; set; }
            /// <summary>
            /// 针对手与物体抓取时的偏移量
            /// </summary>
            /// <value>The offset.</value>
            public Vector3 Offset { get; set; }
            /// <summary>
            /// 计算最后一帧时的手坐标
            /// </summary>
            /// <value>The last hand position.</value>
            public Vector3 LastHandPos { get; set; }

            /// <summary>
            /// 手编号
            /// </summary>
            /// <value>The index of the hand.</value>
            public int HandIndex;

            /// <summary>
            /// 绑定相关抓取事件
            /// </summary>
            public void BindGrab()
            {
                Operate.OnGrab = OnGrabObject;
                Operate.OnSetGrab = SetGrabObject;
                Operate.OnEnable();
            }

            /// <summary>
            /// 抓取设置
            /// </summary>
            /// <param name="operate">Operate.</param>
            /// <param name="handIndex">Hand index.</param>
            public void OnGrabObject(IOperateObject operate,int handIndex)
            {
                if (HandIndex != handIndex) return;

                Offset = MUtility.GetOffsetPosition(Operate.InputHand.ScreenPoint, operate.GrabObject);
                OperateObject = operate;
            }


            /// <summary>
            /// 设置物体被抓取
            /// </summary>
            /// <param name="operate">Operate.</param>
            /// <param name="handIndex">Hand index.</param>
            public void SetGrabObject(IOperateObject operate,int handIndex,float cameraRelativeDistance)
            {
                if (HandIndex != handIndex) return;

                Vector3 screenPoint = Operate.InputHand.ScreenPoint;
                OperateObject = operate;

                Vector3 screenMainCamera = MUtility.MainWorldToScreenPoint(MUtility.MainCamera.transform.position
                + MUtility.MainCamera.transform.forward * cameraRelativeDistance);

                Vector3 position = MUtility.MainScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, screenMainCamera.z));
                Offset = Vector3.zero;

                OperateObject.GrabObject.transform.position = position;
            }

            /// <summary>
            /// 针对OperateObject的处理
            /// </summary>
            public void OnOperateObjectHandle()
            { 
                switch(Operate.InputHand.HandStatus)
                {
                    case MInputHandStatus.Grabing:

                        var screenDevice = MUtility.MainWorldToScreenPoint(OperateObject.GrabObject.transform.position);

                        var screenMouse = Operate.InputHand.ScreenPoint;
                        Vector3 vpos = MUtility.MainScreenToWorldPoint(new Vector3(screenMouse.x, screenMouse.y, screenDevice.z));

                        Vector3 position = vpos - Offset;

                        EventUpdateObject.SendListener(OperateObject.GrabObject, position, OperateObject.GrabObject.transform.rotation, HandIndex);

                        break;
                    case MInputHandStatus.Idle:

                        OperateObject = null;

                        break;
                }
            }

            /// <summary>
            /// 是否支持旋转
            /// </summary>
            /// <returns><c>true</c>, if rotate was ised, <c>false</c> otherwise.</returns>
            /// <param name="handOperate">Hand operate.</param>
            public bool IsRotate(HandOperate handOperate)
            {
                return Operate.InputHand.HandStatus == MInputHandStatus.Grip &&
                    handOperate.Operate.InputHand.HandStatus == MInputHandStatus.Idle;
            }

            public bool IsZoom(HandOperate handOperate)
            {
                return Operate.InputHand.HandStatus == MInputHandStatus.Grip &&
                        handOperate.Operate.InputHand.HandStatus == MInputHandStatus.Grip;
            }
        }

        /// <summary>
        /// 缩放操作数据
        /// </summary>
        public class ZoomOperate
        {
            private float distance;
            private float lastDistance;
            private Vector3 startPos;

            public ZoomOperate()
            {
                OnHandIdle();
            }

            public void OnHandGrip()
            {
                distance = Vector3.Distance(MInputKinect.ScreenHandPostion(0), MInputKinect.ScreenHandPostion(1));
                lastDistance = distance;
            }

            public void OnHandIdle()
            {
                distance = 0;
                lastDistance = 0;
            }

            public float ZoomCameraToMoveFloat()
            {
                float moveDistance = 0;
                lastDistance = Vector3.Distance(MInputKinect.ScreenHandPostion(0), MInputKinect.ScreenHandPostion(1));

                moveDistance = lastDistance - distance;
                distance = lastDistance;

                return moveDistance;
            }
        }

        public Vector2 handSize = new Vector2(50, 50);

        public HandOperate rightHandOperate = new HandOperate() { HandIndex = 0 };
        public HandOperate leftHandOperate = new HandOperate() { HandIndex = 1 };

        public Dictionary<int, MInputHand> InputHands { get; set; }
        public bool IsPlaying { get; private set; }

        private MBehaviour behaviour;
        private ZoomOperate zoomOperate = new ZoomOperate();//缩放数据操作

        [SerializeField]
        private KinectHandModel handModel = KinectHandModel.Two;

        private MouseController mouseController; //鼠标控制端

        public MInputHand GetInputHand(int handIndex)
        {
            MInputHand hand;

            InputHands.TryGetValue(handIndex, out hand);

            if (hand == null)
                throw new Exception("手势编号错误：" + handIndex);

            return hand;
        }

        /// <summary>
        /// Kinect控制端数据初始化
        /// </summary>
        /// <param name="handModel">Hand model.</param>
        private void KinectInitialize(KinectHandModel handModel)
        {
            InputHands = new Dictionary<int, MInputHand>();
            isEnable = true;

            MInputKinect inputKinect = gameObject.GetComponent<MInputKinect>() ?? gameObject.AddComponent<MInputKinect>();

            MInputKinect.HandModel = handModel;
            IsPlaying = true;

            //实例化右手
            var rightHandUI = MHandUIManager.CreateHandUI(transform, rightHandOperate.handIcon);
            var rightInputHand = new MInputHand(rightHandOperate.HandIndex, rightHandUI, OperatePlatform.Kinect);
            InputHands.Add(rightHandOperate.HandIndex, rightInputHand);

            //实例化左手
            var leftHandUI = MHandUIManager.CreateHandUI(transform, leftHandOperate.handIcon);
            var leftInputHand = new MInputHand(leftHandOperate.HandIndex, leftHandUI, OperatePlatform.Kinect);
            InputHands.Add(leftHandOperate.HandIndex, leftInputHand);

            //右手操作端相关初始化与事件绑定
            rightHandOperate.Operate = MOperateManager.AddOperateHand(rightInputHand, this);
            rightHandOperate.BindGrab();

            //左手操作端相关初始化与事件的绑定
            leftHandOperate.Operate = MOperateManager.AddOperateHand(leftInputHand, this);
            leftHandOperate.BindGrab();

            mouseController = gameObject.GetComponent<MouseController>() ?? gameObject.AddComponent<MouseController>();
            mouseController.IsEnable = false;
        }

        /// <summary>
        /// 手停止
        /// </summary>
        /// <param name="handIndex">Hand index.</param>
        private void HandStop(int handIndex)
        {
            if (handIndex == 0)
            {
                rightHandOperate.Operate.OnDisable();
            }
            else
            {
                leftHandOperate.Operate.OnDisable();
            }

            ChangePlatform();
        }

        /// <summary>
        /// 变更平台
        /// </summary>
        void ChangePlatform()
        {
            if(!MInputKinect.IsHandActive(2))
            {
                mouseController.IsEnable = true;
                MUtility.CurrentPlatform = OperatePlatform.Mouse;
            }
            else
            {
                mouseController.IsEnable = false;
                MUtility.CurrentPlatform = OperatePlatform.Kinect;
            }
        }

        /// <summary>
        /// 手启动
        /// </summary>
        /// <param name="handIndex">Hand index.</param>
        private void HandStart(int handIndex)
        {
            if (handIndex == 0)
            {
                rightHandOperate.Operate.OnEnable();
            }
            else
            {
                leftHandOperate.Operate.OnEnable();
            }

            //根据手势的启用情况，设置他的状态
            ChangePlatform();
        }

        void Awake()
        {
            KinectInitialize(handModel);
            IsEnable = true;
        }

        void OnKinectUpdate()
        {
            if (!isEnable) return;

            SetHandStatus(0); //右手
            SetHandStatus(1); //左手

            rightHandOperate.OnOperateObjectHandle();
            leftHandOperate.OnOperateObjectHandle();

            OnRotate();
            OnZoom();

        }

        /// <summary>
        /// 设置手的状态
        /// </summary>
        /// <param name="handIndex">Hand index.</param>
        private void SetHandStatus(int handIndex)
        {

            if (MInputKinect.IsHandActive(handIndex))
            {
                //发送抓取事件等相关事件
                InputHands[handIndex].OnUpdate(MInputKinect.ScreenHandPostion(handIndex));

                if (MInputKinect.HandGrip(handIndex))
                {
                    InputHands[handIndex].SetGrip();
                }

                if (MInputKinect.HandRelease(handIndex))
                {
                    InputHands[handIndex].SetIdle();
                }

                if (MInputKinect.HandLasso(handIndex))
                {
                    InputHands[handIndex].SetLasso();
                }
            }
        }

        /// <summary>
        /// 旋转
        /// </summary>
        public void OnRotate()
        {
            //当两只手都处于空闲状态时，才支持旋转
            if (rightHandOperate.IsRotate(leftHandOperate) && InputHands[0].ScreenVector.magnitude > 2)
            {
                InputHands[0].HandStatus = MInputHandStatus.Rotate;
            }

            if(InputHands[0].IsRotateStatus)
            {
                EventCameraRotate.SendListener(InputHands[0].ScreenVector);
            }

            if (leftHandOperate.IsRotate(rightHandOperate) && InputHands[1].ScreenVector.magnitude > 2)
            {
                InputHands[1].HandStatus = MInputHandStatus.Rotate;
            }

            if (InputHands[1].IsRotateStatus)
            {
                EventCameraRotate.SendListener(InputHands[1].ScreenVector);
            }
        }

        #region 缩放

        /// <summary>
        /// 是否支持缩放
        /// </summary>
        /// <returns><c>true</c>, if zoom was ised, <c>false</c> otherwise.</returns>
        private bool IsZoom()
        {
            return InputHands[0].HandStatus == MInputHandStatus.Grip &&
                InputHands[1].HandStatus == MInputHandStatus.Grip || 
                InputHands[0].IsZoomStatus && InputHands[1].IsZoomStatus;
        }

        private bool IsZooming()
        {
            return
                InputHands[0].IsZoomStatus && InputHands[1].IsZoomStatus;
        }

        /// <summary>
        /// 缩放
        /// </summary>
        private void OnZoom()
        { 
            if(IsZoom())
            { 
                if(!IsZooming())
                {
                    InputHands[0].HandStatus = MInputHandStatus.Zoom;
                    InputHands[1].HandStatus = MInputHandStatus.Zoom;

                    zoomOperate.OnHandGrip();
                }

                float zoomValue = zoomOperate.ZoomCameraToMoveFloat() / 10000;
                if(IsZooming())
                {
                    EventCameraZoom.SendListener(zoomValue);
                }
            }
            else
            {
                if(IsZooming())
                {
                    zoomOperate.OnHandIdle();
                    EventCameraZoom.SendListener(0);

                    InputHands[0].HandStatus = MInputHandStatus.Idle;
                    InputHands[1].HandStatus = MInputHandStatus.Idle;

                }
            }
        }

        #endregion

        /// <summary>
        /// 切换多手
        /// </summary>
        public void StartMultipleHand()
        {
            MInputKinect.HandModel = KinectHandModel.Two;
        }

        /// <summary>
        /// 切换单手
        /// </summary>
        public void StartOnlyHand()
        {
            MInputKinect.HandModel = KinectHandModel.One;
        }
    }
}

