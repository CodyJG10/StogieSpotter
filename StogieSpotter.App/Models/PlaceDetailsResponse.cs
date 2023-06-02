using GoogleApi.Entities.Places.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.Models
{
    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    public class CurrentOpeningHours
    {
        public bool OpenNow { get; set; }
        public List<Period> Periods { get; set; }
        public List<string> WeekdayText { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Viewport
    {
        public Location Northeast { get; set; }
        public Location Southwest { get; set; }
    }

    public class Period
    {
        public Close Close { get; set; }
        public Open Open { get; set; }
    }

    public class Close
    {
        public string Date { get; set; }
        public int Day { get; set; }
        public string Time { get; set; }
        public bool Truncated { get; set; }
    }

    public class Open
    {
        public string Date { get; set; }
        public int Day { get; set; }
        public string Time { get; set; }
        public bool Truncated { get; set; }
    }

    public class Photo
    {
        public int Height { get; set; }
        public List<string> HtmlAttributions { get; set; }
        public string PhotoReference { get; set; }
        public int Width { get; set; }
    }

    public class PlusCode
    {
        public string CompoundCode { get; set; }
        public string GlobalCode { get; set; }
    }

    public class Review
    {
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public string Language { get; set; }
        public string OriginalLanguage { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public int Rating { get; set; }
        public string RelativeTimeDescription { get; set; }
        public string Text { get; set; }
        public int Time { get; set; }
        public bool Translated { get; set; }
    }

    public class DetailsResopnse
    {
        public List<AddressComponent> AddressComponents { get; set; }
        public string AdrAddress { get; set; }
        public string BusinessStatus { get; set; }
        public bool CurbsidePickup { get; set; }
        public CurrentOpeningHours CurrentOpeningHours { get; set; }
        public bool Delivery { get; set; }
        public bool DineIn { get; set; }
        public string FormattedAddress { get; set; }
        public string FormattedPhoneNumber { get; set; }
        public Geometry Geometry { get; set; }
        public string Icon { get; set; }
        public string IconBackgroundColor { get; set; }
        public string IconMaskBaseUri { get; set; }
        public string InternationalPhoneNumber { get; set; }
        public string Name { get; set; }
        public OpeningHours OpeningHours { get; set; }
        public List<Photo> Photos { get; set; }
        public string PlaceId { get; set; }
        public PlusCode PlusCode { get; set; }
        public int PriceLevel { get; set; }
        public double Rating
        {
            get;

        }
    }
}