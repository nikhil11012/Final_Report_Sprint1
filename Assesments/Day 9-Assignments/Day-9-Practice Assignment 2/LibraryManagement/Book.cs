using System;

namespace LibraryManagement
{
    /// <summary>
    /// Represents a book in the library.
    /// </summary>
    public class Book
    {
        // Book details
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool IsBorrowed { get; set; }

        // Constructor to set up a new book
        public Book(string title, string author, string isbn)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            IsBorrowed = false; // new books are available by default
        }

        /// <summary>
        /// Marks the book as borrowed.
        /// Throws an exception if the book is already borrowed.
        /// </summary>
        public void Borrow()
        {
            if (IsBorrowed)
            {
                throw new InvalidOperationException("This book is already borrowed.");
            }
            IsBorrowed = true;
        }

        /// <summary>
        /// Marks the book as returned (available).
        /// Throws an exception if the book was not borrowed.
        /// </summary>
        public void Return()
        {
            if (!IsBorrowed)
            {
                throw new InvalidOperationException("This book was not borrowed.");
            }
            IsBorrowed = false;
        }
    }
}
