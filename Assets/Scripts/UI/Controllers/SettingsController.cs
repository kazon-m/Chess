using Helpers;
using Systems;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class SettingsController : IController
    {
        private Menu _menu;
        private SettingsView _view;

        public void Show() => _view.gameObject.SetActive(true);

        public void Hide() => _view.gameObject.SetActive(false);

        public void Init()
        {
            if(_view != null) return;

            _view = ObjectCreator.Create<SettingsView>("UI/SettingsView/SettingsView", _menu.UI);
            _view.gameObject.SetActive(false);
        }

        public void Destroy()
        {
            if(_view == null) return;

            Object.Destroy(_view.gameObject);
            _view = null;
        }
    }
}