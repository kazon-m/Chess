using Helpers;
using Systems;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class LobbyController : IController
    {
        private LobbyView _view;

        public Menu menu;

        public void Show() => _view.gameObject.SetActive(true);

        public void Hide() => _view.gameObject.SetActive(false);

        public void Init()
        {
            if(_view != null) return;
            
            _view = ObjectCreator.Create<LobbyView>("UI/LobbyView/LobbyView", menu.UI);
            _view.gameObject.SetActive(false);
            
            if(_view.playButton != null) _view.playButton.onClick.AddListener(OnPlayClick);
            if(_view.infoButton != null) _view.infoButton.onClick.AddListener(OnInfoClick);
            if(_view.settingsButton != null) _view.settingsButton.onClick.AddListener(OnSettingsClick);
        }

        public void Destroy()
        {
            if(_view == null) return;
            
            if(_view.playButton != null) _view.playButton.onClick.RemoveAllListeners();
            if(_view.infoButton != null) _view.infoButton.onClick.RemoveAllListeners();
            if(_view.settingsButton != null) _view.settingsButton.onClick.RemoveAllListeners();

            Object.Destroy(_view.gameObject);
            _view = null;
        }

        private void OnPlayClick()
        {
            Hide();
        }

        private void OnInfoClick()
        {
            Hide();
        }

        private void OnSettingsClick()
        {
            Hide();
            menu.Get<SettingsController>().Show();
        }
    }
}