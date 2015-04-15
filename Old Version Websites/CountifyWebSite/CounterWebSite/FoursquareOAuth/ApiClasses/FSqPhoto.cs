using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth.ApiClasses
{
    public class FSqPhoto
    {
        public string prefix { get; set; }
        public string suffix { get; set; }

        public enum PhotoSize { ThirtySix, OneHundred, ThreeHundred, FiveHundred };
        private Dictionary<PhotoSize, string> sizeStrings = new Dictionary<PhotoSize, string>()
            {
                {PhotoSize.ThirtySix, "36x36"},
                {PhotoSize.OneHundred, "100x100"},
                {PhotoSize.ThreeHundred, "300x300"},
                {PhotoSize.FiveHundred, "500x500"},
            };

        public string GetUrl(PhotoSize size = PhotoSize.OneHundred)
        {
            return prefix + sizeStrings[size] + suffix;
        }
    }
}
