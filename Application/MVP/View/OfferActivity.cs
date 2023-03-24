using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text.Method;
using Android.Widget;

namespace Application.MVP.View
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class OfferActivity : AppCompatActivity
    {
        private TextView _textView = null!;
        
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OfferActivity);

            _textView = FindViewById<TextView>(Resource.Id.OfferJson)!;
            _textView.MovementMethod = new ScrollingMovementMethod();
            var id = Intent?.Extras?.GetString("OfferId");
            var json = Intent?.Extras?.GetString("OfferJson");
            _textView.Text = json;
        }
    }
}