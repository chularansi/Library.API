using Blazored.LocalStorage;
using Library.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library.UI.Services
{
    public class AuthorRepository : BaseRepository<AuthorModel>, IAuthorRepository
    {
        private readonly IHttpClientFactory httpClient;
        private readonly ILocalStorageService localStorage;

        public AuthorRepository(IHttpClientFactory httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }
    }
}
