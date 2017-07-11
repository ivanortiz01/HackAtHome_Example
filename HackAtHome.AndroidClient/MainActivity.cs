using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;
using System;
using HackAtHome.AndroidClient.Utils;
using Newtonsoft.Json;

namespace HackAtHome.AndroidClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/app", Theme = "@android:style/Theme.Material.Light")]
    public class MainActivity : Activity
    {
        private static string SUCCESS = "SUCCESS";
        private ProgressDialog ProcessDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Login);

            ProcessDialog = AndroidUtils.ShowLoadDialog(this, GetString(Resource.String.Cargando));

            var EmailText = FindViewById<EditText>(Resource.Id.editTextEmailLogin);
            var PasswordText = FindViewById<EditText>(Resource.Id.editTextPasswordLogin);
            var ValidateButton = FindViewById<Button>(Resource.Id.buttonValidateLogin);
            ValidateButton.Click += (s, e) =>
            {
                ProcessDialog.Show();

                var emailToValidate = EmailText.Text;
                var passwordToValidate = PasswordText.Text;
                if (!string.IsNullOrWhiteSpace(emailToValidate) && !string.IsNullOrWhiteSpace(passwordToValidate))
                {
                    Validate(emailToValidate, passwordToValidate);                    
                }
                else
                {
                    ProcessDialog.Dismiss();
                    AndroidUtils.ShowMessage(this, GetString(Resource.String.ErrorAutenticacionTitulo), Resources.GetString(Resource.String.ErrorAutenticacionCampos));
                }
            };
        }

        private async void Validate(string StudentEmail, string Password)
        {
            AutenticationService autenticationService = new AutenticationService();

            try
            {
                ResultInfo result = await autenticationService.AutenticateAsync(StudentEmail, Password);
                if (SUCCESS.Equals(result.Status.ToString().ToUpper()))
                {                    
                    var Intent = new Android.Content.Intent(this, typeof(ActivitiesListActivity));
                    Intent.PutExtra(AndroidConstants.USER_DATA_KEY, JsonConvert.SerializeObject(result));

                    ProcessDialog.Dismiss();
                    StartActivity(Intent);
                }
                else
                {
                    AndroidUtils.ShowMessage(this, GetString(Resource.String.ErrorAutenticacionTitulo), GetString(Resource.String.ErrorAutenticacionInvalido));
                }
            }
            catch (Exception ex)
            {
                string message = GetString(Resource.String.ErrorAutenticacionInterno);
                Android.Util.Log.Error(GetString(Resource.String.ApplicationName), $"{message} {ex.ToString()}");
                AndroidUtils.ShowMessage(this, GetString(Resource.String.ErrorAutenticacionTitulo), message);
            }
        }

    }
}

