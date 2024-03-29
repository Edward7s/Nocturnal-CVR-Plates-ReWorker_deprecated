﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ABI_RC.Core.Networking.IO.Social;
namespace Nocturnal
{
    internal class NamePlateHandler : MonoBehaviour, IDisposable
    {
        private GameObject _maskGameObject { get; set; }
        private UnityEngine.UI.Image _maskImageComp { get; set; }
        private GameObject _backgroundGameObject { get; set; }
        public UnityEngine.UI.Image BackgroundImageComp { get; set; }
        public UnityEngine.UI.Image BackgroundMask { get; set; }
        private GameObject _backgroundGameObj{ get; set; }
        private GameObject _freindIcon { get; set; }
        public GameObject MicOn { get; set; }
        public GameObject MicOff { get; set; }
        public Color UserColor { get; set; }
        private Color UserColorMaxAlpha { get; set; }

        private UnityEngine.UI.Image _micOffImage{ get; set; }
        private UnityEngine.UI.Image _micOnImage { get; set; }
        private UnityEngine.UI.Image _friend { get; set; }
        
        private Transform _canvas { get; set; }

        void Start() => InvokeRepeating(nameof(Setup), -1, 0.3f);
         private void Setup()
        {
            try
            {
                if (this.transform.Find("Canvas/Content/Image/ObjectMaskSlave/UserImageMask (1)") == null) return;
            }
            catch { return; }

            _canvas = this.transform.Find("Canvas").transform;
            _canvas.localScale = new Vector3(0.45f, 0.45f, 1);
            if (ABI_RC.Core.InteractionSystem.ViewManager.Instance.FriendList.FirstOrDefault(x => x.UserId == this.transform.parent.gameObject.name) != null)
                UserColor = Config.FriendsColor;

            else
            {
                switch (this.transform.parent.gameObject.GetComponent<ABI_RC.Core.Player.PlayerDescriptor>().userRank)
                {
                    case "Legend":
                        UserColor = Config.Legend;
                        break;
                    case "Community Guide":
                        UserColor = Config.Guide;
                        break;
                    case "Moderator":
                        UserColor = Config.Mod;
                        break;
                    case "Developer":
                        UserColor = Config.Dev;
                        break;
                    default:
                        UserColor = Config.DefaultColor;
                        break;
                }
            }
            UserColor = new Color(UserColor.r, UserColor.g, UserColor.b, 0.4f);
            UserColorMaxAlpha = new Color(UserColor.r, UserColor.g, UserColor.b, 1);
            _backgroundGameObject = this.transform.Find("Canvas/Content/Image").gameObject;
            Component.DestroyImmediate(_backgroundGameObject.GetComponent<UnityEngine.UI.Image>());
            BackgroundImageComp = _backgroundGameObject.AddComponent<UnityEngine.UI.Image>();
            BackgroundImageComp.type = UnityEngine.UI.Image.Type.Sliced;
            BackgroundImageComp.pixelsPerUnitMultiplier = 500;
            BackgroundImageComp.color = UserColor;
            BackgroundImageComp.ChangeSpriteFromString(Config.Instance.Js.Background, 200, new Vector4(255, 0, 255, 0));
            _maskGameObject = this.transform.Find("Canvas/Content/Image/ObjectMaskSlave/UserImageMask").gameObject;
            Component.DestroyImmediate(_maskGameObject.GetComponent<UnityEngine.UI.Image>());
            _maskImageComp = _maskGameObject.AddComponent<UnityEngine.UI.Image>();
            _maskImageComp.ChangeSpriteFromString(Config.Instance.Js.Icon);
            _maskGameObject.transform.localScale = new Vector3(1.25f, 1.1f, 1);
            Component.DestroyImmediate(_maskGameObject.GetComponent<UnityEngine.UI.Mask>());
            _maskGameObject.AddComponent<UnityEngine.UI.Mask>();
            _backgroundGameObj = this.transform.Find("Canvas/Content/Image/ObjectMaskSlave/UserImageMask (1)").gameObject;
            Component.DestroyImmediate(_backgroundGameObj.GetComponent<UnityEngine.UI.Image>());
            BackgroundMask = _backgroundGameObj.AddComponent<UnityEngine.UI.Image>();
            BackgroundMask.color = UserColor;
            BackgroundMask.ChangeSpriteFromString(Config.Instance.Js.Icon);
            BackgroundMask.transform.SetSiblingIndex(0);
            BackgroundMask.transform.localScale = new Vector3(1.45f, 1.25f, 1);
            _maskGameObject.transform.Find("UserImage").transform.localScale = new Vector3(1.05f, 1.05f, 1);
            _freindIcon = _backgroundGameObject.transform.Find("FriendsIndicator").gameObject;
            _freindIcon.transform.localScale = new Vector3(0.9f, 0.6f, 1);
            _freindIcon.transform.localPosition = new Vector3(0.60f, 0.39f, 0);
            _friend = _freindIcon.GetComponent<UnityEngine.UI.Image>();
            _friend.ChangeSpriteFromString(Config.Instance.Js.Friend).color = UserColorMaxAlpha;
            _friend.enabled = false;
            MicOff = GameObject.Instantiate(_freindIcon, _freindIcon.transform.parent.transform);
            _micOffImage = MicOff.GetComponent<UnityEngine.UI.Image>();
            _micOffImage.ChangeSpriteFromString(Config.Instance.Js.MicIconOff).color = UserColor;
            _micOffImage.enabled = true;
            MicOff.transform.localPosition = new Vector3(0.944f, 0.39f, 0);
            MicOn = GameObject.Instantiate(MicOff, _freindIcon.transform.parent.transform);
            MicOn.GetComponent<UnityEngine.UI.Image>().ChangeSpriteFromString(Config.Instance.Js.MicIconOn).color = UserColor;
            MicOn.transform.localPosition = new Vector3(0.944f, 0.39f, 0);
            MicOn.gameObject.SetActive(false);
            this.transform.Find("Canvas/Content/Image/Image").gameObject.GetComponent<UnityEngine.UI.Image>().color = UserColorMaxAlpha;
            if (UserColor == new Color(Config.FriendsColor.r, Config.FriendsColor.g, Config.FriendsColor.b, 0.4f)) _friend.enabled = true;
            CancelInvoke(nameof(Setup));
            Dispose();
            if (Config.Instance.Js.DistanceScale) 
                InvokeRepeating(nameof(GetDistance), -1, 0.1f);
        }

        private float _distance { get; set; }
        private float _distance2 { get; set; }

        private void GetDistance()
        {
            _distance = 0.1f + Vector3.Distance(this.transform.parent.transform.position, Main.LocalPlayerTransform.position) / 9;
            _distance2 = _distance > 0.9f ? 0.9f : _distance;
            _canvas.localScale = new Vector3(_distance2, _distance2, 1);
        }


        public void Dispose()
        {
            _friend = null;
            _micOnImage = null;
            _micOffImage = null;
            _maskGameObject = null;
            _maskImageComp = null;
            _backgroundGameObject = null;
            _backgroundGameObj = null;
            _freindIcon = null;
        }
    }
}
