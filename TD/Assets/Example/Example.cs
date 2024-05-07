//------------------------------------------------------------------------------
// Copyright (c) 2018-2023 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using ByteDance.Union.Mediation;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The example for the SDK.
/// </summary>
///
public sealed class Example : MonoBehaviour
{
    [SerializeField]
    public Text information;

    public NativeAd bannerAd;                    // 自渲染banner，仅支持csj。推荐使用ExpressBannerAd
    public ExpressBannerAd mExpressBannerAd;     // 模板banner，支持csj和融合
    public BUSplashAd splashAd;                  // 开屏广告，支持csj和融合
    public ExpressAd mExpressFeedad;             // 模板feed，仅支持csj
    public FeedAd feedAd;                        // 自渲染feed，支持csj和融合。在融合里模板和自渲染都支持。
    public DrawFeedAd drawFeedAd;                // drawFeed，仅支持融合
    public FullScreenVideoAd fullScreenVideoAd;  // 插全屏和新插屏，支持csj和融合
    public RewardVideoAd rewardAd;               // 激励视频，支持csj和融合
    public ExpressRewardVideoAd expressRewardAd; // 模板激励视频，仅支持iOS csj。推荐使用RewardVideoAd

    // Unity 主线程ID:
    public static int MainThreadId;
    public static int MNowPlayAgainCount = 0;
    public static int MNextPlayAgainCount = 0;

    public static bool useMediation = true;

    private void Awake()
    {
        MainThreadId = Thread.CurrentThread.ManagedThreadId;
    }

    private void SdkInitCallback(bool success, string message)
    {
        // 注意：在初始化回调成功后再请求广告
        Debug.Log("CSJM_Unity "+"sdk初始化结束：success: " + success + ", message: " + message);
    }

    void Start()
    {
        // sdk初始化
        SDKConfiguration sdkConfiguration = new SDKConfiguration.Builder()
            .SetAppId(CSJMDAdPositionId.APP_ID)
            .SetAppName("APP测试媒体")
            .SetUseMediation(Example.useMediation) // 是否使用融合功能，置为false，可不初始化聚合广告相关模块
            .SetDebug(true) // debug日志开关，app发版时记得关闭
            .SetMediationConfig(GetMediationConfig())
            .SetPrivacyConfigurationn(GetPrivacyConfiguration())
            .Build();
        Pangle.Init(sdkConfiguration); // 合规要求，初始化分为2步，第一步先调用init
        Pangle.Start(SdkInitCallback); // 第二步再调用start。注意在初始化回调成功后再请求广告
    }

    /* 💖💖💖💖💖💖💖💖💖💖💖💖💖💖 ↓↓↓↓↓↓↓↓↓↓ 广告sdk初始化 及 其他设置相关 ↓↓↓↓↓↓↓↓↓↓ 💖💖💖💖💖💖💖💖💖💖💖💖💖💖 */

    /**
      * 初始化时进行隐私合规相关配置。不设置的将使用默认值
      */
    private PrivacyConfiguration GetPrivacyConfiguration()
    {
        // 这里仅展示了部分设置，开发者根据自己需要进行设置，不设置的将使用默认值，默认值可能不合规。
        PrivacyConfiguration privacyConfig = new PrivacyConfiguration();
        privacyConfig.CanUsePhoneState = false;
        privacyConfig.CanUsePermissionRecordAudio = true;
        privacyConfig.CanUseLocation = false;
        privacyConfig.Longitude = 115.7;
        privacyConfig.Latitude = 39.4;

        // 融合相关配置示例
        privacyConfig.MediationPrivacyConfig = new MediationPrivacyConfig();
        privacyConfig.MediationPrivacyConfig.LimitPersonalAds = false;
        privacyConfig.MediationPrivacyConfig.ProgrammaticRecommend = false;
        privacyConfig.MediationPrivacyConfig.forbiddenCAID = false;
        privacyConfig.MediationPrivacyConfig.CanUseOaid = false;

        return privacyConfig;
    }

    /**
     * 使用融合功能时，初始化时进行相关配置
     */
    private MediationConfig GetMediationConfig()
    {
        MediationConfig mediationConfig = new MediationConfig();

        // 聚合配置json字符串（从gromore平台下载），用于首次安装时作为兜底配置使用。可选
        mediationConfig.CustomLocalConfig = MediationLocalConfig.CONFIG_JSON_STR;

        // 流量分组功能，可选
        MediationConfigUserInfoForSegment segment = new MediationConfigUserInfoForSegment();
        segment.Age = 18;
        segment.Gender = AdConst.GENDER_MALE;
        segment.Channel = "mediation-unity";
        segment.SubChannel = "mediation-sub-unity";
        segment.UserId = "mediation-userId-unity";
        segment.UserValueGroup = "mediation-user-value-unity";
        segment.CustomInfos = new Dictionary<string, string>
        {
            { "customKey", "customValue" }
        };
        mediationConfig.MediationConfigUserInfoForSegment = segment;

        return mediationConfig;
    }

