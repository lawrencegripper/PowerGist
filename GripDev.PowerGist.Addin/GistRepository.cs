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

        public async Task<IEnumerable<GistObject>> GetMyGists()
        {
            return await client.ListGists(GistClient.ListMode.AuthenticatedUserGist);
        }

        public async Task<IEnumerable<GistObject>> GetStarredGists()
        {
            return await client.ListGists(GistClient.ListMode.AuthenticatedUserStarredGist);
        }

        public async Task<GistObject> GetById(string id)
        {
            return await client.GetSingleGist(id);
        }

        public async void SaveGist(GistObject obj, string filename, string content)
        {
            await client.EditAGist(obj.id, obj.description, filename, content);
        }

        public async void DeleteGist(string id)
        {
            await client.DeleteAGist(id);
        }

        public async void DeleteFile(string id, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
