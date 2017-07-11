using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.Entities;
using HackAtHome.SAL;
using HackAtHome.AndroidClient.Adapters;
using Newtonsoft.Json;
using HackAtHome.AndroidClient.Utils;
using static Android.Widget.AdapterView;

namespace HackAtHome.AndroidClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/app", Theme = "@android:style/Theme.Material.Light")]
    public class ActivitiesListActivity : Activity
    {
        private ResultInfo UserData;
        private TextView UserNameViewText;
        private ListView ListActivities;
        private List<Evidence> Evidences;
        private ProgressDialog ProcessDialog;

        private static ActiviesService ACTIVIES_SERVICE = new ActiviesService();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ActivitiesList);

            ProcessDialog = AndroidUtils.ShowLoadDialog(this, GetString(Resource.String.Cargando));

            UserNameViewText = FindViewById<TextView>(Resource.Id.textViewNameActivitiesList);
            ListActivities = FindViewById<ListView>(Resource.Id.listViewActiviesList);

            create(bundle);
        }
        
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString(AndroidConstants.USER_DATA_KEY, JsonConvert.SerializeObject(UserData));
            outState.PutString(AndroidConstants.EVIDENCES_DATA_KEY, JsonConvert.SerializeObject(Evidences));
        }

        private async void create(Bundle bundle)
        {
            try
            {
                ProcessDialog.Show();

                if (bundle == null)
                {
                    UserData = JsonConvert.DeserializeObject<ResultInfo>(Intent.GetStringExtra(AndroidConstants.USER_DATA_KEY));
                    Evidences = await ACTIVIES_SERVICE.GetEvidencesAsync(UserData.Token.ToString());                    
                }
                else
                {
                    UserData = JsonConvert.DeserializeObject<ResultInfo>(bundle.GetString(AndroidConstants.USER_DATA_KEY));
                    Evidences = JsonConvert.DeserializeObject<List<Evidence>>(bundle.GetString(AndroidConstants.EVIDENCES_DATA_KEY));
                }

                UserNameViewText.Text = UserData.FullName;

                ListActivities.Adapter = new ActiviesAdapter(
                        this,
                        Evidences,
                        Resource.Layout.ActivitiesListItems,
                        Resource.Id.textViewTitleActiviesListItem,
                        Resource.Id.textViewStatusActivitiesListItem);

                ListActivities.ItemClick += (object sender, ItemClickEventArgs e) =>
                {
                    Evidence Evidence = Evidences[e.Position];
                    
                    var Intent = new Android.Content.Intent(this, typeof(ActivityDetailActivity));
                    Intent.PutExtra(AndroidConstants.USER_DATA_KEY, JsonConvert.SerializeObject(UserData));
                    Intent.PutExtra(AndroidConstants.EVIDENCE_DATA_KEY, JsonConvert.SerializeObject(Evidence));
                    StartActivity(Intent);
                };

                ProcessDialog.Dismiss();
            }
            catch (Exception ex)
            {
                string message = GetString(Resource.String.ErrorListaActividadesInterno);
                Android.Util.Log.Error(GetString(Resource.String.ApplicationName), $"{message} {ex.ToString()}");
                AndroidUtils.ShowMessage(this, GetString(Resource.String.ErrorListaActividadesTitulo), message);
            }
        }
    }
}