using System;

namespace BookLibrary1.Model
{
    public class BookRecords
    {
        public string ISBN { get; set; }
        public int BookID { get; set; }
        public string Title { get; set; }
        public int? Amount { get; set; }
        public KeepType KeepType { get; set; }

        private DateTime? _ValidTill;

        public DateTime? ValidTill
        {
            get { return _ValidTill; }
            set { _ValidTill = value; }
        }

        public DateTime? PurchasedOn { get; set; }
        public bool HasExpiry { get; set; }

        private string _OwnBorrow;

        public string OwnBorrow
        {
            get { return _OwnBorrow; }
            set { _OwnBorrow = value; }
        }


    }
}
