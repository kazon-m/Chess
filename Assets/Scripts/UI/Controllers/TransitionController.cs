using Helpers;
using Systems;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class TransitionController : IController
    {
        private TransitionView _view;
        
        public Menu menu;

        public void Show() => _view.gameObject.SetActive(true);

        public void Hide() => _view.gameObject.SetActive(false);

        public void Init()
        {
            if(_view != null) return;
            
            _view = ObjectCreator.Create<TransitionView>("UI/TransitionView/TransitionView", menu.UI);
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