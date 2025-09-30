using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation;
using System.Linq;

namespace Adfurikun
{
    public class AdfurikunAdMobMediationExtras : MediationExtras
    {
        private const string EXTRA_KEY_IS_DEBUG_MODE = "ADF_IS_DEBUG_MODE";
        private const string EXTRA_KEY_SOUND_STATE = "ADF_SOUND_STATE";
        private const string EXTRA_KEY_HAS_USER_CONSENT = "ADF_HAS_USER_CONSENT";
        private const string EXTRA_KEY_IS_CHILD_DIRECTED = "ADF_IS_CHILD_DIRECTED";
        private const string EXTRA_KEY_IS_GOOGLE_FAMILIES_POLICY = "ADF_IS_GOOGLE_FAMILIES_POLICY";
        private const string EXTRA_KEY_IS_APPLICATION_FOR_CHILD = "ADF_IS_APPLICATION_FOR_CHILD";
        private const string EXTRA_KEY_CUSTOM_PARAMS = "ADF_CUSTOM_PARAMS";
        private const string EXTRA_KEY_LOAD_TIMEOUT = "ADF_LOAD_TIMEOUT";
        private const string EXTRA_KEY_NATIVE_AD_WIDTH = "ADF_NATIVE_AD_WIDTH";
        private const string EXTRA_KEY_NATIVE_AD_HEIGHT = "ADF_NATIVE_AD_HEIGHT";

        private string mediationExtraBuilderClassName;
        private bool isDebugMode = false;
        private bool? soundState = null;
        private bool? hasUserConsent = null;
        private bool? isChildDirected = null;
        private bool? isGoogleFamiliesPolicy = null;
        private bool isApplicationForChild = false;
        private Dictionary<string, string> customParams;
        private float loadTimeout = 0.0f;
        private int nativeAdWidth = 0;
        private int nativeAdHeight = 0;

        public AdfurikunAdMobMediationExtras(AdType adType)
        {
            Extras = new Dictionary<string, string>();
            SetMediationExtraBuilderClassName(adType);
        }

        /*
        Debug Log出力モードを設定（true: ON、false: OFF）
         */
        public void SetDebugMode(bool isDebugMode)
        {
            this.isDebugMode = isDebugMode;
        }

        /*
        音をコントロールを設定（true: BGM ON、false: Mute）
        端末設定に合わせる場合には、この関数を設定しないでください。
         */
        public void SetSoundState(bool soundState)
        {
            this.soundState = soundState;
        }

        /*
        ユーザーGDPR同意の設定（true: 同意する、false: 同意しない）
        EU地域外の場合には、この関数を設定しないでください。
         */
        public void SetHasUserConsent(bool hasUserConsent)
        {
            this.hasUserConsent = hasUserConsent;
        }

        /*
        ユーザーCOPPAの設定（true: 同意する、false: 同意しない）
        COPPA対応が必要ない場合には、この関数を設定しないでください。
         */
        public void SetChildDirected(bool isChildDirected)
        {
            this.isChildDirected = isChildDirected;
        }

        /*
        Googleファミリーポリシーの設定（true: 同意する、false: 同意しない）
         */
        public void SetGoogleFamiliesPolicy(bool isGoogleFamiliesPolicy)
        {
#if UNITY_ANDROID
            this.isGoogleFamiliesPolicy = isGoogleFamiliesPolicy;
#endif
        }

        /*
        子供向けのアプリケーションで特定アドネットワークを停止の設定（true: 停止する、false: 停止しない）
         */
        public void SetApplicationForChild(bool isApplicationForChild)
        {
            this.isApplicationForChild = isApplicationForChild;
        }

        /*
        カスタムパラメータ情報の設定
         */
        public void SetCustomParams(Dictionary<string, string> customParams)
        {
            this.customParams = customParams;
        }

        /*
        広告Loadタイムアウトの設定
         */
        public void SetLoadTimeout(float timeout)
        {
            this.loadTimeout = timeout;
        }

        /*
        NativeAdのサイズの設定
         */
        public void SetNativeAdSize(int width, int height)
        {
            this.nativeAdWidth = width;
            this.nativeAdHeight = height;
        }

