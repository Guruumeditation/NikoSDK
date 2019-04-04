using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Net.ArcanaStudio.NikoSDK.Converters;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Model;
using Net.ArcanaStudio.NikoSDK.Model.Commands;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;

namespace Net.ArcanaStudio.NikoSDK.Tests
{
    [TestClass]
    public class NikoClientTests
    {
        #region Instanciation Tests

        [TestMethod]
        [TestCategory("Instanciation")]
        public void Instanciation_With_Wrong_IP_Address()
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var client = new NikoClient("@@@@@");
            }
            catch (Exception e)
            {
                e.Should().BeOfType<ArgumentException>();
                return;
            }
          
            Assert.Fail("Exception should have been raised.");
        }

        [TestMethod]
        [TestCategory("Instanciation")]
        public void Instanciation_With_Null_ITcpClient()
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var client = new NikoClient((ITcpClient) null);
            }
            catch (Exception e)
            {
                e.Should().BeOfType<ArgumentNullException>();
                return;
            }

            Assert.Fail("Exception should have been raised.");
        }

        [TestMethod]
        [TestCategory("Instanciation")]
        public void Instanciation_With_ITcpClient_OK()
        {
            var tcpclientmock = new Mock<ITcpClient>();

            var client = new NikoClient(tcpclientmock.Object);

            client.Should().NotBeNull();
            client.IsConnected.Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Instanciation")]
        public void Instanciation_With_IP_OK()
        {
            var client = new NikoClient("127.0.0.1");

            client.Should().NotBeNull();
            client.IsConnected.Should().BeFalse();
        }

        #endregion

        #region StartStop

        [TestMethod]
        [TestCategory("StartStop")]
        public void Start_Client_Connects()
        {
            var tcs = new TaskCompletionSource<int>();
            var isconnected = false;
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => isconnected);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => tcs.Task);

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            isconnected = true;

            client.Should().NotBeNull();
            tcpclientmock.Verify(c => c.Start(),Times.Once);
            tcpclientmock.VerifyGet(c => c.IsConnected,Times.Once);
            client.IsConnected.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("StartStop")]
        public void Get_Client_IP()
        {
            var tcs = new TaskCompletionSource<int>();
            var isconnected = false;
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => isconnected);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => tcs.Task);
            tcpclientmock.SetupGet(c => c.IpAddress).Returns(IPAddress.Loopback);

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            isconnected = true;

            client.Should().NotBeNull();
            client.IpAddress.Should().Be(IPAddress.Loopback);
        }

        [TestMethod]
        [TestCategory("StartStop")]
        public void Stop_Client_Without_Start_First_OK()
        {
            var tcpclientmock = new Mock<ITcpClient>();
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);

            var client = new NikoClient(tcpclientmock.Object);
            client.StopClient();

            client.Should().NotBeNull();
            tcpclientmock.Verify(c => c.Start(), Times.Never);
            tcpclientmock.Verify(c => c.Stop(), Times.Never);
            tcpclientmock.VerifyGet(c => c.IsConnected, Times.Once);
            client.IsConnected.Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("StartStop")]
        public void Start_Client_Twice_OK()
        {
            var tcs = new TaskCompletionSource<int>();
            var isconnected = false;
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => isconnected);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => tcs.Task);

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            isconnected = true;
            client.StartClient();

            client.Should().NotBeNull();
            tcpclientmock.Verify(c => c.Start(), Times.Once);
            tcpclientmock.Verify(c => c.Stop(), Times.Never);
            tcpclientmock.VerifyGet(c => c.IsConnected, Times.Exactly(2));
            client.IsConnected.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("StartStop")]
        public void Start_Stop_Client()
        {
            var tcs = new TaskCompletionSource<int>();
            var isconnected = false;
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => isconnected);
            tcpclientmock.Setup(c => c.Stop()).Callback(() => isconnected = false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => tcs.Task);

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            isconnected = true;
            client.StopClient();

            client.Should().NotBeNull();
            tcpclientmock.Verify(c => c.Start(), Times.Once);
            tcpclientmock.Verify(c => c.Stop(), Times.Once);
            tcpclientmock.VerifyGet(c => c.IsConnected, Times.Exactly(2));
            client.IsConnected.Should().BeFalse();
        }

        #endregion

        #region ReadData

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/GetSystemInfo_OK.json", "Json")]
        public async Task Get_SystemInfo_OK()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<SystemInfo>>(@"Json\GetSystemInfo_OK.json", true, new SystemInfoConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[],int,int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b,o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.GetSystemInfo();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Data.Should().NotBeNull();
            response.Data.Should().Be(responsemodel.Data);
        }



        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/GetSystemInfo_Error.json", "Json")]
        public async Task Get_SystemInfo_Error()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<SystemInfo>>(@"Json\GetSystemInfo_Error.json", true, new SystemInfoConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.GetSystemInfo();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Data.Should().BeNull();
            response.IsError.Should().BeTrue();
            response.Error.Should().Be(responsemodel.Error);
        }

        [TestMethod]
        [TestCategory("Get data")]
        public async Task Get_SystemInfo_Exception()
        {
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new TaskCompletionSource<int>().Task);

            tcpclientmock.Setup(c => c.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws<ArgumentNullException>();

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            Func<Task> act = async () => await client.GetSystemInfo();

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/GetActions_OK.json", "Json")]
        public async Task Get_Actions_OK()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<IReadOnlyList<IAction>>>(@"Json\GetActions_OK.json", true, new ActionsConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.GetActions();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(responsemodel.Data.Count);
            response.Data.Should().Equal(responsemodel.Data);
        }



        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/GetActions_Error.json", "Json")]
        public async Task Get_Actions_Error()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<IReadOnlyList<IAction>>>(@"Json\GetActions_Error.json", true, new ActionsConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.GetActions();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Data.Should().BeNull();
            response.IsError.Should().BeTrue();
            response.Error.Should().Be(responsemodel.Error);
        }

        [TestMethod]
        [TestCategory("Get data")]
        public async Task Get_Actions_Exception()
        {
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new TaskCompletionSource<int>().Task);

            tcpclientmock.Setup(c => c.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws<ArgumentNullException>();

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            Func<Task> act = async () => await client.GetActions();

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/GetLocations_OK.json", "Json")]
        public async Task Get_Locations_OK()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<IReadOnlyList<ILocation>>>(@"Json\GetLocations_OK.json", true, new LocationsConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.GetLocations();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(responsemodel.Data.Count);
            response.Data.Should().Equal(responsemodel.Data);
        }



        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/GetLocations_Error.json", "Json")]
        public async Task Get_Locations_Error()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<IReadOnlyList<ILocation>>>(@"Json\GetLocations_Error.json", true, new LocationsConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.GetLocations();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Data.Should().BeNull();
            response.IsError.Should().BeTrue();
            response.Error.Should().Be(responsemodel.Error);
        }

        [TestMethod]
        [TestCategory("Get data")]
        public async Task Get_Locations_Exception()
        {
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new TaskCompletionSource<int>().Task);

            tcpclientmock.Setup(c => c.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws<ArgumentNullException>();

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            Func<Task> act = async () => await client.GetLocations();

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/StartEvents_OK.json", "Json")]
        public async Task StartEvents_OK()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<ErrorImp>(@"Json\StartEvents_OK.json", true, new BaseResponseConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.StartEvents();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.IsError.Should().BeFalse();
            response.Error.Should().Be(responsemodel.Error);
        }



        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/StartEvents_Error.json", "Json")]
        public async Task StartEvents_Error()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<ErrorImp>>(@"Json\StartEvents_Error.json", true, new BaseResponseConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.StartEvents();
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Error.Should().Be(responsemodel.Data.Error);
        }

        [TestMethod]
        [TestCategory("Get data")]
        public async Task StartEvents_Exception()
        {
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new TaskCompletionSource<int>().Task);

            tcpclientmock.Setup(c => c.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws<ArgumentNullException>();

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            Func<Task> act = async () => await client.StartEvents();

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/ExecuteCommand_OK.json", "Json")]
        public async Task ExecuteCommand_OK()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<ErrorImp>(@"Json\ExecuteCommand_OK.json", true, new BaseResponseConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.ExecuteCommand(31,100);
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.IsError.Should().BeFalse();
            response.Error.Should().Be(responsemodel.Error);
        }

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/ExecuteCommand_Error.json", "Json")]
        public async Task ExecuteCommand_Error()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, responsemodel) = GetJson<NikoResponse<ErrorImp>>(@"Json\ExecuteCommand_Error.json", true, new BaseResponseConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            var responsetask = client.ExecuteCommand(31,100);
            tcs.SetResult(jsonbytes.Length);
            var response = await responsetask;

            response.Should().NotBeNull();
            response.Command.Should().Be(response.Command);
            response.Error.Should().Be(responsemodel.Data.Error);
        }

        [TestMethod]
        [TestCategory("Get data")]
        public async Task ExecuteCommand_Exception()
        {
            var tcpclientmock = new Mock<ITcpClient>();
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new TaskCompletionSource<int>().Task);

            tcpclientmock.Setup(c => c.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws<ArgumentNullException>();

            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            Func<Task> act = async () => await client.ExecuteCommand(31,0);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/Event.json", "Json")]
        public async Task Event_Received()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, _) = GetJson<ErrorImp>(@"Json\Event.json", true, new EventConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);
            client.StartClient();
            using (var minitoredclient = client.Monitor())
            {
                tcs.SetResult(jsonbytes.Length);
                minitoredclient.Should().Raise(nameof(NikoClient.OnValueChanged));
                await Task.Delay(1000);
            }
        }


        [TestMethod]
        [TestCategory("Get data")]
        [DeploymentItem("Json/Invalid_Json.json", "Json")]
        public async Task Invalid_JSON()
        {
            TaskCompletionSource<int> tcs = null;
            var tcpclientmock = new Mock<ITcpClient>();
            var (jsonbytes, _) = GetJson<NikoResponse<IReadOnlyList<IAction>>>(@"Json\Invalid_Json.json", false, new SystemInfoConverter());
            // ReSharper disable once AccessToModifiedClosure
            tcpclientmock.SetupGet(c => c.IsConnected).Returns(() => false);
            tcpclientmock.Setup(c => c.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                .Callback<byte[], int, int>((b, o, l) =>
                {
                    jsonbytes.CopyTo(b, o);
                })
                .Returns(() => (tcs = new TaskCompletionSource<int>()).Task);


            var client = new NikoClient(tcpclientmock.Object);

            client.StartClient();
            var sendtask = client.SendCommand<ISystemInfo>(new GetSystemInfoCommand());
            tcs.SetResult(jsonbytes.Length);
            Func<Task> act = async () => await sendtask;

            await act.Should().NotThrowAsync<JsonReaderException>();
        }

        #endregion

        #region Private methods

        private (byte[], T) GetJson<T>(string filename, bool deserialize, params JsonConverter[] converters) where T: class
        {
            var jsonstring = System.IO.File.ReadAllText(filename) + "\r\n";
            var model = deserialize ? JsonConvert.DeserializeObject<T>(jsonstring, converters) : null;
            var bytes = Encoding.ASCII.GetBytes(jsonstring);

            return (bytes, model);
        }

        #endregion
    }
}
