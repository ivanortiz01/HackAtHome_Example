
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
    }
}