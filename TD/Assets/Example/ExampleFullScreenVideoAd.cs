using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using ByteDance.Union.Mediation;
using UnityEngine;

/**
 * 全屏视频代码示例
 * 注：仅支持穿山甲，不支持融合。
 */
public class ExampleFullScreenVideoAd
{
    public static void LoadFullScreenVideoAd(Example example)
    {
        if (example.fullScreenVideoAd != null)
        {
            example.fullScreenVideoAd.Dispose();
            example.fullScreenVideoAd = null;
        }
        var adSlot = new AdSlot.Builder()
            .SetCodeId(Example.useMediation ? CSJMDAdPositionId.M_INTERSTITAL_FULL_SCREEN_ID :
                CSJMDAdPositionId.CSJ_ExpressFullScreen_V_ID) // 必传
            .SetOrientation(AdOrientation.Vertical)
            .SetMediationAdSlot(new MediationAdSlot.Builder()
                .SetScenarioId("ScenarioId") // 可选
                .SetUseSurfaceView(false) // 可选
                .SetBidNotify(true) // 可选
                .Build())
            .Build();
        SDK.CreateAdNative().LoadFullScreenVideoAd(adSlot, new FullScreenVideoAdListener(example));
    }

    public static void ShowFullScreenVideoAd(Example example)
    {
        if (example.fullScreenVideoAd == null)
        {
            Debug.LogError("CSJM_Unity "+"请先加载广告");
            example.information.text = "请先加载广告";
            return;
        }

        example.fullScreenVideoAd.SetFullScreenVideoAdInteractionListener(new FullScreenAdInteractionListener(example));
        example.fullScreenVideoAd.SetDownloadListener(new AppDownloadListener(example));

        example.fullScreenVideoAd.ShowFullScreenVideoAd();
    }

    // 广告加载监听器
    public sealed class FullScreenVideoAdListener : IFullScreenVideoAdListener
    {
        private Example example;

        public FullScreenVideoAdListener(Example example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity "+$"OnFullScreenError: {message}  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnFullScreenError: " + message;
        }

        public void OnFullScreenVideoAdLoad(FullScreenVideoAd ad)
        {
            Debug.Log("CSJM_Unity "+$"OnFullScreenAdLoad  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnFullScreenAdLoad";

            this.example.fullScreenVideoAd = ad;
        }

        public void OnFullScreenVideoCached()
        {
            Debug.Log("CSJM_Unity "+$"OnFullScreenVideoCached  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnFullScreenVideoCached";
        }

        public void OnFullScreenVideoCached(FullScreenVideoAd ad)
        {
        }
    }

    // 广告展示监听器
    public sealed class FullScreenAdInteractionListener : IFullScreenVideoAdInteractionListener
    {
        private Example example;

        public FullScreenAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + $"fullScreenVideoAd show  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "fullScreenVideoAd show";
            }
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("CSJM_Unity " + $"fullScreenVideoAd bar click  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "fullScreenVideoAd bar click";
            }
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + $"fullScreenVideoAd close  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "fullScreenVideoAd close";
            }

            if (this.example.fullScreenVideoAd != null)
            {
                this.example.fullScreenVideoAd.Dispose();
                this.example.fullScreenVideoAd = null;
            }
        }

        public void OnVideoComplete()
        {
            Debug.Log("CSJM_Unity " + $"fullScreenVideoAd complete  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "fullScreenVideoAd complete";
            }
        }

        public void OnVideoError()
        {
            Debug.Log("CSJM_Unity " + $"fullScreenVideoAd OnVideoError  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "fullScreenVideoAd OnVideoError";
            }
        }

        public void OnSkippedVideo()
        {
            Debug.Log("CSJM_Unity " + $"fullScreenVideoAd OnSkippedVideo  on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "fullScreenVideoAd skipped";
            }
        }
    }
}
