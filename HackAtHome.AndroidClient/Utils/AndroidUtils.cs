
using Android.App;
using Android.Content;

namespace HackAtHome.AndroidClient.Utils
{
    public static class AndroidUtils
    {
        public static void ShowMessage(Context context, string title, string message)
        {
            Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(context);
            AlertDialog Alert = Builder.Create();
            Alert.SetTitle(title);
            Alert.SetIcon(Resource.Drawable.Icon);
            Alert.SetMessage(message);
            Alert.SetButton("OK", (s, ev) => { });
            Alert.Show();
        }

        public static ProgressDialog ShowLoadDialog(Context context, string message)
        {
            Android.App.ProgressDialog progress;

            progress = new Android.App.ProgressDialog(context);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage(message);
            progress.SetCancelable(false);

            return progress;

        }
    }
}