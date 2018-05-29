using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ev.docker.helper
{
    public class DockerServiceCore
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>

        private static DockerServiceCore _instance;
        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        /// 
        public static DockerServiceCore Instance => _instance ?? (_instance = new DockerServiceCore());

        private DockerClient _dockerProvider;
        public DockerClient Provider { get { return _dockerProvider; } }

        public bool Initialize(string uri)
        {
            _dockerProvider = new DockerClientConfiguration(new Uri(uri))
                                    .CreateClient();

            return true;
        }

        public async Task<IList<ImagesListResponse>> GetImagesAsync()
        {
            return  await _dockerProvider.Images.ListImagesAsync(new ImagesListParameters()
            {                
                All = true
            }).ConfigureAwait(false);

        }
        public async Task<IList<ContainerListResponse>> GetContainersAsync()
        {
            return await _dockerProvider.Containers.ListContainersAsync(new ContainersListParameters()
            {
                All = true
            }).ConfigureAwait(false);

        }
        public async Task RemoveContainerAsync(string id)
        {
            await _dockerProvider.Containers.RemoveContainerAsync(id, new ContainerRemoveParameters()
            {
                Force = true
            });
        }

        public async Task DeleteImageAsync(string reposTag)
        {
            await _dockerProvider.Images.DeleteImageAsync(reposTag, new ImageDeleteParameters()
            {
                Force = true
            });
        }
    }
}
