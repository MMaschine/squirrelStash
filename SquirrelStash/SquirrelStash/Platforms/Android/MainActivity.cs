using Android.App;
using Android.Content.PM;
using Android.OS;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace SquirrelStash
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();

            Window?.DecorView?.Post(ApplyBottomNavigationStyle);
        }

        private void ApplyBottomNavigationStyle()
        {
            var bottomNavigationView = FindBottomNavigationView(Window?.DecorView);

            if (bottomNavigationView is null)
            {
                return;
            }

            bottomNavigationView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
            bottomNavigationView.ItemHorizontalTranslationEnabled = false;
            bottomNavigationView.ItemIconSize = (int)(32 * Resources.DisplayMetrics.Density);
        }

        private static BottomNavigationView? FindBottomNavigationView(AView? root)
        {
            if (root is null)
            {
                return null;
            }

            if (root is BottomNavigationView bottomNavigationView)
            {
                return bottomNavigationView;
            }

            if (root is not AViewGroup viewGroup)
            {
                return null;
            }

            for (var index = 0; index < viewGroup.ChildCount; index++)
            {
                var result = FindBottomNavigationView(viewGroup.GetChildAt(index));

                if (result is not null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
