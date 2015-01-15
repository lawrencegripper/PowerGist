using GistsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripDev.PowerGist.Addin
{
    public class GistRepository
    {
        private GistClient client;
        public GistRepository(GistClient client)
        {
            if (client == null)
            {
                throw new ArgumentException("No client provided");
            }

            this.client = client;
        }

        public async Task<GistObject> Create(string description, string filename, string content)
        {
            return await client.CreateAGist(description, false, new List<Tuple<string, string>>()
            {
                new Tuple<string, string>(filename, content)
            });
        }

        public async Task<IEnumerable<GistObject>> GetUsers()
        {
            return await client.ListGists(GistClient.ListMode.AuthenticatedUserGist);
        }

        public async Task<IEnumerable<GistObject>> GetStarred()
        {
            return await client.ListGists(GistClient.ListMode.AuthenticatedUserStarredGist);
        }

        public async Task<GistObject> GetById(string id)
        {
            return await client.GetSingleGist(id);
        }

        public async Task<string> GetFileContentByUri(string rawUri)
        {
            return await client.DownloadRawText(new Uri(rawUri));
        }

        public async Task Update(GistObject obj, string filename, string content)
        {
            await client.EditAGist(obj.id, obj.description, filename, content);
        }

        public async void Delete(string id)
        {
            await client.DeleteAGist(id);
        }

        public async void DeleteFile(string id, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
