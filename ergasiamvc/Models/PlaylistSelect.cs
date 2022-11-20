using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ergasiamvc.Models
{
    public class PlaylistSelect
    {
        public Track track { get; set; }
        public List<Playlist> pl { get; set; }

        public PlaylistSelect(Track t, List<Playlist> p)
        {
            track = t;
            pl = p;
        }
    }
}