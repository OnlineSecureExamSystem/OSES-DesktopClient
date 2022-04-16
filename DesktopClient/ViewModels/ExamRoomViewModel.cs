using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class ExamRoomViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment => "/ExamRoom";

        public IScreen HostScreen { get; }

        public ExamRoomViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
