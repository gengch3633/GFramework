#if SDK_ADMOB
using GoogleMobileAds.Ump.Api;
#endif
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class GDPRPopup : UIPopup
    {
#if SDK_ADMOB
        // If true, it is safe to call MobileAds.Initialize() and load Ads.
        public static bool CanRequestAds => ConsentInformation.ConsentStatus == ConsentStatus.Obtained || ConsentInformation.ConsentStatus == ConsentStatus.NotRequired;
        public bool ForNonGDPRUser => ConsentInformation.ConsentStatus == ConsentStatus.NotRequired;
#endif

        protected override void OnInitVars()
        {
            base.OnInitVars();
            btnPrivacy.interactable = false;
            errorContainerVar.gameObject.SetActive(false);
        }

        public void GatherConsent(Action<string> onComplete)
        {
#if SDK_ADMOB
            // TagForUnderAgeOfConsent False means users are not under age.
            var requestParameters = new ConsentRequestParameters {  TagForUnderAgeOfConsent = false };
            // Combine the callback with an error popup handler.
            onComplete = (onComplete == null) ? UpdateErrorPopup : onComplete + UpdateErrorPopup;
            ConsentInformation.Update(requestParameters, (FormError updateError) =>
            {
                UpdatePrivacyButton();

                if (updateError != null)
                {
                    onComplete(updateError.Message);
                    return;
                }

                // Determine the consent-related action to take based on the ConsentStatus.
                if (CanRequestAds)
                {
                    // Consent has already been gathered or not required.
                    // Return control back to the user.
                    onComplete(null);
                    return;
                }

                // Consent not obtained and is required.
                // Load the initial consent request form for the user.
                ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
                {
                    UpdatePrivacyButton();
                    var message = showError != null ? showError.Message : null;
                    onComplete?.Invoke(message);
                });
            });
#else
            onComplete?.Invoke(null);
#endif
        }

        public void ShowPrivacyOptionsForm(Action<string> onComplete)
        {
#if SDK_ADMOB
            onComplete = (onComplete == null) ? UpdateErrorPopup : onComplete + UpdateErrorPopup;

            ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
            {
                UpdatePrivacyButton();
                var message = showError != null ? showError.Message : null;
                onComplete?.Invoke(message);
            });
#endif
        }

        public void ResetConsentInformation()
        {
#if SDK_ADMOB
            ConsentInformation.Reset();
            UpdatePrivacyButton();
#endif
        }

        void UpdatePrivacyButton()
        {
#if SDK_ADMOB
            btnPrivacy.interactable = ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required;
#endif
        }

        void UpdateErrorPopup(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            errorTextVar.text = message;
            errorContainerVar.gameObject.SetActive(true);
        }
        
        private Button btnPrivacy;
        private Button btnClose;
        private Text errorTextVar;
        private Image bkgVar;
        private Image errorContainerVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            btnPrivacy = transform.Find("errorContainer_Var/btnPrivacy").GetComponent<Button>();
            btnClose = transform.Find("errorContainer_Var/btnClose").GetComponent<Button>();
            errorTextVar = transform.Find("errorContainer_Var/errorText_Var").GetComponent<Text>();
            bkgVar = transform.Find("bkg_Var").GetComponent<Image>();
            errorContainerVar = transform.Find("errorContainer_Var").GetComponent<Image>();
            btnPrivacy.onClick.AddListener(OnBtnPrivacyClick);
            btnClose.onClick.AddListener(OnBtnCloseClick);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnPrivacy.onClick.RemoveListener(OnBtnPrivacyClick);
            btnClose.onClick.RemoveListener(OnBtnCloseClick);
        }

        private void OnBtnPrivacyClick()
        {
            OnBtnCloseClick();
        }
    }
}