using ClubHouse.Common;

using Prism.Mvvm;
using Prism.Regions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubHouse.UI.DesktopApp.ViewModels {
    public class ErrorViewModel : BindableBase, INavigationAware {
        public ErrorViewModel() {

        }

        private string _errorMessage;
        public string ErrorMessage {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            _errorMessage = navigationContext.Parameters.GetValue<string>(Consts.ErrorMessageNavigationParamtereKey);
        }
    }
}
