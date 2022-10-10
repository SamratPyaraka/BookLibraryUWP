using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BookLibrary1.Model
{
    public class Books
    {
        public int BookID { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }
        public string ISBN13 { get; set; }
        public string ISBN10 { get; set; }

        public string Authors { get; set; }

        public string Category { get; set; }

        public string ImageURL { get; set; }
        public string PublishedYear { get; set; }
        public int BookCount { get; set; }
        public float AverageRating { get; set; }
        public int NumberOfPages { get; set; }
        public int RatingsCount { get; set; }
        public KeepType KeepType { get; set; }
        public DateTime InsertedDate { get; set; }
        public string InsertedBy { get; set; } = "";
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; } = "";

    }

    public enum BookType
    {
        Finance,
        Programming,
        Language,
        Story,
        Novels

    }

    public enum KeepType
    {
        Rent,
        Purchase
    }
}
