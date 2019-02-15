﻿using System;
using UnityEngine;
using System.Collections.Generic;
using MagiCloud.Operate;
using MagiCloud.Core.Events;

namespace MagiCloud.KGUI
{
    public class PanelRange
    {
        public int handIndex;
        public bool IsRangeUI;
    }

    /// <summary>
    /// Button物体
    /// </summary>
    public class KGUI_ButtonObject : KGUI_ButtonBase
    {
        public GameObject bindObject;//绑定的物体

        [Header("当值为-1时为无穷。当值为0时，则自动禁用")]
        public int maxCount = -1;

        protected List<GameObject> targetObjects = new List<GameObject>();//*****************************

        protected List<PanelRange> ranges = new List<PanelRange>();//******************************

        public KGUI_Panel panel;

        public float zValue = 5f;

        private int tempCount = 0;


        protected override void Awake()
        {
            base.Awake();
            tempCount = maxCount;
        }

        public override void OnDown(int handIndex)
        {
            base.OnDown(handIndex);

            if (tempCount == 0) return;

            var targetObject = Instantiate(bindObject) as GameObject;
            var frontUI = targetObject.GetComponent<KGUI_ObjectFrontUI>() ?? targetObject.AddComponent<KGUI_ObjectFrontUI>();
            frontUI.OnSet();

            MOperateManager.SetObjectGrab(targetObject, handIndex, zValue);
            //KinectTransfer.SetObjectGrab(targetObject, zValue, handIndex: handIndex);

            targetObjects.Add(targetObject);

            if (tempCount != -1)
                tempCount--;

            if (tempCount == 0)
            {
                IsEnable = false;
            }
        }

        public override void OnEnter(int handIndex)
        {
            if (IsStartAudio && audioSource != null)
            {
                audioSource.Play();
            }

            base.OnEnter(handIndex);
        }

        void Register()
        {
            //Events.EventHandReleaseObject.AddListener(Events.EventLevel.B, OnReleaseObject);
            //MCKinect.Events.KinectEventHandReleaseObject.AddListener(MCKinect.Events.EventLevel.B, OnReleaseObject);
            //Events.EventHandGrabObject.AddListener(Events.EventLevel.B, OnGrabObject);

            EventHandReleaseObject.AddListener(OnReleaseObject);
            EventHandGrabObject.AddListener(OnGrabObject);

            if (panel != null)
            {
                panel.onEnter.AddListener(OnPanelEnter);
                panel.onExit.AddListener(OnPanelExit);
            }
        }

        void Logout()
        {
            EventHandReleaseObject.RemoveListener(OnReleaseObject);
            EventHandGrabObject.RemoveListener(OnGrabObject);


            //Events.EventHandReleaseObject.RemoveListener(OnReleaseObject);
            //MCKinect.Events.KinectEventHandReleaseObject.RemoveListener(OnReleaseObject);

            //Events.EventHandGrabObject.RemoveListener(OnGrabObject);

            if (panel != null)
            {
                panel.onEnter.RemoveListener(OnPanelEnter);
                panel.onExit.RemoveListener(OnPanelExit);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Register();
        }

        protected virtual void OnPanelEnter(int handIndex)
        {
            PanelRange panelRange = ranges.Find(obj => obj.handIndex.Equals(handIndex));

            if (panelRange == null)
            {
                panelRange = new PanelRange() { handIndex = handIndex, IsRangeUI = true };
                ranges.Add(panelRange);
            }
            else
            {
                panelRange.IsRangeUI = true;
            }

        }

        protected virtual void OnPanelExit(int handIndex)
        {
            PanelRange panelRange = ranges.Find(obj => obj.handIndex.Equals(handIndex));
            if (panelRange != null)
            {
                panelRange.IsRangeUI = false;
            }
        }

        protected virtual void OnGrabObject(GameObject target, int handIndex)
        {
            if (!targetObjects.Contains(target)) return;

            target.GetComponent<KGUI_ObjectFrontUI>().OnSet();
        }

        protected virtual void OnReleaseObject(GameObject target, int handIndex)
        {
            //释放时，发送一次事件?

            if (!targetObjects.Contains(target))
                return;

            PanelRange panelRange = ranges.Find(obj => obj.handIndex.Equals(handIndex));

            if (panelRange == null)
                return;

            if (panelRange.IsRangeUI)
            {
                targetObjects.Remove(target);

                Destroy(target);

                if (tempCount != -1 && tempCount <= maxCount)
                {
                    tempCount++;
                    IsEnable = true;
                }
            }
            else
            {
                target.GetComponent<KGUI_ObjectFrontUI>().OnReset();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Logout();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Logout();
        }
    }
}
