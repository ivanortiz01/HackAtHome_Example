using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.Entities;
using HackAtHome.SAL;
using Newtonsoft.Json;
using HackAtHome.AndroidClient.Utils;

namespace HackAtHome.AndroidClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/app", Theme = "@android:style/Theme.Holo")]
    public class ActivityDetailActivity : Activity
    {
        private ResultInfo UserData;
        private Evidence Evidence;
        private EvidenceDetail EvidenceDetail;

        private TextView UserNameViewText;
        private TextView TitleViewText;
        private TextView StatusViewText;
        private TextView DescriptionViewText;
        private ImageView ImageView;

        private static ActiviesService ACTIVIES_SERVICE = new ActiviesService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityDetail);

            UserNameViewText = FindViewById<TextView>(Resource.Id.textViewNameActivitiesList);
            TitleViewText = FindViewById<TextView>(Resource.Id.textViewTitleActivityDetail);
            StatusViewText = FindViewById<TextView>(Resource.Id.textViewStatusActivityDetail);
            DescriptionViewText = FindViewById<TextView>(Resource.Id.textViewDescriptionActivityDetail);
            ImageView = FindViewById<ImageView>(Resource.Id.imageViewActivityDetail);

            create(savedInstanceState);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString(AndroidConstants.USER_DATA_KEY, JsonConvert.SerializeObject(UserData));
            outState.PutString(AndroidConstants.EVIDENCE_DATA_KEY, JsonConvert.SerializeObject(Evidence));
            outState.PutString(AndroidConstants.EVIDENCE_DETAIL_DATA_KEY, JsonConvert.SerializeObject(EvidenceDetail));
            //outState.PutParcelable(AndroidConstants.EVIDENCE_LIST_KEY, ListActivities.OnSaveInstanceState());
        }

        private async void create(Bundle bundle)
        {
            try
            {                
                if (bundle == null)
                {
                    UserData = JsonConvert.DeserializeObject<ResultInfo>(Intent.GetStringExtra(AndroidConstants.USER_DATA_KEY));
                    Evidence = JsonConvert.DeserializeObject<Evidence>(Intent.GetStringExtra(AndroidConstants.EVIDENCE_DATA_KEY));
                    EvidenceDetail = await ACTIVIES_SERVICE.GetEvidenceByIDAsync(UserData.Token, Evidence.EvidenceID);
                } else
                {
                    UserData = JsonConvert.DeserializeObject<ResultInfo>(bundle.GetString(AndroidConstants.USER_DATA_KEY));
                    Evidence = JsonConvert.DeserializeObject<Evidence>(bundle.GetString(AndroidConstants.EVIDENCE_DATA_KEY));
                    EvidenceDetail = JsonConvert.DeserializeObject<EvidenceDetail>(bundle.GetString(AndroidConstants.EVIDENCE_DETAIL_DATA_KEY));
                }
                
                UserNameViewText.Text = UserData.FullName;
                StatusViewText.Text = Evidence.Status;
                DescriptionViewText.Text = EvidenceDetail.Description;
                Koush.UrlImageViewHelper.SetUrlDrawable(ImageView, EvidenceDetail.Url);
            }
            catch (Exception ex)
            {
                string message = GetString(Resource.String.ErrorDetalleActividadInterno);
                Android.Util.Log.Error(GetString(Resource.String.ApplicationName), $"{message} {ex.ToString()}");
                AndroidUtils.ShowMessage(this, GetString(Resource.String.ErrorDetalleActividadTitulo), message);
            }
        }
    }
}