        /*
        MediationExtras情報のセットアップ
         */
        public void SetupMediationExtras(AdRequest adRequest)
        {
            if (adRequest != null)
            {
                try
                {
                    Extras[EXTRA_KEY_IS_DEBUG_MODE] = this.isDebugMode.ToString();
                    if (this.soundState != null)
                    {
                        Extras[EXTRA_KEY_SOUND_STATE] = this.soundState.ToString();
                    }
                    if (this.hasUserConsent != null)
                    {
                        Extras[EXTRA_KEY_HAS_USER_CONSENT] = this.hasUserConsent.ToString();
                    }
                    if (this.isChildDirected != null)
                    {
                        Extras[EXTRA_KEY_IS_CHILD_DIRECTED] = this.isChildDirected.ToString();
                    }
                    if (this.isGoogleFamiliesPolicy != null)
                    {
                        Extras[EXTRA_KEY_IS_GOOGLE_FAMILIES_POLICY] = this.isGoogleFamiliesPolicy.ToString();
                    }
                    Extras[EXTRA_KEY_IS_APPLICATION_FOR_CHILD] = this.isApplicationForChild.ToString();
                    if (this.customParams != null && this.customParams.Count > 0)
                    {
                        string customParamsString = "{";
                        var customParamList = this.customParams.ToList();
                        for (int i = 0; i < customParamList.Count; i++)
                        {
                            customParamsString += "\"" + customParamList[i].Key + "\":\"" + customParamList[i].Value + "\"";
                            if ((customParamList.Count - 1) > i)
                            {
                                customParamsString += ",";
                            }
                        }
                        customParamsString += "}";
                        Extras[EXTRA_KEY_CUSTOM_PARAMS] = customParamsString;
                    }
                    if (this.loadTimeout > 0.0f)
                    {
                        Extras[EXTRA_KEY_LOAD_TIMEOUT] = this.loadTimeout.ToString();
                    }
                    if (this.nativeAdWidth > 0)
                    {
                        Extras[EXTRA_KEY_NATIVE_AD_WIDTH] = this.nativeAdWidth.ToString();
                    }
                    if (this.nativeAdHeight > 0)
                    {
                        Extras[EXTRA_KEY_NATIVE_AD_HEIGHT] = this.nativeAdHeight.ToString();
                    }
                    adRequest.MediationExtras.Add(this);
                }
                catch (System.Exception e){}
            }
        }

        public override string AndroidMediationExtraBuilderClassName
        {
            get 
            {
                return this.mediationExtraBuilderClassName;
            }
        }

        public override string IOSMediationExtraBuilderClassName
        {
            get 
            {
                return this.mediationExtraBuilderClassName;
            }
        }

        /*
        Mediationクラス名を設定
         */
        private void SetMediationExtraBuilderClassName(AdType adType)
        {
#if UNITY_IPHONE
            this.mediationExtraBuilderClassName = "AdfurikunAdnetworkExtraBuilder";
#elif UNITY_ANDROID
            switch (adType)
            {
                case AdType.APP_OPEN_AD:
                    this.mediationExtraBuilderClassName = "jp.tjkapp.adfurikunsdk.moviereward.AdfurikunAdMobAppOpenAdBuilder";
                    break;
                case AdType.INTERSTITIAL:
                    this.mediationExtraBuilderClassName = "jp.tjkapp.adfurikunsdk.moviereward.AdfurikunAdMobInterstitialBuilder";
                    break;
                case AdType.NATIVE_AD:
                    this.mediationExtraBuilderClassName = "jp.tjkapp.adfurikunsdk.moviereward.AdfurikunAdMobNativeAdBuilder";
                    break;
                case AdType.RECTANGLE:
                    this.mediationExtraBuilderClassName = "jp.tjkapp.adfurikunsdk.moviereward.AdfurikunAdMobRectangleBuilder";
                    break;
                case AdType.BANNER:
                    this.mediationExtraBuilderClassName = "jp.tjkapp.adfurikunsdk.moviereward.AdfurikunAdMobBannerBuilder";
                    break;
                // Defaultをリワードで設定する
                default:
                    this.mediationExtraBuilderClassName = "jp.tjkapp.adfurikunsdk.moviereward.AdfurikunAdMobRewardBuilder";
                    break;
            }
#endif
        }

        public enum AdType
        {
            APP_OPEN_AD = 0,
            REWARD = 1,
            INTERSTITIAL = 2,
            NATIVE_AD = 3,
            RECTANGLE = 4,
            BANNER = 5
        }
    }
}
