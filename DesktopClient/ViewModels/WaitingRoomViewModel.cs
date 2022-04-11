using DesktopClient.Helpers;
using DesktopClient.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class WaitingRoomViewModel : ViewModelBase, IRoutableViewModel
    {

        public string? UrlPathSegment => "/WaitingRoom";

        public IScreen HostScreen { get; }

        public WaitingRoomViewModel(IScreen screen)
        {
            HostScreen = screen;


        }
    }
}
