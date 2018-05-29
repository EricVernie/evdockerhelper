using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Docker.DotNet.Models;
using ev.docker.host.ViewModel;

namespace ev.docker.host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DockerViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new DockerViewModel();
            this.DataContext = _viewModel;
            lstViewImages.MouseDoubleClick += LstViewImages_MouseDoubleClick;
            lstViewContainers.MouseDoubleClick += LstViewContainers_MouseDoubleClick;
            
        }

      

        private async void LstViewContainers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                await _viewModel.RemoveContainerAsync(((ContainerListResponse)(lstViewContainers.SelectedItem)).ID);
               

                lstViewContainers.ItemsSource = _viewModel.Containers;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
            
        }

        private async void LstViewImages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var reposTag = ((ImagesListResponse)(lstViewImages.SelectedItem)).ID;
                await _viewModel.DeleteImageAsync(reposTag);                
                lstViewImages.ItemsSource = _viewModel.Images;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

        }

        private async void cmdGetImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.GetImagesAsync();
                lstViewImages.ItemsSource = _viewModel.Images;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void lstViewImages_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.GetContainersAsync();

                lstViewContainers.ItemsSource = _viewModel.Containers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            
        }

        private async void lstViewContainers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var container = GetCurrentContainer();
            if (container ==null)
            {
                return;
            }

            if (!container.State.Equals("running"))
            {
                return;
            }

            await _viewModel.ListContainerProcessAsync(container.ID);
            grdProcess.DataContext = _viewModel.EVProcesses;
            lstProcesses.ItemsSource = _viewModel.EVProcesses.Processes;
        }

        private ContainerListResponse GetCurrentContainer()
        {
            return (ContainerListResponse)lstViewContainers.SelectedItem;
        }

        private void cmdStopOrStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var container = GetCurrentContainer();

                _viewModel.StopOrStartContainerAsync(container);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
          
        }
    }
}