    /* 💖💖💖💖💖💖💖💖💖💖💖💖💖💖 ↑↑↑↑↑↑↑↑↑↑ 广告sdk初始化 及 其他设置相关 ↑↑↑↑↑↑↑↑↑↑ 💖💖💖💖💖💖💖💖💖💖💖💖💖💖 */


    /* 💛💛💛💛💛💛💛💛💛💛💛💛💛💛 ↓↓↓↓↓↓↓↓↓↓ 激励视频相关样例 ↓↓↓↓↓↓↓↓↓↓ 💛💛💛💛💛💛💛💛💛💛💛💛💛💛 */

    // Load the reward Ad.
    public void LoadRewardAd()
    {
        ExampleRewardAd.LoadReward(this, false);
    }

    // Show the reward Ad.
    public void ShowRewardAd()
    {
        ExampleRewardAd.ShowReward(this);
    }

    // Load the expressReward Ad. 仅iOS端穿山甲
    public void LoadExpressRewardAd()
    {
        ExampleExpressRewardAd.LoadExpressRewardAd(this);
    }

    // Show the expressReward Ad. 仅iOS端穿山甲
    public void ShowExpressRewardAd()
    {
        ExampleExpressRewardAd.ShowExpressRewardAd(this);
    }

    // load mediation reward ad
    public void LoadMediationRewardAd()
    {
        ExampleRewardAd.LoadReward(this, true);
    }

    // Show the mediation reward Ad.
    public void ShowMediationRewardAd()
    {
        ExampleRewardAd.ShowReward(this);
    }
    /* 💛💛💛💛💛💛💛💛💛💛💛💛💛💛 ↑↑↑↑↑↑↑↑↑↑ 激励视频相关样例 ↑↑↑↑↑↑↑↑↑↑ 💛💛💛💛💛💛💛💛💛💛💛💛💛💛 */


    /* 💜💜💜💜💜💜💜💜💜💜💜💜💜💜 ↓↓↓↓↓↓↓↓↓↓ 开屏广告相关样例 ↓↓↓↓↓↓↓↓↓↓ 💜💜💜💜💜💜💜💜💜💜💜💜💜💜 */

    // load and show splash ad
    public void LoadAndShowSplashAd()
    {
        ExampleSplashAd.LoadAndShowSplashAd(this, false);
    }

    // load and show mediation splash ad
    public void LoadAndShowMediationSplashAd()
    {
        ExampleSplashAd.LoadAndShowSplashAd(this, true);
    }

    /* 💜💜💜💜💜💜💜💜💜💜💜💜💜💜 ↑↑↑↑↑↑↑↑↑↑ 开屏广告相关样例 ↑↑↑↑↑↑↑↑↑↑ 💜💜💜💜💜💜💜💜💜💜💜💜💜💜 */


    /* ❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️ ↓↓↓↓↓↓↓↓↓↓ 插全屏广告相关样例 ↓↓↓↓↓↓↓↓↓↓ ❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️ */
    // Loads the full screen video ad.
    public void LoadFullScreenVideoAd()
    {
        ExampleFullScreenVideoAd.LoadFullScreenVideoAd(this);
    }

    // Show the fullScreen Ad.
    public void ShowFullScreenVideoAd()
    {
        ExampleFullScreenVideoAd.ShowFullScreenVideoAd(this);
    }

    // Loads the express full screen video ad.
    public void LoadExpressFullScreenVideoAd()
    {
        ExampleExpressFullScreenVideoAd.LoadExpressFullScreenVideoAd(this, false);
    }

    // Show the express full screen Ad.
    public void ShowExpressFullScreenVideoAd()
    {
        ExampleExpressFullScreenVideoAd.ShowExpressFullScreenVideoAd(this);
    }

    // Loads the mediation full screen video ad.
    public void LoadMediationFullScreenVideoAd()
    {
        ExampleExpressFullScreenVideoAd.LoadExpressFullScreenVideoAd(this, true);
    }

    // Show the mediation full screen Ad.
    public void ShowMediationFullScreenVideoAd()
    {
        ExampleExpressFullScreenVideoAd.ShowExpressFullScreenVideoAd(this);
    }
    /* ❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️ ↑↑↑↑↑↑↑↑↑↑ 插全屏广告相关样例 ↑↑↑↑↑↑↑↑↑↑ ❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️ */


