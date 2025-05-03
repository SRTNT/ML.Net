using ML.Net.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.Net
{
    public class AdvertisementRounded
    {
        public float AreaRounded;
        public float RoomsRounded;
        public float FloorRounded;
    }


    public class AdvertisementMapping
    {
        public static void MapRounded(Advertisement input, AdvertisementRounded output)
        {
            output.AreaRounded = (float)Math.Round(input.Area);
            output.RoomsRounded = (float)Math.Round(input.Rooms);
            output.FloorRounded = (float)Math.Round(input.Floor);
        }
    }
}
