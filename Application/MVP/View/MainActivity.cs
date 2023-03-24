using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using Application.MVP.Presenter;

namespace Application.MVP.View
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme",
        MainLauncher = true)]
    public class MainActivity : AppCompatActivity, MainPresenter.IView
    {
        public event EventHandler LoadData = delegate { };
        public event EventHandler<string> ItemClick = delegate { };

        public bool IsLoading
        {
            set
            {
                if (value)
                {
                    RunOnUiThread(() => { _progressDialog.Show(); });
                }
                else
                {
                    RunOnUiThread(() => { _progressDialog.Dismiss(); });
                }
            }
        }

        public Activity Activity => this;

        public string[] OfferList
        {
            set
            {
                if (value.Length == 0)
                {
                    _debugText.Text = "Null data";
                }
                else
                {
                    _debugText.Text = "Data is loaded";
                    RunOnUiThread(() =>
                    {
                        _offerAdapter.Offers.AddRange(value);
                        _offerAdapter.NotifyItemRangeInserted(
                            _offerAdapter.Offers.Count - value.Length,
                            value.Length);
                    });
                }
            }
        }

        public string DebugText
        {
            set => _debugText.Text = value;
        }

        private MainPresenter? _presenter;

        private Button _loadDataButton = null!;

        private RecyclerView _recyclerView = null!;
        private RecyclerView.LayoutManager _layoutManager = null!;
        private OfferAdapter _offerAdapter = null!;

        private ProgressDialog _progressDialog = null!;
        private TextView _debugText = null!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainActivity);

            _loadDataButton = FindViewById<Button>(Resource.Id.LoadDataButton)!;
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetMessage("Wait..");
            _progressDialog.SetCancelable(false);
            _debugText = FindViewById<TextView>(Resource.Id.DebugText)!;

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.OfferList)!;

            _layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(_layoutManager);
            _recyclerView.HasFixedSize = true;
            _offerAdapter = new OfferAdapter();
            _recyclerView.SetAdapter(_offerAdapter);

            _offerAdapter.ItemClick += async (s, position) =>
            {
                await Task.Run(() => ItemClick(this,
                    _offerAdapter.Offers[position]));
            };

            _loadDataButton.Click += async (s, e) =>
            {
                await Task.Run(() => LoadData(this, EventArgs.Empty));
            };
            
            _presenter = new MainPresenter(this);
        }
    }
}