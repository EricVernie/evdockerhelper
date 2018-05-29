using ev.docker.host.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ev.docker.helper;

using Docker.DotNet.Models;

namespace ev.docker.host.ViewModel
{
    public class DockerViewModel : BindableBase
    {
        IList<ImagesListResponse> _images;
        public IList<ImagesListResponse> Images { get { return _images; } set { this.SetProperty(ref _images,value); } }


        IList<ContainerListResponse> _containers;
        public IList<ContainerListResponse> Containers { get { return _containers; } set { this.SetProperty(ref _containers, value); } }

        public DockerViewModel()
        {
            DockerServiceCore.Instance.Initialize("npipe://./pipe/docker_engine");
            
        }

        public async Task GetImagesAsync()
        {
            _images = await DockerServiceCore.Instance.GetImagesAsync();
        }

        public async Task DeleteImageAsync(string reposTag)
        {
            await DockerServiceCore.Instance.DeleteImageAsync(reposTag);
            await DockerServiceCore.Instance.GetImagesAsync();
        }

        public async Task RemoveContainerAsync(string id)
        {
            await DockerServiceCore.Instance.RemoveContainerAsync(id);
        }

        public async Task GetContainersAsync()
        {
            _containers = await DockerServiceCore.Instance.GetContainersAsync();
        }
    }
}
