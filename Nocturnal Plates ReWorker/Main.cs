using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ABI_RC.Core.Player;
using System.Reflection;
using ABI_RC.Core;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BepInEx;
namespace Nocturnal
{
    [BepInPlugin("org.bepinex.plugins.Nocturnal.Plates", "Nocturnal CVR Plates Reworker", "1.1.0.0")]
    public class Main : BaseUnityPlugin
    {
        private static Harmony Instance  = new Harmony(Guid.NewGuid().ToString());
        internal static Config s_config { get; set; }
        private void Awake()
        {
            s_config = new Config();
            HPatch();
            StartCoroutine(WaitForUi());
        }

        private IEnumerator WaitForUi()
        {
            while (RootLogic.Instance == null) yield return new WaitForSeconds(1f);

            RootLogic.Instance.comms.OnPlayerStartedSpeaking += PlayerTalking;
            RootLogic.Instance.comms.OnPlayerStoppedSpeaking += PlayerStopTalking;
            yield break;
        }

        private NamePlateHandler _handler { get; set; }

        private void PlayerStopTalking(Dissonance.VoicePlayerState obj)
        {      
            _handler = GameObject.Find($"/{obj.Name}").transform.Find("[NamePlate]").gameObject.GetComponent<NamePlateHandler>();
            _handler.BackgroundMask.color = new Color(_handler.UserColor.r, _handler.UserColor.g, _handler.UserColor.b, 0.4f);
            _handler.BackgroundImageComp.color = new Color(_handler.UserColor.r, _handler.UserColor.g, _handler.UserColor.b, 0.4f);
            _handler.MicOn.SetActive(false);
            _handler.MicOff.SetActive(true);
        }
        private void PlayerTalking(Dissonance.VoicePlayerState obj)
        {
           
            if (obj.Amplitude > 0.1f) return;
            _handler = GameObject.Find($"/{obj.Name}").transform.Find("[NamePlate]").gameObject.GetComponent<NamePlateHandler>();
            _handler.BackgroundMask.color = new Color(_handler.UserColor.r, _handler.UserColor.g, _handler.UserColor.b, 1f);
            _handler.BackgroundImageComp.color = new Color(_handler.UserColor.r, _handler.UserColor.g, _handler.UserColor.b, 1f);
            _handler.MicOn.SetActive(true);
            _handler.MicOff.SetActive(false);
        }

        private static void HPatch() =>
            Instance.Patch(typeof(PlayerNameplate).GetMethod(nameof(PlayerNameplate.UpdateNamePlate)),null,new HarmonyMethod(typeof(Main).GetMethod(nameof(PostFix), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)));
        


        private static void PostFix(PlayerNameplate __instance) =>       
            __instance.gameObject.AddComponent<NamePlateHandler>();
        
    }
}
