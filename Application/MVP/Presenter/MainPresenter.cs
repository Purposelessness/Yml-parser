using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Application.MVP.Model;
using Application.MVP.View;

namespace Application.MVP.Presenter
{
    public class MainPresenter
    {
        public interface IView
        {
            public event EventHandler LoadData;
            public event EventHandler<string> ItemClick;
            
            public Activity Activity { get; }
            public string[] OfferList { set; }
            public string DebugText { set; }
            public bool IsLoading { set; }
        }

        private readonly IView _view;
        private readonly MainModel _model;

        public MainPresenter(IView view)
        {
            _view = view;
            _model = new MainModel();
            _model.DataIsReady += OnDataIsReady;
            _model.DebugAction += (s) => { _view.DebugText = s; };
            _view.LoadData += async (s, e) => { await OnDataLoad(s, e); };
            _view.ItemClick += OnItemClick;
        }

        protected virtual void OnDataIsReady(string[] arr)
        {
            _view.IsLoading = false;
            _view.DebugText = "data is ready";
            _view.OfferList = arr;
        }

        protected virtual async Task OnDataLoad(object sender, EventArgs e)
        {
            _view.IsLoading = true;
            _view.DebugText = "Loading data (presenter)";
            await _model.LoadData(
                "https://yastatic.net/market-export/_/partner/help/YML.xml");
        }

        protected virtual void OnItemClick(object? sender, string idStr)
        {
            if (!_model.TryGetOffer(idStr, out var offerJson))
            {
                _view.DebugText = "Invalid id";
            }

            var intent = new Intent(_view.Activity, typeof(MainActivity));
            intent.Extras?.PutString("OfferId", idStr);
            intent.Extras?.PutString("OfferJson", offerJson);
            _view.Activity.StartActivity(intent);
        }
    }
}