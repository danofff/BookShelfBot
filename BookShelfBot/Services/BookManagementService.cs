using BookShelfBot.Models;
using BookShelfBot.Repositories;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelfBot.Services
{
    public class BookManagementService
    {
        private readonly GenericRepository<Book> _bookRepository;
        private readonly GenericRepository<BookRequests> _bookRequestRepository;
        private static readonly Random _random = new Random();

        public BookManagementService()
        {
            _bookRepository = new GenericRepository<Book>();
            _bookRequestRepository = new GenericRepository<BookRequests>();
        }

        public byte [] GetBook()
        {
            var books = _bookRepository.ReadAll().ToArray();
            var index = _random.Next(0, books.Count());

            return GetBookFromBlob(books[index].BookUrl);
        }

        private byte[] GetBookFromBlob(string bookUrl)
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
            string sourceFile = null;
            string destinationFile = null;

            string connectionString = "";

            storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            cloudBlobContainer = cloudBlobClient.GetContainerReference("books");

            CloudBlockBlob blob = new CloudBlockBlob(new Uri(bookUrl), storageAccount.Credentials);

            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
                var length = memoryStream.Length;
                return memoryStream.ToArray();
            }
        }

        public bool CheckQuantityGet(int id)
        {
           var alreadyRequested=_bookRequestRepository.Read(r => r.Date == DateTime.Now.Date&&r.UserId==id).ToList().Count;
            if(alreadyRequested<3)
                return true;
            return false;
        }

        public void AddBookRequestRecord(int chatId)
        {
            BookRequests request =new BookRequests()
            {
                UserId = chatId,
                Date = DateTime.Now.Date
            };
            _bookRequestRepository.Add(request);
        }      
    }
}
