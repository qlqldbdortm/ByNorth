using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ByNorth.Core.GameFlow;
using ByNorth.SlotSystem;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ByNorth.UI.InGame
{
    public class NextStageTimer : MonoBehaviour
    {
        public float stageDelay = 5f;       // 타이머 시간
        public int requireSupplyItem = 10;
        private CancellationTokenSource token = null;


        private void OnDestroy()
        {
            token?.Cancel();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (SlotManager.Instance.ReturnSupplyItemCount() >= requireSupplyItem)
                {
                    token?.Cancel();
                    token = new();
                    Timer(token.Token).Forget();
                }
                else
                {
                    InGameUIManager.Instance.timerText.text = "아이템이 부족합니다.";
                }
                InGameUIManager.Instance.timerText.gameObject.SetActive(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (SlotManager.Instance.ReturnSupplyItemCount() >= requireSupplyItem)
                {
                    token?.Cancel();
                    InGameUIManager.Instance.timerText.text = "0.0";
                }
                InGameUIManager.Instance.timerText.gameObject.SetActive(false);
            }
        }

        public async UniTask Timer(CancellationToken cancellationToken)
        {
            float timer = stageDelay;
            while (timer > 0f)
            {
                cancellationToken.ThrowIfCancellationRequested();

                timer -= Time.deltaTime; 
                if (timer < 0f) timer = 0f;

                InGameUIManager.Instance.timerText.text = timer.ToString("F1");

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
            
            Debug.Log("다음 스테이지로 이동");
            InGameFlowManager.Instance.CreateVillage();
            SlotManager.Instance.DeleteSupplyItem();
            InGameUIManager.Instance.timerText.text = "0.0";
            InGameUIManager.Instance.timerText.gameObject.SetActive(false);
        }
    }
}