    /* 💙💙💙💙💙💙💙💙💙💙💙💙💙💙 ↓↓↓↓↓↓↓↓↓↓ banner广告相关样例 ↓↓↓↓↓↓↓↓↓↓ 💙💙💙💙💙💙💙💙💙💙💙💙💙💙 */
    public void LoadNativeBannerAd()
    {
        ExampleBannerAd.LoadNativeBannerAd(this);
    }

    public void ShowNativeBannerAd()
    {
        ExampleBannerAd.ShowNativeBannerAd(this);
    }

    // load express banner
    public void LoadExpressBannerAd()
    {
        ExampleExpressBannerAd.LoadExpressBannerAd(this, false);
    }

    // Show the express banner Ad.
    public void ShowExpressBannerAd()
    {
        ExampleExpressBannerAd.ShowExpressBannerAd(this);
    }

    // load mediation banner
    public void LoadMediationBannerAd()
    {
        ExampleExpressBannerAd.LoadExpressBannerAd(this, true);
    }

    // Show the mediation banner Ad.
    public void ShowMediationBannerAd()
    {
        ExampleExpressBannerAd.ShowExpressBannerAd(this);
    }
    /* 💙💙💙💙💙💙💙💙💙💙💙💙💙💙 ↑↑↑↑↑↑↑↑↑↑ banner广告相关样例 ↑↑↑↑↑↑↑↑↑↑ 💙💙💙💙💙💙💙💙💙💙💙💙💙💙 */


    /* 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 ↓↓↓↓↓↓↓↓↓↓ feed广告相关样例 ↓↓↓↓↓↓↓↓↓↓ 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 */
    // load express feed ad
    public void LoadExpressFeedAd()
    {
        ExampleExpressFeedAd.LoadExpressFeedAd(this);
    }

    // Show the expressFeed Ad.
    public void ShowExpressFeedAd()
    {
        ExampleExpressFeedAd.ShowExpressFeedAd(this);
    }

    // load feed ad.
    public void LoadFeedAd()
    {
        ExampleFeedAd.LoadFeedAd(this, false);
    }

    // Show the Feed Ad.
    public void ShowFeedAd()
    {
        ExampleFeedAd.ShowFeedAd(this);
    }

    // load mediation feed ad.
    public void LoadMediationFeedAd()
    {
        ExampleFeedAd.LoadFeedAd(this, true);
    }

    // Show the mediation Feed Ad.
    public void ShowMediationFeedAd()
    {
        ExampleFeedAd.ShowFeedAd(this);
    }
    /* 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 ↑↑↑↑↑↑↑↑↑↑ feed广告相关样例 ↑↑↑↑↑↑↑↑↑↑ 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 */


    /* 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 ↓↓↓↓↓↓↓↓↓↓ DrawFeed广告相关样例 ↓↓↓↓↓↓↓↓↓↓ 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 */

    // load mediation draw feed ad
    public void LoadMediationDrawFeedAd()
    {
        ExampleDrawFeedAd.LoadDrawFeedAd(this);
    }

    // show mediation draw feed ad
    public void ShowMediationDrawFeedAd()
    {
        ExampleDrawFeedAd.ShowDrawFeedAd(this);
    }

    /* 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 ↑↑↑↑↑↑↑↑↑↑ DrawFeed广告相关样例 ↑↑↑↑↑↑↑↑↑↑ 🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤🖤 */

    // Dispose the reward Ad.
    public void DisposeAds()
    {
        // 激励
        if (this.rewardAd != null)
        {
            this.rewardAd.Dispose();
            this.rewardAd = null;
        }
        if (this.expressRewardAd != null)
        {
            this.expressRewardAd.Dispose();
            this.expressRewardAd = null;
        } 

        // 全屏/新插屏
        if (this.fullScreenVideoAd != null)
        {
            this.fullScreenVideoAd.Dispose();
            this.fullScreenVideoAd = null;
        }

        // banner
        if (this.bannerAd != null)
        {
            this.bannerAd.Dispose();
            this.bannerAd = null;
        }
        if (this.mExpressBannerAd != null)
        {
            this.mExpressBannerAd.Dispose();
            this.mExpressBannerAd = null;
        }

        // 信息流
        if (this.feedAd != null)
        {
            this.feedAd.Dispose();
            this.feedAd = null;
        }
        if (this.mExpressFeedad != null)
        {
            this.mExpressFeedad.Dispose();
            this.mExpressFeedad = null;
        }
        if (this.drawFeedAd != null)
        {
            this.drawFeedAd.Dispose();
            this.drawFeedAd = null;
        }

        // 开屏
        if (this.splashAd != null)
        {
            this.splashAd.Dispose();
            this.splashAd = null;
        }
    }
}
